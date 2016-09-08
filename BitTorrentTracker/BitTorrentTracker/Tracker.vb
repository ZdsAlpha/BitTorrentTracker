Imports System.Net
Imports System.Net.Sockets
Imports MiscUtil.IO

Public Class Tracker
    Private client As New UdpClient
    Private bitconverter As New MiscUtil.Conversion.BigEndianBitConverter
    Private random As New Random
    Private EndPoint As IPEndPoint
    Private connection_id As Long = &H41727101980L
    Public WriteOnly Property Timeout As Integer
        Set(value As Integer)
            client.Client.SendTimeout = value
            client.Client.ReceiveTimeout = value
        End Set
    End Property
    Public Sub Connect(EndPoint As IPEndPoint)
        Dim transaction_id As Integer = random.Next(65535)
        Dim request As New IO.MemoryStream
        Dim writer As New EndianBinaryWriter(bitconverter, request)
        writer.Write(connection_id)
        writer.Write(0)
        writer.Write(transaction_id)
        Dim rawrequest As Byte() = request.ToArray
        writer.Dispose()
        request.Dispose()
        If client.Send(rawrequest, rawrequest.Length, EndPoint) <> rawrequest.Length Then Throw New Exception("Packet was not sent properly.")
        Dim source As IPEndPoint = Nothing
Retry:
        Dim rawresponse As Byte() = client.Receive(source)
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
        connection_id = reader.ReadInt64
        Me.EndPoint = EndPoint
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
        If client Is Nothing Then Throw New Exception("Tracker is disposed.")
        If EndPoint Is Nothing Or connection_id = &H41727101980L Then Throw New Exception("Not connected to tracker.")
        If Hash.Length <> 20 Then Throw New Exception("Invalid hash.")
        If PeerId.Length <> 20 Then Throw New Exception("Invalid PeerId.")
        Dim transaction_id As Integer = random.Next(65535)
        Dim key As Integer = random.Next(65535)
        Dim request As New IO.MemoryStream
        Dim writer As New EndianBinaryWriter(bitconverter, request)
        writer.Write(connection_id)
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
        If client.Send(rawrequest, rawrequest.Length, EndPoint) <> rawrequest.Length Then Throw New Exception("")
        Dim source As IPEndPoint = Nothing
Retry:
        Dim rawresponse As Byte() = client.Receive(source)
        If source Is Nothing OrElse rawresponse Is Nothing Then Throw New Exception("Failed to get response from server.")
        If Not EndPoint.Equals(source) Then
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
        Return New AnnounceResult With {.Interval = interval, .Leechers = leechers, .Seeders = seeders, .Clients = clients.ToArray}
    End Function
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
