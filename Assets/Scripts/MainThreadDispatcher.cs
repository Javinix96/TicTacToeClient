

using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{

    public static ConcurrentQueue<Action> _queue = null;
    public static MainThreadDispatcher MD { private set; get; }


    public void Awake()
    {
        if (MD != null && MD != this)
        {
            Destroy(gameObject);
            return;
        }

        MD = this;
        _queue = new();
        DontDestroyOnLoad(this);
    }

    public void Enqueue(Action action)
    {
        if (action == null)
            return;

        _queue.Enqueue(action);
    }

    private void Update()
    {
        while (_queue.TryDequeue(out var action))
            action?.Invoke();
    }

}