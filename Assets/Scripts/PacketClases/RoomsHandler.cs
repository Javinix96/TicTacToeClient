public class RoomsHandler : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.RoomList;

    private RoomService _roomService;

    public RoomsHandler(RoomService roomService) => _roomService = roomService;

    public void Handle(Packet payload, ISession session = null)
    {
        bool isJson = payload.ReadBool();
        if (!isJson)
            return;
        string roomInfo = payload.ReadString();
        _roomService.UpdateRooms(roomInfo);
    }
}