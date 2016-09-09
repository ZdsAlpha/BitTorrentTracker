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
End Class
