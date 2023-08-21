using UnityEngine;

[CreateAssetMenu(fileName = "ToonDefinition", menuName = "New Toon")]
public class ToonDefinition : ScriptableObject
{
    public string toonName;
    public Sprite sprite;
    public AvatarType type;
}