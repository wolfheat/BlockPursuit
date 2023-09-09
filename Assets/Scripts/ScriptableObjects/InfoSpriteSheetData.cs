using UnityEngine;

[CreateAssetMenu(fileName = "InfoSpriteSheetData", menuName = "New Information with SpriteSheet")]
public class InfoSpriteSheetData : ScriptableObject
{
    [TextArea(5, 6)]
    public string informationText;
    public Sprite[] sprites;
}