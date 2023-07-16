using UnityEngine;

public class UIResolutionFixer : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] int Space = 90;
    // Start is called before the first frame update
    void Start()
    {
        //Read screen size and set Rect
        int height = Screen.height;
        int width = Screen.width;

        rect.sizeDelta = new Vector2 (0, height-Space);
        //rect.position = new Vector3(width / 2, height -Space);  
    }
}
