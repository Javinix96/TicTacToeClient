using System;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private INetworking _netClient = null;
    private ISession _session = null;
    private PacketRouter _packetRouter = null;
#if UNITY_EDITOR
    public static Action<UnityEditor.PlayModeStateChange> StopPlaying;
#endif
    void OnEnable()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += StopPlaying;
#endif
    }

    void Awake()
    {
        _session = new ClientSession();
        RoomService roomService = new RoomService(_session);
        var packetHandlers = new IPacketHandler[]
        {
            new Welcome(),
            new RoomsHandler(roomService),
            new Message(roomService),
            new JoinRoom(roomService)
        };

        _packetRouter = new(packetHandlers);
        _netClient = new ClientTCP();

        _netClient.Connect("localhost", 7777, _session, _packetRouter);
        // _netClient.Connect("168.220.91.181", 7777, _session, _packetRouter);

        var ui = FindAnyObjectByType<MenuUI>();
        ui.Init(_session, roomService);

#if UNITY_EDITOR
        StopPlaying += Stop;
#endif
    }

#if UNITY_EDITOR
    private void Stop(UnityEditor.PlayModeStateChange change)
    {
        if (change == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            if (_session != null)
                _session.Close();
    }
#endif

    void OnApplicationQuit()
    {
        if (_session != null)
            _session.Close();
    }


}
