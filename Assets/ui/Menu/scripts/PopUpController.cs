using UnityEngine;
using UnityEngine.UIElements;

public class PopUpController : MonoBehaviour
{
    public static UIDocument uiDocument;

    public VisualTreeAsset popupTemplate;

    public static VisualTreeAsset template;



    private static VisualElement root;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        template = popupTemplate;
    }

    public static void ShowPopup(string title, string message)
    {
        try
        {
            var popupLayer = root.Q<VisualElement>("Container");
            var popup = template.Instantiate();

            popup.style.flexGrow = 1;

            var titleLabel = popup.Q<Label>("Title");
            var messageLabel = popup.Q<Label>("Message");
            var okButton = popup.Q<Button>("okButton");

            titleLabel.text = title;
            messageLabel.text = message;

            okButton.clicked += () =>
            {
                // popupLayer.style.display = DisplayStyle.None;
                popupLayer.Remove(popup);
            };

            popupLayer.Add(popup);
            popupLayer.BringToFront();
            popup.BringToFront();

            popupLayer.Add(popup);
            popupLayer.style.display = DisplayStyle.Flex;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al mostrar el popup: {e.Message}");
        }
    }
}
