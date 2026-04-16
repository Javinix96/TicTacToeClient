using System;
using System.Net.Sockets;
using System.Threading.Tasks;


public class ClientSession : ISession
{
    TcpClient _client = null;
    private Packet pck;
    private PacketRouter _router;
    private NetworkStream _stream = null;
    private int _id;
    private byte[] _buffer = null;
    private int _bytesRead = 0;
    private int _bytesTotalRead = 0;

    public NetworkStream Stream { get => _stream; }
    public int Id { get => _id; set => _id = value; }
    public byte[] Buffer { get => _buffer; set => _buffer = value; }
    public int BytesRead { get => _bytesRead; set => _bytesRead = value; }
    public int BytesTotalRead { get => _bytesTotalRead; set => _bytesTotalRead = value; }

    public int RoomID { get; set; }
    public string Who { get; set; }
    public event Action<int> SetId;
    public event Action<RoomInfoDTO> OnRoomsReceived;

    public void Initilize(TcpClient client, PacketRouter router)
    {
        _client = client;
        _stream = client.GetStream();
        _buffer = new byte[1024];
        pck = new Packet();
        _router = router;
        // _stream.WriteAsync(BitConverter.GetBytes(2233223));
    }


    public void ProccessData()
    {
        _bytesTotalRead += _bytesRead;
        byte[] data = new byte[_bytesRead];

        Array.Copy(_buffer, data, _bytesRead);
        pck.WriteBytes(data);

        if (pck.GetBytesArray().Length < 4)
            return;

        int lengthData = pck.ReadInt(false);

        if (lengthData <= 0)
            return;

        // if (lengthData != (pck.UnreadLength() - 4))
        //     return;

        lengthData = pck.ReadInt();


        while (lengthData > 0 && lengthData <= pck.UnreadLength())
        {
            var data2 = pck.ReadBytes(lengthData);
            using (Packet pck = new Packet(data2))
            {
                //Recibimos los paquetes y hacemos la logica
                int id = pck.ReadInt();
                Packet pckTemp = pck.Copy();
                _router.Route(id, pckTemp, this);
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

            ClearBuffer();
            break;
        }
    }

    private void ClearBuffer()
    {
        pck = new Packet();
        _buffer = new byte[1024];
        _bytesTotalRead = 0;
        _bytesRead = 0;
        // pck.Reset();
    }

    public void Close()
    {
        if (_client == null)
            return;
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

    void SetIdUI(int i)
    {
        SetId?.Invoke(i);
    }

    void ISession.SetIdUI(int i)
    {
        SetIdUI(i);
    }

    public void send(Packet packet)
    {
        if (_stream == null)
            return;

        _stream.Write(packet.GetBytesArray(), 0, packet.GetBytesArray().Length);
    }

    public int GetRoomID() => RoomID;
}
