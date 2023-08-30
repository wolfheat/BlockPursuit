using UnityEngine;

[CreateAssetMenu(fileName = "MissionReward", menuName = "New Mission Reward")]
public class MissionRewardData : ScriptableObject
{
    public int value;
    public RewardType rewardType;
    public Sprite sprite;
}
