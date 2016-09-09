Imports System.Net
Imports BitTorrentTracker

Module Main
    Sub Main()
        Dim tracker As New Tracker
        tracker.Connect("tracker.coppersurfer.tk", 6969)
        Dim name(19) As Byte
        Dim random As New Random
        random.NextBytes(name)
        Dim result = tracker.Announce(HexToBin("fddb8e14462d92ab42c5b0cc47e821e16ac89493"), name,,,, Tracker.EventTypeFilter.Completed,,, 8080)
        Stop
    End Sub
    Public Function HexToBin(hex As String) As Byte()
        Dim NumberChars As Integer = hex.Length
        Dim bytes As Byte() = New Byte(NumberChars / 2 - 1) {}
        For i As Integer = 0 To NumberChars - 1 Step 2
            bytes(i / 2) = Convert.ToByte(hex.Substring(i, 2), 16)
        Next
        Return bytes
    End Function
End Module
