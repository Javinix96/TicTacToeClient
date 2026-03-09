using System;
using System.Collections.Generic;

public class PlayerDTO
{
    public int RoomId;
    public string Message;
    public bool Success;
    public List<Player> Players;

}

[Serializable]
public class Player
{
    public int ID;
    public string Name;
    public int LVL;
}