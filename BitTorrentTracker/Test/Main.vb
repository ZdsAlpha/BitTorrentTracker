Imports System.Net
Imports Zds.BitTorrent

Module Main
    Sub Main()
        Dim Random As New Random
        Dim Tracker As New BitTorrentTracker
        Dim PeerId(20 - 1) As Byte
        Random.NextBytes(PeerId)
        Do
            Try
                Console.Write("Enter tracker and port: ")
                'tracker.opentrackr.org:1337                (https://openbittorrent.com/)
                Dim EndPoint As String = Console.ReadLine
                Dim Address As String = Split(EndPoint, ":")(0)
                Dim Port As Integer = Integer.Parse(Split(EndPoint, ":")(1))
                Console.Write("Enter hash: ")
                'E3DAC924B1A55989D3EE677E592942C1A43EE523   (https://unity3d.com/get-unity/download/archive)
                Dim HashString As String = Console.ReadLine
                Dim Hash As Byte() = HexToBin(HashString)
                If Tracker.IsConnected Then Tracker.Disconnect()
                Console.WriteLine("Connecting to " + EndPoint)
                Tracker.Connect(Address, Port)
                Console.WriteLine("Announcing " + HashString)
                Dim Result = Tracker.Announce(Hash, PeerId, BitTorrentTracker.EventTypeFilter.Started, 0)
                Console.WriteLine("Seeders: " + Result.Seeders.ToString)
                Console.WriteLine("Leechers: " + Result.Leechers.ToString)
                Console.WriteLine("IP List: ")
                For Each Client In Result.Clients
                    Console.Write(Client.Address.ToString + ":" + Client.Port.ToString + vbTab)
                Next
                Console.WriteLine("")
            Catch ex As Exception
                Console.WriteLine("An unhandled exception occured.")
                Console.WriteLine(ex.ToString)
            End Try
        Loop
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
