using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class ClientTCP : INetworking
{
    private TcpClient client = null;
    private ISession _session = null;

    private UIManager _uiManager;

    public ClientTCP(UIManager um) => _uiManager = um;


    public async Task Connect(string url, int port, ISession session, PacketRouter router, PopUpController popUpController)
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
            PopUpController.ShowPopup("Error de conexión", "Se ha perdido la conexión con el servidor.");
            Debug.Log(e);
            _uiManager.Pop();
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
            PopUpController.ShowPopup("Error de conexión", "Se ha perdido la conexión con el servidor.");
            Debug.Log(e.Message);
            _uiManager.Pop();
        }
        finally
        {
            Disconnect();
            PopUpController.ShowPopup("Exito", "Te has desconectado del servidor");
            _uiManager.Pop();
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
