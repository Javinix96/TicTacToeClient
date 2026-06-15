public class PlayerReady : IPacketHandler

{
    public int Header => (int)PacketTypeReceive.receivedPlayerReady;
    private UIManager _uiManager;

    public PlayerReady(UIManager uim) => _uiManager = uim;

    public void Handle(Packet payload, ISession session = null)
    {
        int playerWhoIsReady = payload.ReadInt();
        _uiManager.onPlayerReady?.Invoke(playerWhoIsReady);
    }
}