using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

struct ScreenData
{
    public VisualElement element;
    public VisualTreeAsset asset;
}

public class UIManager : MonoBehaviour
{
    private VisualElement root;

    private Stack<ScreenData> screenStack = new Stack<ScreenData>();

    [Header("UI Elements")]
    [SerializeField] private VisualTreeAsset mainMenu;
    [SerializeField] private VisualTreeAsset roomList;
    [SerializeField] private VisualTreeAsset createRoom;
    [SerializeField] private VisualTreeAsset joinRoom;
    [SerializeField] private VisualTreeAsset playerItem;

    private NetworkManager _networkManager;
    private ISession _session;

    private float duration = 0.25f;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement.Q("Container");
    }

    void Start()
    {
        Push(mainMenu, (root2) =>
       {
           new MainMenuController(root2, _networkManager);
       });
    }

    public void AddSession(ISession session) => _session = session;
    public void AddNetworkManager(NetworkManager manager) => _networkManager = manager;
    public void Push(VisualTreeAsset asset, Action<VisualElement> onInit)
    {
        try
        {
            var newScreen = asset.CloneTree();
            newScreen.style.left = new Length(100, LengthUnit.Percent); // fuera de pantalla
            newScreen.style.position = UnityEngine.UIElements.Position.Absolute;
            newScreen.style.height = new Length(100, LengthUnit.Percent);
            newScreen.style.width = new Length(100, LengthUnit.Percent);
            newScreen.style.flexGrow = 0;
            root.Add(newScreen);

            onInit?.Invoke(newScreen);

            if (screenStack.Count > 0)
            {
                var current = screenStack.Peek().element;
                current.style.position = UnityEngine.UIElements.Position.Absolute;
                current.style.top = 0;
                current.style.left = 0;
                current.style.width = new Length(100, LengthUnit.Percent);
                current.style.height = new Length(100, LengthUnit.Percent);

                // 🔥 rompe flex
                current.style.flexGrow = 0;

                Slide(current, 0, -150, duration, null);
            }

            // Animar entrada nueva (desde derecha)
            Slide(newScreen, 150, 0, duration, null);

            screenStack.Push(new ScreenData
            {
                element = newScreen,
                asset = asset
            });
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in Push: {ex.Message}");
        }
    }

    public void Lobby(PlayerDTO dto)
    {
        GoToJoinRoom(dto, 2);

        Debug.Log(dto.Players[0]);
    }

    // 🔙 POP (regresar)
    public void Pop()
    {
        if (screenStack.Count <= 1)
            return;

        var current = screenStack.Pop();
        var previous = screenStack.Peek();

        // Animar salida actual (derecha)
        Slide(current.element, 0, 100, duration, () =>
        {
            root.Remove(current.element);
        });

        // Animar regreso de anterior
        Slide(previous.element, -100, 0, duration, null);
    }

    void Slide(VisualElement element, float fromX, float toX, float duration, Action onComplete)
    {
        float time = 0f;

        element.style.position = UnityEngine.UIElements.Position.Absolute; ;
        element.style.top = 0;
        element.style.left = 0;
        element.style.width = new Length(100, LengthUnit.Percent);
        element.style.height = new Length(100, LengthUnit.Percent);
        IVisualElementScheduledItem animation = null;

        animation = element.schedule.Execute(() =>
       {
           time += Time.deltaTime;

           float t = time / duration;
           float eased = Mathf.SmoothStep(0, 1, t);

           float value = Mathf.Lerp(fromX, toX, eased);
           element.style.translate = new Translate(
           new Length(value, LengthUnit.Pixel), 0, 0);

           if (t >= 1f)
           {
               element.style.translate = new Translate(
               new Length(toX, LengthUnit.Pixel), 0, 0);
               onComplete?.Invoke();
               animation.Pause();
           }
       }).Every(16);
    }

    public void GoToRoomList() => Push(roomList, (root) => new RoomListController(root, this, _session));

    public void GoToCreateRoom() => Push(createRoom, (root) => new CreateRoomController(root, this, _session));

    public void GoToJoinRoom(PlayerDTO dto, int times = 1) => Push(joinRoom, (root) => new PlayerListController(root, this, times, playerItem, dto));

    public void GoBack() => Pop();

}
