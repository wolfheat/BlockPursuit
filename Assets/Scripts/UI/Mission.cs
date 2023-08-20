using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI missionName;
    [SerializeField] Image missionLogo;
    [SerializeField] TextMeshProUGUI missionDescription;
    [SerializeField] MissionReward missionReward;
    [SerializeField] MissionData missionData;

    internal void SetData(MissionData data)
    {
        missionData = data;
        missionName.text = missionData.missionName;
        missionDescription.text = missionData.missionDescription;
        missionReward.SetData(missionData.missionRewardData);
        missionLogo.sprite = missionData.missionLogoSprite;
    }
}
