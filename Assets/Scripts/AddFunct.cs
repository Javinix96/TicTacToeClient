using UnityEngine.UIElements;
using UnityEngine;

public static class AddFunc
{
    public static int Index(this VisualElement cell) => (int)cell.userData;
    
}
