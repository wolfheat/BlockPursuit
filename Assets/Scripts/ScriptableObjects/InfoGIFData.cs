using UnityEngine;

[CreateAssetMenu(fileName = "InfoGIFData", menuName = "New Information with GIF")]
public class InfoGIFData : ScriptableObject
{
    [TextArea(5, 20)]
    public string informationText;
    public Sprite gifSprite;
}
