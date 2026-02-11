using System;
using UnityEngine;

public class TCPClientTic : MonoBehaviour
{
    public static Action<UnityEditor.PlayModeStateChange> StopPlying;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        var client = new Client("localhost", 7777);
        _ = client.Start();
        Debug.Log((byte)10);
        UnityEditor.EditorApplication.playModeStateChanged += StopPlying;
    }


}
