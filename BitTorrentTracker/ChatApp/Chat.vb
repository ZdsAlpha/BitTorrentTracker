Imports Zds.BitTorrent
Public Class Chat
    Private _tracker As Tracker
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        _tracker = New Tracker
        _tracker.Timeout = 5

    End Sub
End Class
