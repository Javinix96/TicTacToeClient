using UnityEngine;

public class JoinRoom : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedRequestJoinToRoom;
    private UIManager _UIManager;

    public JoinRoom(UIManager uim) => _UIManager = uim;

    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        string dto = payload.ReadString();
        _UIManager.GoToJoinRoom(JsonUtility.FromJson<PlayerDTO>(dto));
    }
}