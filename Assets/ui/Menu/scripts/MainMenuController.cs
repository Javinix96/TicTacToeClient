using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController
{
    private VisualElement _root;
    private NetworkManager _networkManager;

    public MainMenuController(VisualElement root, NetworkManager networkManager)
    {
        _root = root;
        _networkManager = networkManager;

        Init();
    }

    void Init()
    {
        var playButton = _root.Q<Button>("btnMulti");

        playButton.clicked += Connect;
    }

    private void Connect() => _networkManager.ConnectToServer();
}