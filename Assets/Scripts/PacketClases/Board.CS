public class Board : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedBoard;

    private GameService _gameService;
    public Board(GameService gs) => _gameService = gs;

    public void Handle(Packet payload, ISession session = null)
    {
        payload.ReadBool();
        string board = payload.ReadString();
        _gameService.UpdateBoard(board);
    }
}