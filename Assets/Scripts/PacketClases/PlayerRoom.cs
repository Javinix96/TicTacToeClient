

public class PlayersRoom : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.PlayersInRoom;

    private UIManager _UIManager;

    public PlayersRoom(UIManager uim) => _UIManager = uim;

    public void Handle(Packet payload, ISession session = null)
    {
        bool json = payload.ReadBool();
        string playerDTO = payload.ReadString();
        _UIManager.OnJoinRoom?.Invoke(playerDTO);
    }
}