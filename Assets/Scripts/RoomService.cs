using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomService
{
    private ISession session;

    public Action<RoomInfoDTO> OnRoomsUpdated;

    public Action<PlayerDTO> OnCreateRoom;

    public Action<PlayerDTO> OnRoomJoin;

    public RoomService(ISession session) => this.session = session;

    public void UpdateRooms(string roomInfo)
    {
        var dto = JsonUtility.FromJson<RoomInfoDTO>(roomInfo);
        OnRoomsUpdated?.Invoke(dto);
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
        OnCreateRoom?.Invoke(players);
    }

    public void PlayerJoinRoom(string dto)
    {
        PlayerDTO players = JsonUtility.FromJson<PlayerDTO>(dto);
        if (!players.Success)
        {
            Debug.Log(players.Message);
            return;
        }
        OnRoomJoin?.Invoke(players);
    }
}