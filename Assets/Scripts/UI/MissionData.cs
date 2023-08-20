using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "New Mission")]
public class MissionData : ScriptableObject
{
    public Sprite missionLogoSprite;
    public string missionName;
    public string missionDescription;
    public MissionRewardData missionRewardData;
}