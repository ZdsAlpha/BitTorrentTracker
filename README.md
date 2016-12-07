# BitTorrentTracker
You can easily interact with a bittorrent tracker on udp protocol using this library. For example:

        Dim Tracker As New Tracker
        Tracker.Connect("tracker.opentrackr.org", 1337)
        Dim Name(20-1) As Byte
        Dim Random As New Random
        Random.NextBytes(Name)
        Dim Result = tracker.Announce({0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20}, Name,,,,,,, 8080)

Implemented from: bittorrent.org/beps/bep_0015.html
