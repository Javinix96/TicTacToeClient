using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;

    #region MENU
    private VisualElement _menuContainer;
    private Button _createButton;
    private Button _JoinButton;
    #endregion

    #region RoomList
    private VisualElement _listContainer;
    private ScrollView _scrollView;
    private Button _createRoomButton;
    private Button _joinRoomButton;
    #endregion

    #region CreateRoom
    private VisualElement _createContainer;
    private TextField _roomName;
    private Button _createRoom;
    #endregion

    #region JoinRoom
    private VisualElement _joinContainer;
    private ScrollView _playerListScrollView;
    private Button _playButton;
    #endregion

    public List<string> _rooms;

    void OnEnable()
    {
        _rooms = new();
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        GetContainers();
        GetButtons();
        Init();
    }

    private void GetButtons()
    {
        _createButton = _menuContainer.Query<Button>("CreateServer");
        _JoinButton = _menuContainer.Query<Button>("RoomsButton");

        VisualElement contButtons = _listContainer.Query<VisualElement>("buttonsCont");
        _createRoomButton = contButtons.Query<Button>("CreateRoom");
        _joinRoomButton = contButtons.Query<Button>("JoinRoom");
        _scrollView = _listContainer.Query<ScrollView>("RoomList");
    
        _roomName = _createContainer.Query<TextField>("NameRoom");
        _createRoom = _createContainer.Query<Button>("CreateButton");
    
        _playerListScrollView = _joinContainer.Query<ScrollView>("Players");
        _playButton = _joinContainer.Query<Button>("PlayButton");
    }

    private void Init()
    {
        _createContainer.style.display = DisplayStyle.None;
        _joinContainer.style.display = DisplayStyle.None;
        _listContainer.style.display = DisplayStyle.None;
        _menuContainer.style.display = DisplayStyle.Flex;
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
    private void GetList()
    {
        _scrollView = _listContainer.Query<ScrollView>("RoomList2");

        if (_scrollView == null)
        {
            Debug.LogError("No se encontro la lista de rooms");
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            var container = new VisualElement();
            container.AddToClassList("itemCont");

            var label = new Label { name = "label" };
            label.text = $"Lol {Random.Range(0, 1000)}";
            label.AddToClassList("item");
            container.Add(label);
            container.RegisterCallback<ClickEvent>(OnClickEvent);

            _scrollView.Add(container);
        }
    }
    private void OnClickEvent(ClickEvent evt)
    {
        var selectedItem = evt.target as VisualElement;
        if (selectedItem == null)
            return;

        var label = selectedItem.Q("label") as Label;

        if (label == null)
            return;

        Debug.Log(label.text);
    }
}
