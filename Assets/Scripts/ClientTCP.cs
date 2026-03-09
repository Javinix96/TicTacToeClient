using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class ClientTCP : INetworking
{
    private TcpClient client = null;
    private ISession _session = null;

    public async Task Connect(string url, int port, ISession session, PacketRouter router)
    {
        try
        {
            client = new TcpClient();
            await client.ConnectAsync(url, port);
            Debug.Log("Conectado al server");
            _session = session;
            _session.Initilize(client, router);
            _ = Listen();

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void Disconnect()
    {
        client.Dispose();
        client.Close();
    }

    public async Task Listen()
    {
        try
        {
            while ((_session.BytesRead = await _session.Stream.ReadAsync(_session.Buffer, 0, _session.Buffer.Length)) > 0)
            {
                if (_session.BytesRead <= 0)
                    break;

                _session.ProccessData();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    public void Send(Packet packet)
    {
    }

    //     public void QuitGame(UnityEditor.PlayModeStateChange change)
    //     {
    // #if UNITY_EDITOR
    //         if (change != UnityEditor.PlayModeStateChange.ExitingPlayMode)
    //             return;
    //         client.Dispose();
    //         client.Close();
    //         Debug.Log(change);
    // #else
    //         client.Dispose();
    //         client.Close();
    // #endif

    //     }

}
