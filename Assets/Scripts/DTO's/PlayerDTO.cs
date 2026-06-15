using System;
using System.Collections.Generic;

public class PlayerDTO
{
    public int RoomId;
    public string RoomName;
    public string Message;
    public bool RoomHasPassword;
    public bool Success;
    public List<Player> Players;

    public RoomInfo Room;

}

[Serializable]
public class Player
{
    public int ID;
    public string Name;
    public int LVL;
    public bool Ready;
}