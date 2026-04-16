
public class LoadScene : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.LoadScene;

    private RoomService _service;
    public LoadScene(RoomService rs)
    {
        _service = rs;
    }

    public void Handle(Packet payload, ISession session = null)
    {
        bool canJoin = payload.ReadBool();
        _service.LoadGame(canJoin);
    }
}