public class JoinRoom : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.JoinRoom;
    private RoomService _service;

    public JoinRoom(RoomService service) => _service = service;

    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        string dto = payload.ReadString();
        _service.PlayerJoinRoom(dto);
    }
}