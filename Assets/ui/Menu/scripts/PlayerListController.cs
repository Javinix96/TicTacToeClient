using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class PlayerListController
{
    public UIManager _uiManager;
    private VisualElement _root;
    private ScrollView _playersScroll;
    private int times = 1;
    private VisualTreeAsset _item;
    private Label _timer;
    private ISession _session;

    private List<Player> players;

    public PlayerListController(VisualElement root, UIManager uiManager, int times, VisualTreeAsset item, PlayerDTO dto, ISession session)
    {
        _uiManager = uiManager;
         _root = root;
         _item = item;
        _session = session;
        GameManager.Instance.Room = dto.Room;
        this.times = times;

        Init();
        AddPlayers(dto);
    }

    private void AddPlayers(PlayerDTO dto)
    {
        _playersScroll.Clear();
        players = dto.Players;
        for (int count = 0; count < dto.Players.Count; count++)
            _playersScroll.Add(AddPlayuerLayout(dto.Players[count].Name,dto.Players[count].Ready));
    }

    private void UpdatePlayers(string json)
    {
        PlayerDTO players = JsonUtility.FromJson<PlayerDTO>(json);

        if (!players.Success)
        {
            Debug.Log(players.Message);
            return;
        }

        GameManager.Instance.Room = players.Room;

        AddPlayers(players);
    }

    private VisualElement AddPlayuerLayout(string name, bool ready)
    {
        VisualElement clone = _item.CloneTree();

        var nameTxt = clone.Q<Label>("PlayerName");

        if (nameTxt == null)
            return null;

        nameTxt.text = name;

        var status = clone.Q<Label>("Status");

        if (!ready)
            status.text = "⌛Esperando";
        else
            status.text = "  ✅Listo  ";

        return clone;
    }

    private void Init()
    {
        _uiManager.OnJoinRoom += UpdatePlayers;
        _playersScroll = _root.Q<ScrollView>("Players");
        _timer = _root.Q<Label>("Timer");

        var exit = _root.Q<VisualElement>("Exit");
        var ready = _root.Q<Button>("Ready");

        exit.RegisterCallback<ClickEvent>((evt) =>
        {
            RequestExit();
        });
        ready.clicked += Ready;

        _uiManager.onPlayerReady += EditPlayerStatus;
        _uiManager.onStartGame += CountToStart;
    }

    private void CountToStart(int counter) => _timer.text = counter.ToString();

    private void EditPlayerStatus(int id)
    {
        var playerIndex = GetIndexByID(id);

        players[playerIndex].Ready = true;

        var visualElement = _playersScroll.Children().ElementAt(playerIndex);
        var label = visualElement.Q<Label>("Status");
        label.text = "    ✅Listo    ";
    }

    private int GetIndexByID(int id)
    {
        for (int index = 0; index < players.Count; index++)
            if (players[index].ID == id)
                return index;

        return 0;
    }

    private void RequestExit()
    {
        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.ExitRoom);
            pck.WriteInt(GameManager.Instance.Room.RoomId);
            pck.WriteInt(times);
            pck.WriteLength();
            _session.send(pck);
        }
    }

    private void Exit()
    {
        for (int i = 0; i < times; i++)
            _uiManager.Pop();
    }

    private void Ready()
    {
        using (Packet pck = new Packet())
        {
            pck.WriteInt((int)PacketTypeSend.PlayerReady);
            pck.WriteInt(GameManager.Instance.Room.RoomId);
            pck.WriteInt((int)GameManager.Instance.ID);
            pck.WriteLength();
            _session.send(pck);
        }
    }
}