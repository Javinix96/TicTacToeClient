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

        VisualElement glow = _root.Q<VisualElement>("Title");

        float time = 0f;

        glow.schedule.Execute(() =>
        {
            time += Time.deltaTime;

            float scale = 1f + Mathf.Sin(time * 2f) * 0.1f;
            float opacity = 0.3f + Mathf.Sin(time * 2f) * 0.2f;

            glow.style.scale = new Scale(new Vector3(scale, scale, 1));
            glow.style.opacity = opacity;

        }).Every(16);
    }

    private async void Connect() => await _networkManager.ConnectToServer();
}