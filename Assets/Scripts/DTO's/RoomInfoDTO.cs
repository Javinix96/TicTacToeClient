using System;
using System.Collections.Generic;

public class RoomInfoDTO
{
    public int Count;
    public List<RoomInfo> Rooms;
}

[Serializable]
public class RoomInfo
{
    public int RoomId;
    public string RoomName;
    public int PlayersCount = 0;
}