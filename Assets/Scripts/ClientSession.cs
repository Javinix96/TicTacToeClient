using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;


public class ClientSession
{
    TcpClient _client = null;
    private Packet pck;
    public NetworkStream _stream = null;
    public int _id = 0;
    public byte[] _buffer = null;
    public int bytesRead = 0;
    public int bytesTotalRead = 0;

    public ClientSession(TcpClient client)
    {
        _stream = client.GetStream();
        _buffer = new byte[1024];
        pck = new Packet();
    }


    public void AddBytes()
    {
        bytesTotalRead += bytesRead;
        byte[] data = new byte[bytesRead];

        Array.Copy(_buffer, data, bytesRead);
        pck.SetBytes(data);

        if (pck.GetBytesArray().Length < 4)
            return;

        int lengthData = pck.ReadInt();

        if (lengthData <= 0)
            pck.Dispose();


        while (lengthData > 0 && lengthData <= pck.UnreadLength())
        {
            var data2 = pck.ReadBytes(lengthData);
            using (Packet pck = new Packet(data2))
            {
                //Recibimos los paquetes y hacermos la logica
                int id = pck.ReadInt();

                if (NetworkManager.NM._packetHandler.packetHandlers.TryGetValue(id, out var welcome))
                {
                    Packet pckTemp = pck.Copy();

                    MainThreadDispatcher.MD.Enqueue(() =>
                    {
                        welcome(pckTemp);

                    });
                }

            }

            if (pck.UnreadLength() >= 4)
            {
                lengthData = pck.ReadInt();
                if (lengthData <= 0)
                {
                    ClearBuffer();
                    break;
                }
                continue;
            }

            pck.Dispose();
            ClearBuffer();
            break;
        }
    }

    private void ClearBuffer()
    {
        pck = new Packet();
        _buffer = new byte[1024];
        bytesTotalRead = 0;
        bytesRead = 0;
        pck.Dispose();
    }

    public void ProccessData()
    {
        if (_buffer == null)
            return;
        if (_buffer.Length <= 0)
            return;
        if (_stream == null)
            return;
        if (bytesRead <= 0)
            return;

        byte[] data = new byte[bytesRead];
        for (int i = 0; i < bytesRead; i++)
            data[i] = _buffer[i];

        using (Packet pck = new Packet(data))
        {
            Console.WriteLine("id: " + pck.ReadInt());
            Console.WriteLine("Lenght: " + pck.ReadInt(false));
            Console.WriteLine("string: " + pck.ReadString());
        }

        _buffer = new byte[1024];
    }

    public void Close()
    {
        _client.Dispose();
        _client.Close();
    }

    public void SendWelcome(string message)
    {
        using (Packet pck = new Packet())
        {
            pck.WriteInt(1);
            pck.WriteString(message);
            pck.WriteLength();

            _ = SendString(pck);
        }
    }

    private async Task SendString(Packet pck)
    {
        await _stream.WriteAsync(pck.GetBytesArray(), 0, pck.GetBytesArray().Length);

    }
}
