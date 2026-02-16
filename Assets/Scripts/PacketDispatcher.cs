using System;
using System.Collections.Generic;
using UnityEngine;
public class PacketDispatcher
{
    public Dictionary<int, Action<Packet>> packetHandlers { private set; get; }

    public void Init()
    {
        packetHandlers = new()
        {
            { 1, HandleWelcome }
        };
    }

    private void HandleWelcome(Packet pck)
    {
        int id = pck.ReadInt();
        string msg = pck.ReadString();

        NetworkManager.NM.session._id = id;
        Debug.Log($"{msg} cliente({id}))");

        NetworkManager.NM.session.SendWelcome(msg);
    }
}