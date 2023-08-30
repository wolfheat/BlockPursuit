using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType { Gold, Tiles, Unlock};
public class MissionReward : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] Image image;
    [SerializeField] MissionReward missionReward;
    private MissionRewardData missionRewardData;
    public MissionRewardData MissionRewardData => missionRewardData;

    public void SetData(MissionRewardData data)
    {
        
        missionRewardData = data;
        rewardText.text = (missionRewardData.rewardType == RewardType.Unlock)?((char)(65+missionRewardData.value)).ToString():missionRewardData.value.ToString();
        image.sprite = missionRewardData.sprite;
    }
}
