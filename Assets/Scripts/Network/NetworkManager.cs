using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public PopUpController popUpController;

    public event Action<string> OnClientAccepted;
    private INetworking _netClient = null;
    private ISession _session = null;
    private GameService _gameService;
    private PacketRouter _packetRouter = null;

    [SerializeField]
    private UIManager _uiManager;
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
        RoomService roomService = new RoomService(_session, _uiManager);
        _gameService = new GameService(_session, popUpController);
        _uiManager.AddNetworkManager(this);
        _uiManager.AddSession(_session);

        var packetHandlers = new IPacketHandler[]
        {
            new Welcome(),
            new RoomsHandler(roomService),
            new Message(roomService),
            new JoinRoom(_uiManager),
            new PlayersRoom(_uiManager),
            new LoadScene(roomService),
            new Counter(_uiManager),
            new Who(_gameService),
            new RequestedExitRoom(_uiManager),
            new Position(_gameService),
            new Board(_gameService),
            new Turn(_gameService),
            new Winner(_gameService),
            new ClientAccepted(_uiManager),
            new SearchedRoom(_uiManager),
            new RoomCreated(_uiManager),
            new RequiredPassword(_uiManager),
            new PlayerReady(_uiManager),
        };

        _packetRouter = new(packetHandlers);
        _netClient = new ClientTCP(_uiManager);

        // _gameService.ConnectToServer += ConnectToServer;

        // // _netClient.Connect("137.66.15.84", 7777, _session, _packetRouter);
        // _netClient.Connect("localhost", 7777, _session, _packetRouter);

        // var ui = FindAnyObjectByType<MenuUI>();
        // ui.Init(this, _gameService, _session, roomService);

        SceneManager.sceneLoaded += OnSceneLoaded;

#if UNITY_EDITOR
        StopPlaying += Stop;
#endif
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var startGame = FindFirstObjectByType<StartGame>();

        if (startGame != null)
            startGame.Init(_session, _gameService);

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

    public async Task ConnectToServer()
    {
        if (_netClient == null) return;

        await _netClient.Connect("10.246.228.3", 7777, _session, _packetRouter, popUpController);
    }

    public void Disconnect()
    {
        if (_session != null)
            _session.Close();
    }

    public void ClientAccepted(string message) => OnClientAccepted?.Invoke(message);
}
