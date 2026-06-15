using UnityEngine;

public interface IPacketHandler
{
    int Header { get; }
    void Handle(Packet payload, ISession session = null);
}
