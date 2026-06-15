using UnityEngine;

public class SearchedRoom : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedSearchedRoom;

    private UIManager _UIMnager;

    public SearchedRoom(UIManager uim) => _UIMnager = uim;

    public void Handle(Packet payload, ISession session = null)
    {
        string dto = payload.ReadString();
        _UIMnager.OnRoomsRequest?.Invoke(JsonUtility.FromJson<RoomInfoDTO>(dto));
    }
}