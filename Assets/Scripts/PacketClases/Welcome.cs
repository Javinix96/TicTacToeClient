using UnityEngine;

public class Welcome : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.Welcome;

    public void Handle(Packet payload, ISession session = null)
    {
        int playerId = payload.ReadInt();
        if (session != null)
        {
            session.Id = playerId;
            session.SetIdUI(playerId);
        }
        string message = payload.ReadString();
        Debug.Log(message);
    }
}