

using Unity.VisualScripting;

public class PlayersRoom : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.PlayersInRoom;

    private RoomService _roomManager;

    public PlayersRoom(RoomService rm) => _roomManager = rm;

    public void Handle(Packet payload, ISession session = null)
    {
        bool json = payload.ReadBool();
        string playerDTO = payload.ReadString();
        _roomManager.PlayerLeave(playerDTO);
    }
}