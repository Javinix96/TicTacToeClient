public class Winner : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.RecieveWinner;
    private GameService _gameService;
    public Winner(GameService gm) => _gameService = gm;

    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        string message = payload.ReadString();
        _gameService.PlayerWinner(message);
    }
}