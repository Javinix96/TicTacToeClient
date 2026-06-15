public class Turn : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedTurn;

    private GameService _gameService;

    public Turn(GameService gm) => _gameService = gm;

    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        string player = payload.ReadString();
        _gameService.PlayerTurn(player);
    }
}