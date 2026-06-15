using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private  VisualElement _root;

    [SerializeField] private VisualTreeAsset player;

    VisualElement clickedCell;
    VisualElement pp;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

       var list =  _root.Query<VisualElement>("cell").ToList();
       pp = player.CloneTree();

    int index = 0;
       foreach(var cell in list)
        {
            cell.userData = index;
            cell.RegisterCallback<ClickEvent>(CreatePlayer);
            index++;
        }
    }

    private void CreatePlayer(ClickEvent evt)
    {
        if (clickedCell != null)
        {
            clickedCell.Clear();
        }

        var cell = evt.target as VisualElement;
        pp.style.flexGrow = 1;
        pp.style.width = Length.Percent(98);
        pp.style.height = Length.Percent(95);
        cell.Add(pp);

        Debug.Log(cell.Index());

        clickedCell = cell;
        
    }

}