using System;
using System.Threading.Tasks;
using UnityEngine;

public class RoomService
{
    private int roomID = 0;

    private UIManager _UIManager;
    private ISession session;

    public Action<RoomInfoDTO> OnRoomsUpdated;

    public Action<PlayerDTO> OnCreateRoom;

    public Action<PlayerDTO> OnRoomJoin;

    public Action<PlayerDTO> OnPlayerLeave;

    public Action<int> OnCounterUpdate;

    public event Func<bool, Task> LoadScene;
    public RoomService(ISession session, UIManager uiManager)
    {
        this.session = session;
        _UIManager = uiManager;
    }
    private PopUpController _popUpController;
    public void UpdateRooms(string roomInfo)
    {
        var dto = JsonUtility.FromJson<RoomInfoDTO>(roomInfo);
        _UIManager.OnRoomsRequest?.Invoke(dto);
    }

    public void RequestRooms()
    {
        using (Packet packet = new Packet())
        {
            packet.WriteInt((int)PacketTypeSend.RequestRooms);
            packet.WriteLength();
            session.send(packet);
        }
        ;
    }

    public void PlayerJoinRoomRequest(int roomID)
    {
        using (Packet packet = new Packet())
        {
            packet.WriteInt((int)PacketTypeSend.JoinRoomRequest);
            packet.WriteInt(roomID);
            packet.WriteLength();
            session.send(packet);
        }
    }

    public void CreateRoom(string roomName)
    {
        using (Packet packet = new Packet())
        {
            packet.WriteInt((int)PacketTypeSend.createRoom);
            packet.WriteString(roomName);
            packet.WriteLength();
            session.send(packet);
        }
    }

    public void GetPlayersFromServer(ClientSession session)
    {
        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.Players);
            pck.WriteLength();
            session.send(pck);
        }
    }

    public void PlayerJoin(string dto)
    {
        PlayerDTO players = JsonUtility.FromJson<PlayerDTO>(dto);
        if (!players.Success)
        {
            Debug.Log(players.Message);
            return;
        }
        roomID = players.RoomId;
        // _UIManager.GoToJoinRoom(players);

        // OnCreateRoom?.Invoke(players);
    }

    public void PlayerJoinRoom(string dto)
    {
        PlayerDTO players = JsonUtility.FromJson<PlayerDTO>(dto);

        if (!players.Success)
        {
            Debug.Log(players.Message);
            return;
        }

        roomID = players.RoomId;
        session.RoomID = roomID;

        // roomID = players.RoomId;
        // OnRoomJoin?.Invoke(players);
    }

    public void PlayerLeave(string dto)
    {
        PlayerDTO players = JsonUtility.FromJson<PlayerDTO>(dto);

        if (!players.Success)
        {
            Debug.Log(players.Message);
            return;
        }

        OnPlayerLeave?.Invoke(players);
    }

    public void ExitRooom()
    {
        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.ExitRoom);
            pck.WriteInt(roomID);
            pck.WriteLength();
            session.send(pck);
        }
    }

    public void RequestLoadScene()
    {
        using (Packet packet = new Packet())
        {
            packet.WriteInt((int)PacketTypeSend.RequestJoin);
            packet.WriteInt(roomID);
            packet.WriteLength();
            session.send(packet);
        }
    }
    public void LoadGame(bool load) => LoadScene?.Invoke(load);

    public void UpdateTxt(int counter) => OnCounterUpdate?.Invoke(counter);
}