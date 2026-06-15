using UnityEngine;

public class RequestJoinToRoom : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedRequestJoinToRoom;

    private UIManager _uIManager;

    public RequestJoinToRoom(UIManager uim) => _uIManager = uim;

    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        string json = payload.ReadString();
        _uIManager.GoToJoinRoom(JsonUtility.FromJson<PlayerDTO>(json));
    }
}