using UnityEngine;

[CreateAssetMenu(fileName = "InfoSpriteSheetData", menuName = "New Information with SpriteSheet")]
public class InfoSpriteSheetData : ScriptableObject
{
    public string headerText;
    [TextArea(5, 6)]
    public string informationText;
    public Sprite[] sprites;
}