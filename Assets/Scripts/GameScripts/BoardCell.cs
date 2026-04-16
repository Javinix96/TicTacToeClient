using UnityEngine;
using UnityEngine.EventSystems;

public class BoardCell : MonoBehaviour, IPointerDownHandler
{
    private ISession _client;
    private CatUI _board;
    private int index = 0;
    private bool canClick;

    public int Index { get => index; set { index = value; } }

    public bool CanClick { get => canClick; set { canClick = value; } }

    public void Init(ISession client, CatUI board)
    {
        _client = client;
        _board = board;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canClick)
            return;

        RectTransform rect = GetComponent<RectTransform>();

        if (rect == null) return;

        if (string.IsNullOrEmpty(_client.Who)) return;

        Packet pck = PacketFactory.SendPacketPos(PacketTypeSend.Position, _client.RoomID, _client.Who, index);

        _client.send(pck);
    }
}
