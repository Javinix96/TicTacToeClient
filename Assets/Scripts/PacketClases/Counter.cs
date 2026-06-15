public class Counter : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.Counter;

    private UIManager _uiManager;

    public Counter(UIManager uim) => _uiManager = uim;

    public void Handle(Packet payload, ISession session = null)
    {
        int counter = payload.ReadInt();
        _uiManager.onStartGame?.Invoke(counter);
    }
}