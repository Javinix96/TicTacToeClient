using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;

    private RoomService _roomService;
    private GameService _gameService;
    private NetworkManager _netManager;

    private ISession _session;

    #region MENU
    private VisualElement _menuContainer;
    private Button _createButton;
    private Button _joinButton;
    #endregion

    #region RoomList
    private VisualElement _listContainer;
    private ScrollView _scrollView;
    private Button _createRoomButton;
    private Button _joinRoomButton;
    private Button _ExitOnline;
    #endregion

    #region CreateRoom
    private VisualElement _createContainer;
    private TextField _roomName;
    private Button _createRoom;
    private Button _exitCreateRoom;
    #endregion

    #region JoinRoom
    private VisualElement _joinContainer;
    private ScrollView _playerListScrollView;
    private Label _counterTxt;
    private Button _playButton;
    private Button _exitButton;
    #endregion

    private int _roomId = 0;
    public List<string> _rooms;

    public void Init(NetworkManager nm, GameService gs, ISession session, RoomService roomService)
    {
        _session = session;
        _roomService = roomService;
        _gameService = gs;
        _netManager = nm;

        _session.SetId += SetID;
        _roomService.OnRoomsUpdated += SetRoomList;
        _roomService.OnCreateRoom += OnJoinRoom;
        _roomService.OnRoomJoin += OnJoinRoom;
        _roomService.OnPlayerLeave += OnJoinRoom;
        _roomService.LoadScene += LoadNextEscene;
        _roomService.OnCounterUpdate += UpdateTxt;

        _netManager.OnClientAccepted += OnJoinServer;

        AddListeners();
    }

    private void SetID(int i) => _createButton.text = $"Jugador: {i}";


    void OnEnable()
    {
        _rooms = new();
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        GetContainers();
        GetButtons();
        Init();
    }

    private void AddListeners()
    {
        if (_roomService == null)
            return;

        // _joinButton.clicked += ConectServer;
        // _joinButton.clicked += () =>
        // {
        //     EnabledRoomContainer();
        //     _roomService.RequestRooms();
        // };

        _createRoomButton.clicked += OpenCreateRoom;
        _createRoom.clicked += CreateRoom;
        _joinRoomButton.clicked += RequestJoinRoom;
        _exitButton.clicked += ExitRoom;
        _playButton.clicked += RequestLoadScene;
        _ExitOnline.clicked += ExitOnline;
        _exitCreateRoom.clicked += BackToRoomList;
    }

    private void OnJoinServer(string message)
    {
        EnabledRoomContainer();
        _roomService.RequestRooms();
        PopUpController.ShowPopup("Conexion Exitosa", message);
    }

    private void ConectServer() => _gameService.Connect();

    private void GetButtons()
    {
        _createButton = _menuContainer.Query<Button>("CreateServer");
        _joinButton = _menuContainer.Query<Button>("RoomsButton");

        VisualElement contButtons = _listContainer.Query<VisualElement>("buttonsCont");
        _createRoomButton = contButtons.Query<Button>("CreateRoom");
        _joinRoomButton = contButtons.Query<Button>("JoinRoom");
        _scrollView = _listContainer.Query<ScrollView>("ScrollRoomList");
        _ExitOnline = _listContainer.Query<Button>("ExitOnline");

        _roomName = _createContainer.Query<TextField>("NameRoom");
        _createRoom = _createContainer.Query<Button>("CreateButton");
        _exitCreateRoom = _createContainer.Query<Button>("BackButton");

        _playerListScrollView = _joinContainer.Query<ScrollView>("Players");
        _counterTxt = _joinContainer.Query<Label>("Counter");
        _playButton = _joinContainer.Query<Button>("PlayButton");
        _exitButton = _joinContainer.Query<Button>("ExitRoom");

        //container botones de rooms
        var buttonContainer = (VisualElement)_listContainer.Query<VisualElement>();
        _createRoomButton = buttonContainer.Query<Button>("CreateRoom");

    }

    private void Init()
    {
        _menuContainer.style.display = DisplayStyle.Flex;
        _listContainer.style.display = DisplayStyle.None;
        _createContainer.style.display = DisplayStyle.None;
        _joinContainer.style.display = DisplayStyle.None;
    }

    private void GetContainers()
    {
        if (_document == null)
            return;

        _menuContainer = _root.Query<VisualElement>("MenuContainer");

        if (_menuContainer == null)
        {
            Debug.LogError("No se encontro el contenedor del menu");
            return;
        }

        _listContainer = _root.Query<VisualElement>("RoomList");

        if (_listContainer == null)
        {
            Debug.LogError("No se encontro el contenedor de los rooms");
            return;
        }

        _createContainer = _root.Query<VisualElement>("CreateRoomCont");

        if (_createContainer == null)
        {
            Debug.LogError("No se encontro el contenedor de los crear rooms");
            return;
        }

        _joinContainer = _root.Query<VisualElement>("Players");

        if (_joinContainer == null)
        {
            Debug.LogError("No se encontro el contenedor de los players");
            return;
        }
    }

    private void DisabledRoomContainer()
    {
        _listContainer.style.display = DisplayStyle.None;
    }
    private void EnabledRoomContainer()
    {
        _listContainer.style.display = DisplayStyle.Flex;
        _menuContainer.style.display = DisplayStyle.None;
    }

    private void SetRoomList(RoomInfoDTO dto)
    {
        _scrollView = _listContainer.Query<ScrollView>("ScrollRoomList");
        if (_scrollView == null)
        {
            Debug.LogError("No se encontro la lista de rooms");
            return;
        }

        _scrollView.Clear();

        foreach (var room in dto.Rooms)
            AddRoom(room);
    }
    void OpenCreateRoom()
    {
        _menuContainer.style.display = DisplayStyle.None;
        _listContainer.style.display = DisplayStyle.None;
        _createContainer.style.display = DisplayStyle.Flex;
        _joinContainer.style.display = DisplayStyle.None;
    }

    void CreateRoom()
    {
        string text = _roomName.text;
        if (string.IsNullOrEmpty(text))
        {
            Debug.Log("El nombre de la sala no puede estar vacio");
            return;
        }

        //sendRooms
        _roomService.CreateRoom(text);

    }
    private void AddRoom(RoomInfo room)
    {
        var container = new VisualElement();
        container.AddToClassList("itemCont");
        container.userData = room.RoomId;

        var roomName = new Label { name = "RoomLabel" };
        var playerNumber = new Label { name = "PlayerNumberLabel" };
        roomName.text = room.RoomName;
        roomName.pickingMode = PickingMode.Ignore;
        playerNumber.text = $"{room.PlayersCount}/2";
        roomName.AddToClassList("item");
        playerNumber.AddToClassList("item");
        playerNumber.pickingMode = PickingMode.Ignore;
        container.Add(roomName);
        container.Add(playerNumber);
        container.focusable = true;
        container.RegisterCallback<ClickEvent>(OnClickEvent);

        _scrollView.Add(container);
    }
    private void OnClickEvent(ClickEvent evt)
    {
        var selectedItem = evt.target as VisualElement;
        if (selectedItem == null)
            return;

        _roomId = (int)selectedItem.userData;
    }
    private void OnJoinRoom(PlayerDTO roomPlayers)
    {
        if (_playerListScrollView == null)
        {
            Debug.LogError("Nop hay ningun scroll");
            return;
        }

        PlayerListCont();

        _playerListScrollView.Clear();
        foreach (var player in roomPlayers.Players)
            CreateVisualPlayer(player);

    }
    private void PlayerListCont()
    {
        _menuContainer.style.display = DisplayStyle.None;
        _listContainer.style.display = DisplayStyle.None;
        _createContainer.style.display = DisplayStyle.None;
        _joinContainer.style.display = DisplayStyle.Flex;
    }
    private void CreateVisualPlayer(Player player)
    {
        var container = new VisualElement();
        container.AddToClassList("playerCont");

        var roomName = new Label { name = "RoomLabel" };
        var playerNumber = new Label { name = "PlayerNumberLabel" };
        roomName.text = player.Name;
        playerNumber.text = $"{player.ID}";
        roomName.AddToClassList("item");
        playerNumber.AddToClassList("item");
        container.Add(roomName);
        container.Add(playerNumber);
        container.focusable = true;
        container.RegisterCallback<ClickEvent>(OnClickEvent);

        _playerListScrollView.Add(container);
    }
    private void RequestJoinRoom()
    {
        _roomService.PlayerJoinRoomRequest(_roomId);
    }

    private void ExitRoom()
    {
        _roomService.ExitRooom();
        BackToRooms();
    }

    private void BackToRooms()
    {
        _menuContainer.style.display = DisplayStyle.None;
        _listContainer.style.display = DisplayStyle.Flex;
        _createContainer.style.display = DisplayStyle.None;
        _joinContainer.style.display = DisplayStyle.None;
    }

    private void BackToMenu()
    {
        _menuContainer.style.display = DisplayStyle.Flex;
        _listContainer.style.display = DisplayStyle.None;
        _createContainer.style.display = DisplayStyle.None;
        _joinContainer.style.display = DisplayStyle.None;
    }

    private void RequestLoadScene() => _roomService.RequestLoadScene();

    private async Task LoadNextEscene(bool canLoad)
    {
        if (!canLoad)
        {
            Debug.Log("No puedes unirte al juego todavia");
            return;
        }

        await SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    private void UpdateTxt(int counter) => _counterTxt.text = $"Joining: {counter.ToString()}";

    private void ExitOnline()
    {
        if (_session == null) return;
        _session.Close();
        BackToMenu();
    }

    private void BackToRoomList()
    {
        _menuContainer.style.display = DisplayStyle.None;
        _listContainer.style.display = DisplayStyle.Flex;
        _createContainer.style.display = DisplayStyle.None;
        _joinContainer.style.display = DisplayStyle.None;
    }
}
