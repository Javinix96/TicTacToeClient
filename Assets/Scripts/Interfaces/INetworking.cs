using System;
using System.Threading.Tasks;
using UnityEngine;

public interface INetworking
{
    Task Connect(string url, int port, ISession session, PacketRouter router, PopUpController popUpController);
    Task Listen();
    void Send(Packet packet);
    void Disconnect();
}
