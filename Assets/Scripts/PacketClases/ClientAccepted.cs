using System.Diagnostics;

public class ClientAccepted : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedAccept;

    private UIManager _uiManager;

    public ClientAccepted(UIManager _ui) => _uiManager = _ui;

    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        _uiManager.GoToRoomList();
    }
}