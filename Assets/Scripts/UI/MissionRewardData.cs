using UnityEngine;

[CreateAssetMenu(fileName = "MissionReward", menuName = "New Mission Reward")]
public class MissionRewardData : ScriptableObject
{
    public int amount;
    public RewardType rewardType;
    public Sprite sprite;
}
