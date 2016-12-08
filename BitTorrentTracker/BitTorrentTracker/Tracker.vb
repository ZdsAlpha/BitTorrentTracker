Imports System.Net
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports MiscUtil.Conversion
Imports MiscUtil.IO
Public Class BitTorrentTracker
    Public Const SleepTime As Integer = 50
    Private _Stopwatch As New Stopwatch
    Private _BitConverter As New BigEndianBitConverter
    Private _Random As New RNGCryptoServiceProvider
    Private _Client As UdpClient
    Private _IPEndPoint As IPEndPoint = Nothing
    Private _ConnectionId As ULong = &H41727101980UL
    Public Event OnIrrelevantPacketReceived(Source As IPEndPoint, Packet As Byte())
    Public Property Timeout As TimeSpan = TimeSpan.FromSeconds(1)
    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return _Stopwatch Is Nothing Or _BitConverter Is Nothing Or _Random Is Nothing Or _Client Is Nothing
        End Get
    End Property
    Public ReadOnly Property IsConnected As Boolean
        Get
            Return IPEndPoint IsNot Nothing
        End Get
    End Property
    Public ReadOnly Property Client As UdpClient
        Get
            Return _Client
        End Get
    End Property
    Public ReadOnly Property IPEndPoint As IPEndPoint
        Get
            Return _IPEndPoint
        End Get
    End Property
    Public ReadOnly Property ConnectionId As ULong
        Get
            Return _ConnectionId
        End Get
    End Property
    Public Sub Connect(Host As String, Port As Integer)
        Dim Addresses As IPAddress() = Dns.GetHostAddresses(Host)
        If Addresses.Length < 1 Then Throw New SocketException("No such host is found.")
        Connect(New IPEndPoint(Addresses(0), Port))
    End Sub
    Public Sub Connect(IPEndPoint As IPEndPoint)
        SyncLock Client
            If IsDisposed Then Throw New ObjectDisposedException("Object is disposed and cannot be used. Renew object to use.")
            If IsConnected Then Disconnect()
            Dim Action As Action = Action.Connect
            Dim RequestId As UInteger = GenerateRequestId()
            Dim Request As EndianBinaryWriter = CreateRequest(Action, RequestId)
            Send(Request, IPEndPoint)
            Dim Response As EndianBinaryReader = Receive(IPEndPoint, 16, Action, RequestId)
            Request.Dispose()
            Dim ConnectionId As ULong = Response.ReadUInt64
            Response.Dispose()
            _IPEndPoint = IPEndPoint
            _ConnectionId = ConnectionId
        End SyncLock
    End Sub
    Public Sub Disconnect()
        _IPEndPoint = Nothing
        _ConnectionId = &H41727101980UL
    End Sub
    Public Function Announce(Hash As Byte(), PeerId As Byte(), Optional Downloaded As ULong = 0, Optional Remaining As ULong = 0, Optional Uploaded As ULong = 0, Optional [Event] As EventTypeFilter = EventTypeFilter.None, Optional IPAddress As IPAddress = Nothing, Optional Count As UInteger = UInteger.MaxValue, Optional Port As UShort = 0) As ClientsInfo
        SyncLock Client
            If IsDisposed Then Throw New ObjectDisposedException("Object is disposed and cannot be used. Renew object to use.")
            If Not IsConnected Then Throw New InvalidOperationException("Tracker is not connected. Try calling 'Connect'.")
            If Hash Is Nothing Then Throw New NullReferenceException("Hash cannot be null.")
            If PeerId Is Nothing Then Throw New NullReferenceException("PeerId cannot be null.")
            If Hash.Length <> 20 Then Throw New ArgumentException("'Hash' is invalid.")
            If PeerId.Length <> 20 Then Throw New ArgumentException("'PeerId' is not valid.")
            If IPAddress Is Nothing Then IPAddress = New IPAddress(0)
            Dim IPBytes As Byte() = IPAddress.GetAddressBytes
            If IPBytes.Length <> 4 Then Throw New ArgumentException("IPAddress is invalid. Only IPv4 is supported.")
            Dim Action As Action = Action.Announce
            Dim RequestId As UInteger = GenerateRequestId()
            Dim Request As EndianBinaryWriter = CreateRequest(Action, RequestId)
            Request.Write(Hash)
            Request.Write(PeerId)
            Request.Write(Downloaded)
            Request.Write(Remaining)
            Request.Write(Uploaded)
            Request.Write([Event])
            Request.Write(IPBytes)
            Request.Write(0)
            Request.Write(Count)
            Request.Write(Port)
            Send(Request, IPEndPoint)
            Dim Response As EndianBinaryReader = Receive(IPEndPoint, 20, Action, RequestId)
            Request.Dispose()
            Dim Interval As UInteger = Response.ReadUInt32
            Dim Leechers As UInteger = Response.ReadUInt32
            Dim Seeders As UInteger = Response.ReadUInt32
            Dim EndPoints As New List(Of IPEndPoint)
            While Response.BaseStream.Position < Response.BaseStream.Length
                Dim EndPointAddress As IPAddress = New IPAddress(Response.ReadBytes(4))
                Dim EndPointPort As UShort = Response.ReadUInt16
                EndPoints.Add(New IPEndPoint(EndPointAddress, EndPointPort))
            End While
            Response.Dispose()
            Dim Result As New ClientsInfo
            Result.Interval = Interval
            Result.Leechers = Leechers
            Result.Seeders = Seeders
            Result.Clients = EndPoints.ToArray
            Return Result
        End SyncLock
    End Function
    Public Function Announce(Hash As Byte(), PeerId As Byte(), [Event] As EventTypeFilter, Port As UShort) As ClientsInfo
        Return Announce(Hash, PeerId,,,, [Event],,, Port)
    End Function
    Public Function Scrape(ParamArray Hashes As Byte()()) As TorrentInfo()
        SyncLock Client
            If IsDisposed Then Throw New ObjectDisposedException("Object is disposed and cannot be used. Renew object to use.")
            If Not IsConnected Then Throw New InvalidOperationException("Tracker is not connected. Try calling 'Connect'.")
            If Hashes Is Nothing Then Return {}
            If Hashes.Length = 0 Then Return {}
            For Each Hash As Byte() In Hashes
                If Hash Is Nothing Then Throw New NullReferenceException("Hash cannot be null.")
                If Hash.Length <> 20 Then Throw New ArgumentException(("'Hash' is invalid."))
            Next
            Dim Action As Action = Action.Scrape
            Dim RequestId As UInteger = GenerateRequestId()
            Dim Request As EndianBinaryWriter = CreateRequest(Action, RequestId)
            For Each Hash As Byte() In Hashes
                Request.Write(Hash)
            Next
            Send(Request, IPEndPoint)
            Dim Response As EndianBinaryReader = Receive(IPEndPoint, 8, Action, RequestId)
            Request.Dispose()
            Dim TorrentsInfo(Hashes.Length - 1) As TorrentInfo
            For I = 0 To TorrentsInfo.Length - 1
                Dim Info As New TorrentInfo
                Info.Hash = Hashes(I)
                Info.Seeders = Response.ReadUInt32
                Info.Completed = Response.ReadUInt32
                Info.Leechers = Response.ReadUInt32
                TorrentsInfo(I) = Info
            Next
            Return TorrentsInfo
        End SyncLock
    End Function
    Private Function GenerateRequestId() As UInteger
        Dim Bytes(4 - 1) As Byte
        _Random.GetBytes(Bytes)
        Return _BitConverter.ToUInt32(Bytes, 0)
    End Function
    Private Function CreateRequest(Action As Action, RequestId As UInteger) As EndianBinaryWriter
        Dim Request As New IO.MemoryStream
        Dim Writer As New EndianBinaryWriter(_BitConverter, Request)
        Writer.Write(ConnectionId)
        Writer.Write(Action)
        Writer.Write(RequestId)
        Return Writer
    End Function
    Private Sub Send(Request As EndianBinaryWriter, IPEndPoint As IPEndPoint)
        Dim Memory As IO.MemoryStream = Request.BaseStream
        Dim Bytes As Byte() = Memory.ToArray
        If Client.Send(Bytes, Bytes.Length, IPEndPoint) <> Bytes.Length Then Throw New SocketException("Packet is not sent properly.")
    End Sub
    Private Function Receive(IPEndPoint As IPEndPoint, MinimumSize As Long, Action As Action, RequestId As UInteger) As EndianBinaryReader
        _Stopwatch.Restart()
        Do
            If Client.Available = 0 Then
                If _Stopwatch.Elapsed >= Timeout Then Throw New TimeoutException("Packet is not received in time.")
                Threading.Thread.Sleep(SleepTime)
            Else
                Dim Source As IPEndPoint = Nothing
                Dim Buffer As Byte() = Client.Receive(Source)
                If Source IsNot Nothing And Buffer IsNot Nothing Then
                    If IPEndPoint.Equals(Source) Then
                        Dim Reader As New EndianBinaryReader(_BitConverter, New IO.MemoryStream(Buffer))
                        If ValidateResponse(Reader, MinimumSize, Action, RequestId) Then
                            Return Reader
                        Else
                            Reader.Dispose()
                            RaiseEvent OnIrrelevantPacketReceived(Source, Buffer)
                        End If
                    Else
                        RaiseEvent OnIrrelevantPacketReceived(Source, Buffer)
                    End If
                End If
            End If
        Loop
    End Function
    Private Function ValidateResponse(Response As EndianBinaryReader, MinimumSize As Long, Action As Action, RequestId As UInteger) As Boolean
        If Response.BaseStream.Length < MinimumSize Then Return False
        If Response.ReadUInt32 <> Action Then Return False
        If Response.ReadUInt32 <> RequestId Then Return False
        Return True
    End Function
    Sub New()
        _Client = New UdpClient
    End Sub
    Sub New(Client As UdpClient)
        _Client = Client
    End Sub
    Public Structure TorrentInfo
        Public Hash As Byte()
        Public Seeders As UInteger
        Public Completed As UInteger
        Public Leechers As UInteger
    End Structure
    Public Structure ClientsInfo
        Public Interval As UInteger
        Public Leechers As UInteger
        Public Seeders As UInteger
        Public Clients As IPEndPoint()
    End Structure
    Public Enum EventTypeFilter As UInteger
        None = 0
        Completed = 1
        Started = 2
        Stopped = 3
    End Enum
    Public Enum Action As UInteger
        Connect = 0
        Announce = 1
        Scrape = 2
    End Enum
End Class