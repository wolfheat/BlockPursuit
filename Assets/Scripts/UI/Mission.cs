using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    [SerializeField] GameObject missionObject;
    [SerializeField] TextMeshProUGUI missionName;
    [SerializeField] Image missionLogo;
    [SerializeField] TextMeshProUGUI missionDescription;
    [SerializeField] MissionReward missionReward;
    [SerializeField] MissionDefinition missionData;
    private MissionSaveData missionSaveData;

    public static Action<Mission> OnMissionComplete;

    public string Name => missionName.text;

    public MissionRewardData GetMissionRewardData() => missionReward.MissionRewardData;
    public MissionDefinition GetMissionData() => missionData;


    public void Tick()
    {
        if (missionSaveData.active) return;

        // Handle unlocking of timed mission
        bool doUnlock = missionSaveData.LockTimeHasPassed(missionData.type);
        if (doUnlock)
        {
            missionObject.SetActive(true);
        }

    }

    public void CompleteMission() => OnMissionComplete?.Invoke(this);

    public void SetData(MissionDefinition data, MissionSaveData correspondingSave)
    {
        missionData = data;
        missionSaveData = correspondingSave;
        missionObject.SetActive(missionSaveData.active);
        SetText();
    }

    private void SetText()
    {
        missionName.text = missionData.missionName;
        missionDescription.text = missionData.missionDescription;
        missionReward.SetData(missionData.missionRewardData);
        missionLogo.sprite = missionData.missionLogoSprite;
    }

    public void SetActive(bool activate = true)
    {
        missionObject.SetActive(activate);
        missionSaveData.active = activate;
    }

    public void CheckForTimedDeactivation()
    {
        SetActive(missionSaveData.LockTimeHasPassed(missionData.type));        
    }
}
