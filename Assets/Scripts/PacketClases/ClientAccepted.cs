
using UnityEngine;

public class ClientAccepted : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedAccept;

    private UIManager _uiManager;

    public ClientAccepted(UIManager _ui) => _uiManager = _ui;

    public void Handle(Packet payload, ISession session = null)
    {
        int id = payload.ReadInt();
        GameManager.Instance.ID = (uint)id;
        Debug.Log(payload.ReadString());
        _uiManager.GoToRoomList();
    }
}