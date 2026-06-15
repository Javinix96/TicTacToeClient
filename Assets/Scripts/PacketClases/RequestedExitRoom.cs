public class RequestedExitRoom : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedExitRoom;

    private UIManager _UIManager;

    public RequestedExitRoom(UIManager uim) => _UIManager = uim;

    public void Handle(Packet payload, ISession session = null)
    {
        int times = payload.ReadInt();
        _UIManager.BackTimes(times);
    }
}