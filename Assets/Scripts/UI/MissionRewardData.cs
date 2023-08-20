using UnityEngine;

[CreateAssetMenu(fileName = "MissionReward", menuName = "New Mission Reward")]
public class MissionRewardData : ScriptableObject
{
    public string rewardText;
    public RewardType rewardType;
    public Sprite sprite;
}
