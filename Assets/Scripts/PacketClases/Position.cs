public class Position : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedPosition;

    private GameService _gameService;
    public Position(GameService gm) => _gameService = gm;

    public void Handle(Packet payload, ISession session = null)
    {
        string player = payload.ReadString();
        int index = payload.ReadInt();
        _gameService.OnReceivePosition(player, index);
    }
}