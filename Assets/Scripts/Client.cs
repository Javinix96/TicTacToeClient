using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Client
{
    private TcpClient client = null;
    private string address = "";
    private int port = 0000;
    private NetworkManager net = NetworkManager.NM;
    public Client(string address, int port)
    {
        this.address = address;
        this.port = port;
        client = new TcpClient();
    }

    public async Task StartServer()
    {
        NetworkManager.StopPlying += QuitGame;
        try
        {
            await client.ConnectAsync(address, port);
            net.session = new ClientSession(client);

            while ((net.session.bytesRead = await net.session._stream.ReadAsync(net.session._buffer, 0, net.session._buffer.Length)) > 0)
            {
                if (net.session.bytesRead <= 0)
                    break;

                net.session.AddBytes();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            client.Dispose();
            client.Close();
        }
    }


    public void QuitGame(UnityEditor.PlayModeStateChange change)
    {
        if (change != UnityEditor.PlayModeStateChange.ExitingPlayMode)
            return;
        client.Dispose();
        client.Close();
        Debug.Log(change);

    }

}
