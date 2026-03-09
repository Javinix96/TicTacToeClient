public enum PacketTypeReceive
{
    Error = 999,
    Welcome,
    Message,
    RoomList,
    PlayersInRoom,
    JoinRoom
}
public enum PacketTypeSend
{
    Error = 0,
    Welcome,
    Message,
    RequestRooms,
    createRoom,
    Players,
    JoinRoomRequest
}