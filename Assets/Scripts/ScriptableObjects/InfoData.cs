using UnityEngine;

[CreateAssetMenu(fileName = "InfoData", menuName = "New Information")]
public class InfoData : ScriptableObject
{
    [TextArea(5, 20)]
    public string informationText;
}
