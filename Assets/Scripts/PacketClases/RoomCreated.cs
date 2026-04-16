using UnityEngine;

public class RoomCreated : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedRoomCreated;

    private UIManager _UIManager;

    public RoomCreated(UIManager uim) => _UIManager = uim;
    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        string json = payload.ReadString();
        _UIManager.Lobby(JsonUtility.FromJson<PlayerDTO>(json));
    }
}