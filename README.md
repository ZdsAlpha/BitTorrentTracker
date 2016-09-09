# BitTorrentTracker
You can easily interact with a bittorrent tracker on udp protocol using this library. For example:

        Dim tracker As New Tracker
        tracker.Connect("tracker.opentrackr.org", 1337)
        Dim name(19) As Byte
        Dim random As New Random
        random.NextBytes(name)
        Dim result = tracker.Announce({0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20}, name,,,,,,, 8080)