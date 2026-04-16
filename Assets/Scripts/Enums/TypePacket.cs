public enum PacketTypeReceive
{
    Error = 999,
    ReceivedAccept,
    Welcome,
    Message,
    RoomList,
    ReceivedRoomCreated,
    PlayersInRoom,
    JoinRoom,
    LoadScene,
    Counter,
    Who,
    ReceivedTurn,
    ReceivedPosition,
    ReceivedBoard,
    RecieveWinner,
}
public enum PacketTypeSend
{
    Error = 0,
    Welcome,
    SendExit,
    Message,
    RequestRooms,
    createRoom,
    Players,
    JoinRoomRequest,
    ExitRoom,
    RequestJoin,
    PlayerReady,
    Position,
    SendReadyPos,

}