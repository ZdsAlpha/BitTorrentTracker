Imports System.Net
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports Zds.BitTorrent
Public Class Chat
    Private _listener As UdpClient = Nothing
    Private _tracker As Tracker = Nothing
    Private _clients As IPEndPoint() = Nothing
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles connectbutton.Click
        Try
            _listener = New UdpClient
            Dim opener As New Tracker(_listener)
            opener.Connect(Split(Trackers.Text, ":")(0), Integer.Parse(Split(Trackers.Text, ":")(1)))
            _tracker = New Tracker
            _tracker.Connect(Split(Trackers.Text, ":")(0), Integer.Parse(Split(Trackers.Text, ":")(1)))
            checkbutton.Enabled = True
            channel.Enabled = True
            announcebutton.Enabled = True
        Catch ex As Exception
            _listener = Nothing
            _tracker = Nothing
            connectbutton.Enabled = True
            Trackers.Enabled = True
            checkbutton.Enabled = False
            announcebutton.Enabled = False
            sendbutton.Enabled = False
            channel.Enabled = False
            message.Enabled = False
            MsgBox("Unable to connect tracker.")
        End Try
    End Sub
    Private Sub checkbutton_Click(sender As Object, e As EventArgs) Handles checkbutton.Click
        Try
            Dim hasher As New SHA1CryptoServiceProvider
            Dim hash As Byte() = hasher.ComputeHash(System.Text.Encoding.ASCII.GetBytes(channel.Text))
            Dim result = _tracker.Scrape(hash)
            MsgBox(result(0).Seeders.ToString + " people have announced to this channel.")
        Catch ex As Exception
            MsgBox("Unable to fetch channel info.")
        End Try
    End Sub
    Private Sub announcebutton_Click(sender As Object, e As EventArgs) Handles announcebutton.Click
        Try
            Dim hasher As New SHA1CryptoServiceProvider
            Dim hash As Byte() = hasher.ComputeHash(System.Text.Encoding.ASCII.GetBytes(channel.Text))
            Dim id(19) As Byte
            Dim r As New Random
            r.NextBytes(id)
            Dim ipendpoint As IPEndPoint = _listener.Client.LocalEndPoint
            Dim result = _tracker.Announce(hash, id,,,,,,, ipendpoint.Port)
            message.Enabled = True
            sendbutton.Enabled = True
            _clients = result.Clients
        Catch ex As Exception
            MsgBox("Unable to announce.")
        End Try
    End Sub

    Private Sub sendbutton_Click(sender As Object, e As EventArgs) Handles sendbutton.Click
        Dim packet As New IO.MemoryStream
        Dim writer As New IO.BinaryWriter(packet)
        writer.Write(name.Text)
        writer.Write(message.Text)
        Dim raw As Byte() = packet.ToArray
        For Each client In _clients
            _listener.Send(raw, raw.Length, client)
        Next
    End Sub
End Class
