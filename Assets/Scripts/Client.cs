using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Client
{
    private TcpClient client;
    private string address = "";
    private int port = 0000;
    public Client(string address, int port)
    {
        this.address = address;
        this.port = port;
        client = new TcpClient();
    }

    public async Task Start()
    {
        TCPClientTic.StopPlying += QuitGame;
        try
        {
            await client.ConnectAsync(address, port);
            var stream = client.GetStream();
            int bitesLength = 0;
            byte[] buffer = new byte[1024];
            List<byte> rr = new List<byte>();
            rr.AddRange(BitConverter.GetBytes(1));
            rr.AddRange(BitConverter.GetBytes(("Hola usuario").Length));
            rr.AddRange(Encoding.UTF8.GetBytes("Hola usuario"));
            buffer = rr.ToArray();
            await stream.WriteAsync(buffer);

            while ((bitesLength = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                if (bitesLength <= 0)
                    break;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            client.Dispose();
            client.Close();
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
