using System.Collections.Generic;

public class PacketRouter
{
    private readonly Dictionary<int, IPacketHandler> _handlers;

    public PacketRouter(IEnumerable<IPacketHandler> handlers)
    {
        _handlers = new Dictionary<int, IPacketHandler>();

        foreach (var handler in handlers)
        {
            _handlers.Add(handler.Header, handler);
        }
    }

    public void Route(int header, Packet payload, ISession session = null)
    {
        if (_handlers.TryGetValue(header, out var handler))
            handler.Handle(payload, session);
        else
            UnityEngine.Debug.Log($"No handler for header {header}");
    }
}

