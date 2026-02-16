using System;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager NM { private set; get; }

    public PacketDispatcher _packetHandler { private set; get; }

    public ClientSession session = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Action<UnityEditor.PlayModeStateChange> StopPlying;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        var client = new Client("localhost", 7777);
        _ = client.StartServer();
        UnityEditor.EditorApplication.playModeStateChanged += StopPlying;
        _packetHandler = new PacketDispatcher();
        _packetHandler.Init();
        DontDestroyOnLoad(this);
    }

    void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(gameObject);
            return;
        }

        NM = this;
    }

    void Start()
    {
        if (session == null)
            return;

    }
}
