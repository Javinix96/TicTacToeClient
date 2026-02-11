using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    private UIDocument _document;

    private VisualElement _menuContainer;
    private VisualElement _roomsContainer;
    private ScrollView _scrollView;

    public List<string> _rooms;

    void OnEnable()
    {
        _rooms = new();
        _document = GetComponent<UIDocument>();
        GetContainers();
    }

    private void GetContainers()
    {
        if (_document == null)
            return;

        _menuContainer = _document.rootVisualElement.Query<VisualElement>("MenuContainer");

        if (_menuContainer == null)
        {
            Debug.LogError("No se encontro el contenedor del menu");
            return;
        }

        _roomsContainer = _document.rootVisualElement.Query<VisualElement>("RoomList");

        if (_roomsContainer == null)
        {
            Debug.LogError("No se encontro el contenedor de los rooms");
            return;
        }

        // DisabledRoomContainer();
        GetList();

    }
    private void DisabledRoomContainer()
    {
        _roomsContainer.style.display = DisplayStyle.None;
    }

    private void GetList()
    {
        _scrollView = _roomsContainer.Query<ScrollView>("RoomList2");

        if (_scrollView == null)
        {
            Debug.LogError("No se encontro la lista de rooms");
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            var container = new VisualElement();
            container.AddToClassList("itemCont");

            var label = new Label { name = "label" };
            label.text = $"Lol {Random.Range(0, 1000)}";
            label.AddToClassList("item");
            container.Add(label);
            container.RegisterCallback<ClickEvent>(OnClickEvent);

            _scrollView.Add(container);
        }
    }

    private void OnClickEvent(ClickEvent evt)
    {
        var selectedItem = evt.target as VisualElement;
        if (selectedItem == null)
            return;

        var label = selectedItem.Q("label") as Label;

        if (label == null)
            return;

        Debug.Log(label.text);
    }
}
