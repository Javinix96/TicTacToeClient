public class Counter : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.Counter;

    private RoomService _service;

    public Counter(RoomService service) => _service = service;

    public void Handle(Packet payload, ISession session = null)
    {
        int counter = payload.ReadInt();
        _service.UpdateTxt(counter);
    }
}