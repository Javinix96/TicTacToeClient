using System;
using UnityEngine;

public class Message : IPacketHandler
{
    private RoomService _roomService;
    public int Header => (int)PacketTypeReceive.Message;


    public Message(RoomService rs = null)
    {
        _roomService = rs;
    }
    public void Handle(Packet payload, ISession session = null)
    {
        try
        {
            bool isJson = payload.ReadBool();
            string message = payload.ReadString();
            if (isJson)
                _roomService.PlayerJoin(message);
            else
                Debug.Log(message);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
