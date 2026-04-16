using UnityEngine;

public class StartGame : MonoBehaviour
{
    private ISession _session;
    private GameService _gameService;
    private CatUI _ui;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(ISession ss, GameService gm)
    {
        _session = ss;
        _gameService = gm;
    }

    async void Start()
    {
        _ui = FindFirstObjectByType<CatUI>();
        await _ui.Initilize(_session, _gameService);

        if (_session == null)
            return;

        Packet pck = PacketFactory.SendInt(PacketTypeSend.PlayerReady, _session.GetRoomID());
        _session.send(pck);
    }
}
