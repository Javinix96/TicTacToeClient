using System;
using System.Net.Sockets;
using UnityEngine;

public interface ISession
{
    NetworkStream Stream { get; }
    int Id { get; set; }
    byte[] Buffer { get; set; }
    int BytesRead { get; set; }
    int BytesTotalRead { get; set; }

    int RoomID { get; set; }

    string Who { get; set; }

    event Action<int> SetId;

    event Action<RoomInfoDTO> OnRoomsReceived;

    void ProccessData();

    void Initilize(TcpClient client, PacketRouter router);

    void send(Packet packet);

    void Close();

    void SetIdUI(int i);

    int GetRoomID();
}
