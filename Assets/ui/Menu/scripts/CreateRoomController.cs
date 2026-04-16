using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateRoomController
{
    private ISession _session;
    private UIManager _uiManager;
    private VisualElement _root;
    private TextField _roomName;
    private Toggle _privateToggle;
    private TextField _password;
    private Slider _turnTimer;
    private Label _timeTxt;

    public CreateRoomController(VisualElement root, UIManager uiManager, ISession session)
    {
        _root = root;
        _uiManager = uiManager;
        _session = session;

        Init();
    }

    private void Init()
    {
        _roomName = _root.Q<TextField>("RoomName");
        _privateToggle = _root.Q<Toggle>("PrivateToggle");
        _password = _root.Q<TextField>("Password");
        _turnTimer = _root.Q<Slider>("TurnTimer");
        _timeTxt = _root.Q<Label>("timeTxt");

        _turnTimer.RegisterValueChangedCallback(UpdateText);



        var cancelButton = _root.Q<Button>("Cancel");
        var createButton = _root.Q<Button>("CreateRoom");

        cancelButton.clicked += () => OnCancel();
        createButton.clicked += () => CreateButton();
    }

    private void UpdateText(ChangeEvent<float> evt) => _timeTxt.text = $"{Mathf.Round(_turnTimer.value)} Segundos";

    private void OnCancel() => _uiManager.Pop();

    private void CreateButton()
    {
        string name = _roomName.value;
        bool isPrivate = _privateToggle.value;
        string password = _password.value;
        float time = _turnTimer.value;

        if (string.IsNullOrEmpty(name))
        {
            PopUpController.ShowPopup("Error", "Ingrese nombre de la sala");
            return;
        }

        if (name.Length > 12)
        {
            PopUpController.ShowPopup("Error", "Nombre demasiado largo");
            return;
        }

        if (!string.IsNullOrEmpty(password))
        {
            if (password.Length < 4 || password.Length > 4)
            {
                PopUpController.ShowPopup("Error", "Contraseña demasiada larga");
                return;
            }
        }

        time = Mathf.Round(time);

        Packet pck = new Packet();
        pck.WriteInt((int)PacketTypeSend.createRoom);
        pck.WriteString(name);
        pck.WriteBool(isPrivate);
        pck.WriteString(password);
        pck.WriteInt((int)time);
        pck.WriteLength();

        _session.send(pck);
    }

}
