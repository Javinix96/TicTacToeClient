using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomListController
{
    private UIManager _uiManager;
    private VisualElement _root;
    private ScrollView _roomsScroll;
    private VisualTreeAsset _roomLayout;
    private TextField _searchField;
    private ISession _session;

    private List<RoomInfo> rooms;
    private int indexRoom;

    public RoomListController(VisualElement root, UIManager uiManager, ISession session, VisualTreeAsset roomLayout)
    {
        _root = root;
        _uiManager = uiManager;
        _session = session;
        _roomLayout = roomLayout;
        rooms = new();

        Init();
        _uiManager.OnRoomsRequest += updateRooms;
        _uiManager.OnSendPassword += SendPassword;
    }

    private void Init()
    {
        var backButton = _root.Q<VisualElement>("btnSalir");
        var createButton = _root.Q<Button>("btnCreate");
        var joinButton = _root.Q<Button>("btnJoin");
        var btnSearch = _root.Q<VisualElement>("btnSearch");
        _searchField = _root.Q<TextField>("RoomField");

        createButton.clicked += () => CreaterRoom();
        btnSearch.RegisterCallback<ClickEvent>(SearchRoom);
        backButton.RegisterCallback<ClickEvent>((evt) => ExitServer());
        joinButton.clicked += JoinRoom;
        _roomsScroll = _root.Q<ScrollView>("roomList");

        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.RequestRooms);
            pck.WriteLength();
            _session.send(pck);
        }
    }

    private void JoinRoom()
    {
        using (Packet packet = new Packet())
        {
            packet.WriteInt((int)PacketTypeSend.JoinRoomRequest);
            packet.WriteInt(rooms[indexRoom].RoomId);
            packet.WriteLength();
            _session.send(packet);
        }
    }

    private void SearchRoom(ClickEvent evt)
    {
        if (string.IsNullOrEmpty(_searchField.value))
        {
            _ = PopUpController.ShowPopup("Error", "Escribe el nombre del error");
            return;
        }

        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.SendSearchRoom);
            pck.WriteString(_searchField.value);
            pck.WriteLength();
            _session.send(pck);
        }
    }

    private void updateRooms(RoomInfoDTO dto)
    {
        rooms.Clear();
        _roomsScroll.Clear();

        rooms = dto.Rooms;
        for (int index = 0; index < dto.Rooms.Count; index++)
            _roomsScroll.Add(AddRoom(dto.Rooms[index].RoomName, dto.Rooms[index].RoomHost, dto.Rooms[index].PlayersCount, index));
    }

    private async void SendPassword(string dto)
    {
        PlayerDTO playerInfo = JsonUtility.FromJson<PlayerDTO>(dto);

        var (hasValue, value) = await PopUpController.GetTextFieldPopUP($"Ingrese la contraseña de la sala {playerInfo.RoomName}", InputType.Password);

        if (!hasValue)
        {
            _ = PopUpController.ShowPopup("Informacion", "Accion Cancelada");
            return;
        }

        if (string.IsNullOrEmpty(value))
        {
            _ = PopUpController.ShowPopup("Error", "Ingrese contraseña");
            return;
        }

        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.SendPassword);
            pck.WriteInt(playerInfo.RoomId);
            pck.WriteString(value);
            pck.WriteLength();
            _session.send(pck);
        }
    }

    private VisualElement AddRoom(string roomName, string host, int players, int index)
    {
        VisualElement clone = _roomLayout.Instantiate();

        var roomNameLabel = clone.Q<Label>("RoomName");
        var playersLabel = clone.Q<Label>("Players");
        var hostLabel = clone.Q<Label>("HostLabel");

        roomNameLabel.text = roomName;
        playersLabel.text = $"{players}/2";
        hostLabel.text = $"Host: {host}";

        clone.RegisterCallback<ClickEvent>(evt =>
        {
            indexRoom = index;
            Debug.Log(indexRoom);
        });
        return clone;
    }

    private void ExitServer()
    {
        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.SendExit);
            pck.WriteLength();
            _session.send(pck);
        }

    }

    private void CreaterRoom() => _uiManager.GoToCreateRoom();
}