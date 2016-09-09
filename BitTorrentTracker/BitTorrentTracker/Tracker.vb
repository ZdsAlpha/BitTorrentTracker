Imports System.ComponentModel
Imports System.Net
Imports System.Net.Sockets
Imports MiscUtil.IO

Public Class Tracker
    Implements IDisposable
    Private Shared bitconverter As New MiscUtil.Conversion.BigEndianBitConverter
    Private Shared random As New Random
    Private _client As UdpClient
    Private _endpoint As IPEndPoint
    Private _connectionid As Long = &H41727101980L
    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return _client Is Nothing
        End Get
    End Property
    Public ReadOnly Property IsConnected As Boolean
        Get
            If _endpoint IsNot Nothing AndAlso _connectionid <> &H41727101980L Then Return True
            Return False
        End Get
    End Property
    Public ReadOnly Property Client As UdpClient
        Get
            Return _client
        End Get
    End Property
    Public ReadOnly Property EndPoint As IPEndPoint
        Get
            Return _endpoint
        End Get
    End Property
    Public ReadOnly Property ConnectionId As Long
        Get
            Return _connectionid
        End Get
    End Property
    Public WriteOnly Property Timeout As Integer
        Set(value As Integer)
            _client.Client.SendTimeout = value
            _client.Client.ReceiveTimeout = value
        End Set
    End Property
    Public Sub Connect(EndPoint As IPEndPoint)
        Dim transaction_id As Integer = random.Next(65535)
        Dim request As New IO.MemoryStream
        Dim writer As New EndianBinaryWriter(bitconverter, request)
        writer.Write(_connectionid)
        writer.Write(0)
        writer.Write(transaction_id)
        Dim rawrequest As Byte() = request.ToArray
        writer.Dispose()
        request.Dispose()
        If _client.Send(rawrequest, rawrequest.Length, EndPoint) <> rawrequest.Length Then Throw New Exception("Packet was not sent properly.")
        Dim source As IPEndPoint = Nothing
Retry:
        Dim rawresponse As Byte() = _client.Receive(source)
        If source Is Nothing OrElse rawresponse Is Nothing Then Throw New Exception("Failed to get response from server.")
        If Not EndPoint.Equals(source) Then
            source = Nothing
            GoTo Retry
        End If
        If rawresponse.Length < 16 Then Throw New Exception("Invalid response from server.")
        Dim response As New IO.MemoryStream(rawresponse)
        Dim reader As New EndianBinaryReader(bitconverter, response)
        If reader.ReadInt32 <> 0 Then Throw New Exception("Invalid return parameter 'action'.")
        If reader.ReadInt32 <> transaction_id Then Throw New Exception("Invalid transaction id or packet is corrupted.")
        _connectionid = reader.ReadInt64
        Me._endpoint = EndPoint
        reader.Dispose()
        response.Dispose()
    End Sub
    Public Sub Connect(IPAddress As IPAddress, Port As Integer)
        Connect(New IPEndPoint(IPAddress, Port))
    End Sub
    Public Sub Connect(Host As String, Port As Integer)
        Connect(Dns.GetHostEntry(Host).AddressList(0), Port)
    End Sub
    Public Function Announce(Hash As Byte(), PeerId As Byte(), Optional Downloaded As Long = 0, Optional Remaining As Long = 0, Optional Uploaded As Long = 0, Optional [Event] As EventTypeFilter = EventTypeFilter.Started, Optional IPAddress As IPAddress = Nothing, Optional Count As Integer = -1, Optional Port As Short = 0) As AnnounceResult
        If IPAddress Is Nothing Then IPAddress = New IPAddress(0)
        If _client Is Nothing Then Throw New Exception("Tracker is disposed.")
        If _endpoint Is Nothing Or _connectionid = &H41727101980L Then Throw New Exception("Not connected to tracker.")
        If Hash.Length <> 20 Then Throw New Exception("Invalid hash.")
        If PeerId.Length <> 20 Then Throw New Exception("Invalid PeerId.")
        Dim transaction_id As Integer = random.Next(65535)
        Dim key As Integer = random.Next(65535)
        Dim request As New IO.MemoryStream
        Dim writer As New EndianBinaryWriter(bitconverter, request)
        writer.Write(_connectionid)
        writer.Write(1)
        writer.Write(transaction_id)
        writer.Write(Hash)
        writer.Write(PeerId)
        writer.Write(Downloaded)
        writer.Write(Remaining)
        writer.Write(Uploaded)
        writer.Write([Event])
        writer.Write(bitconverter.ToInt32(IPAddress.GetAddressBytes().Reverse.ToArray, 0))
        writer.Write(key)
        writer.Write(Count)
        writer.Write(Port)
        Dim rawrequest As Byte() = request.ToArray
        writer.Dispose()
        request.Dispose()
        If _client.Send(rawrequest, rawrequest.Length, _endpoint) <> rawrequest.Length Then Throw New Exception("")
        Dim source As IPEndPoint = Nothing
Retry:
        Dim rawresponse As Byte() = _client.Receive(source)
        If source Is Nothing OrElse rawresponse Is Nothing Then Throw New Exception("Failed to get response from server.")
        If Not _endpoint.Equals(source) Then
            source = Nothing
            GoTo Retry
        End If
        If rawresponse.Length < 20 Then Throw New Exception("Invalid response from server.")
        Dim response As New IO.MemoryStream(rawresponse)
        Dim reader As New EndianBinaryReader(bitconverter, response)
        If reader.ReadInt32 <> 1 Then Throw New Exception("Invalid return parameter 'action'.")
        If reader.ReadInt32 <> transaction_id Then Throw New Exception("Invalid transaction id or packet is corrupted.")
        Dim interval As Integer = reader.ReadInt32
        Dim leechers As Integer = reader.ReadInt32
        Dim seeders As Integer = reader.ReadInt32
        Dim clients As New List(Of IPEndPoint)
        While Not response.Position = response.Length
            Dim IP As New IPAddress(System.BitConverter.GetBytes(reader.ReadInt32).Reverse().ToArray)
            Dim P As Short = reader.ReadInt16
            clients.Add(New IPEndPoint(IP, P))
        End While
        reader.Dispose()
        response.Dispose()
        Return New AnnounceResult With {.Interval = interval, .Leechers = leechers, .Seeders = seeders, .Clients = clients.ToArray}
    End Function
    Public Function Scrape(ParamArray Hashes As Byte()()) As ScrapeInfo()
        If _client Is Nothing Then Throw New Exception("Tracker is disposed.")
        If _endpoint Is Nothing Or _connectionid = &H41727101980L Then Throw New Exception("Not connected to tracker.")
        If Hashes Is Nothing OrElse Hashes.Length = 0 Then Throw New Exception("Add atleast one hash.")
        For Each Hash In Hashes
            If Hash.Length <> 20 Then Throw New Exception("Hash size must be 20.")
        Next
        Dim transaction_id As Integer = random.Next(65535)
        Dim request As New IO.MemoryStream
        Dim writer As New EndianBinaryWriter(bitconverter, request)
        writer.Write(_connectionid)
        writer.Write(2)
        writer.Write(transaction_id)
        For Each Hash In Hashes
            writer.Write(Hash)
        Next
        Dim rawrequest As Byte() = request.ToArray

    End Function
    Public Sub Dispose() Implements IDisposable.Dispose
        _client.Close()
        _client = Nothing
        _connectionid = &H41727101980L
    End Sub
    Sub New()
        _client = New UdpClient
    End Sub
    Sub New(Client As UdpClient)
        _client = Client
    End Sub
    Public Structure ScrapeInfo
        Public Hash As Byte()
        Public Seeders As Integer
        Public Completed As Integer
        Public Leechers As Integer
    End Structure
    Public Structure AnnounceResult
        Public Interval As Integer
        Public Leechers As Integer
        Public Seeders As Integer
        Public Clients As IPEndPoint()
    End Structure
    Public Enum EventTypeFilter As Integer
        None = 0
        Completed = 1
        Started = 2
        Stopped = 3
    End Enum
End Class
