using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpController : MonoBehaviour
{
    public static UIDocument uiDocument;

    public VisualTreeAsset popupTemplate;
    public VisualTreeAsset popUpTemplateFieldTxt;

    public static VisualTreeAsset template;
    public static VisualTreeAsset popUpTemplateField;
    private static VisualElement root;


    private static bool answer;
    private static bool waiting = true;
    private static string fieldTxt;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        template = popupTemplate;
        popUpTemplateField = popUpTemplateFieldTxt;
    }


    public async static Task<(bool, string)> GetTextFieldPopUP(string message, InputType type = InputType.WriteName)
    {
        var tcs = new TaskCompletionSource<(bool, string)>();
        var popupLayer = root.Q<VisualElement>("Container");
        var popup = popUpTemplateField.Instantiate();

        foreach (var layout in popupLayer.Children().ToList())
        {
            var ff = layout.Q<VisualElement>("Overlay");

            if (ff != null)
                layout.RemoveFromHierarchy();
        }

        popup.style.flexGrow = 1;

        var messageLabel = popup.Q<Label>("Message");
        var textField = popup.Q<TextField>("NameField");
        var cancelButton = popup.Q<Button>("Cancel");
        var okButton = popup.Q<Button>("Ok");

        textField.isPasswordField = type == InputType.Password ? true : false;

        cancelButton.clicked += (() =>
        {
            answer = false;
            fieldTxt = string.Empty;
            tcs.TrySetResult((false, string.Empty));
            popupLayer.Remove(popup);
        });

        okButton.clicked += () =>
    {

        if (string.IsNullOrEmpty(textField.text))
        {
            Debug.Log("Ingrese nombre");
            return;
        }

        answer = true;
        tcs.TrySetResult((true, textField.text));
        popupLayer.Remove(popup);
    };

        messageLabel.text = message;

        popupLayer.Add(popup);
        popupLayer.BringToFront();
        popup.BringToFront();

        popupLayer.style.display = DisplayStyle.Flex;

        return await tcs.Task;
    }

    private static void ok()
    {
        if (string.IsNullOrEmpty(fieldTxt))
        {
            Debug.Log("Ingrese nombre");
            return;
        }
        waiting = false;
        answer = true;
    }


    public static async Task<bool> ShowPopup(string title, string message)
    {
        try
        {
            var tcs = new TaskCompletionSource<bool>();
            var popupLayer = root.Q<VisualElement>("Container");
            var popup = template.Instantiate();

            foreach (var layout in popupLayer.Children().ToList())
            {
                var ff = layout.Q<VisualElement>("Overlay");

                if (ff != null)
                    layout.RemoveFromHierarchy();
            }

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
                tcs.TrySetResult(true);
            };

            popupLayer.Add(popup);
            popupLayer.BringToFront();
            popup.BringToFront();

            popupLayer.style.display = DisplayStyle.Flex;


            return await tcs.Task;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al mostrar el popup: {e.Message}");

            return false;
        }
    }
}
