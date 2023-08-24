using System;
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

    internal void SetData(MissionRewardData data)
    {
        missionRewardData = data;
        rewardText.text = missionRewardData.amount.ToString();
        image.sprite = missionRewardData.sprite;
    }
}
