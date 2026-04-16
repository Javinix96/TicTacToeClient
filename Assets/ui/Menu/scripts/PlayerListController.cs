using UnityEngine;
using UnityEngine.UIElements;

public class PlayerListController
{
    public UIManager _uiManager;
    private VisualElement _root;
    private ScrollView _players;
    private int times = 1;

    private VisualTreeAsset _item;

    public PlayerListController(VisualElement root, UIManager uiManager, int times, VisualTreeAsset item, PlayerDTO dto)
    {
        _uiManager = uiManager;
        _root = root;
        _item = item;
        this.times = times;

        Init();


        for (int count = 0; count < dto.Players.Count; count++)
        {
            var it = _item.CloneTree();
            _players.Add(it);
        }

    }

    private void Init()
    {
        _players = _root.Q<ScrollView>("Players");

        var exit = _root.Q<Button>("Exit");
        var join = _root.Q<Button>("Join");
        var ready = _root.Q<Button>("Ready");

        exit.clicked += Exit;
        join.clicked += Join;
        ready.clicked += Ready;


        Debug.Log("PlayerListController initialized");
    }

    private void Exit()
    {
        for (int i = 0; i < times; i++)
            _uiManager.Pop();
    }

    private void Join()
    {
        Debug.Log("Join");
    }

    private void Ready()
    {
        Debug.Log("Ready Player");
    }
}