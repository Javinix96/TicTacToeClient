using UnityEngine.UIElements;

public class RoomListController
{
    private UIManager _uiManager;
    private VisualElement _root;

    private ISession _session;

    public RoomListController(VisualElement root, UIManager uiManager, ISession session)
    {
        _root = root;
        _uiManager = uiManager;
        _session = session;

        // root.style.flexGrow = 1;

        Init();
    }

    private void Init()
    {
        var backButton = _root.Q<Button>("btnSalir");
        backButton.clicked += ExitServer;

        var createButton = _root.Q<Button>("btnCreate");
        createButton.clicked += () => CreaterRoom();

        var joinButton = _root.Q<Button>("btnJoin");
        // joinButton.clicked += () => JoinRoom();

    }

    private void ExitServer()
    {
        Packet pck = new Packet();
        pck.WriteInt((int)PacketTypeSend.SendExit);
        pck.WriteLength();
        _session.send(pck);

    }

    private void CreaterRoom() => _uiManager.GoToCreateRoom();
}