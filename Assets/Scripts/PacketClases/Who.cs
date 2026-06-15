public class Who : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.Who;
    private GameService _gameService;

    public Who(GameService gs) => _gameService = gs;

    public void Handle(Packet payload, ISession session = null)
    {
        bool rr = payload.ReadBool();
        string who = payload.ReadString();
        _gameService.SetWho(who);
    }
}