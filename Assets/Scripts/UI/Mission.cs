using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI missionName;
    [SerializeField] Image missionLogo;
    [SerializeField] TextMeshProUGUI missionDescription;

    [SerializeField] GameObject CompleteObject;
    [SerializeField] GameObject IncompleteObject;

    [SerializeField] MissionReward[] missionRewards;

    [SerializeField] MissionDefinition missionData;

    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] Slider progressSlider;

    private MissionSaveData missionSaveData;

    public static Action<Mission> OnMissionComplete;

    public string Name => missionName.text;

    public MissionRewardData GetMissionRewardData() => missionData.missionRewardData;
    public MissionDefinition GetMissionData() => missionData;
    public bool Completed => gameObject.activeSelf && (missionSaveData.amount >= missionData.completeAmount);
    public bool IsActive => gameObject.activeSelf && missionSaveData.active;
    public MissionSaveData GetMissionSaveData() => missionSaveData;

    public void CompleteMission() => OnMissionComplete?.Invoke(this);

    public void SetData(MissionDefinition data, MissionSaveData correspondingSave)
    {
        missionData = data;
        missionSaveData = correspondingSave;

        Debug.Log("Mission ["+ data.missionName +"] is active: "+ missionSaveData.active+" and progressComplete: " + (missionSaveData.amount >= missionData.completeAmount));
        Initiate();
    }

    public void Initiate()
    {
        gameObject.SetActive(missionSaveData.active);
        //Also set completed or not
        SetAsProgressOrComplete(missionSaveData.amount >= missionData.completeAmount); // Should make timed free missions completed directly when activated    
        SetVisualsInfo();
    }

    private void SetVisualsInfo()
    {
        missionName.text = missionData.missionName;
        missionDescription.text = missionData.missionDescription;

        missionLogo.sprite = missionData.missionLogoSprite;
        //Scale sprite to fit square
        Sprite sprite = missionData.missionLogoSprite;
        
        foreach (var missionReward in missionRewards)
            missionReward.SetData(missionData.missionRewardData);

        // Define Slider start and max values
        progressSlider.maxValue = missionData.completeAmount;
        progressSlider.value = missionSaveData.amount;

        // Update Slider and text
        UpdateProgressVisuals();
    }

    public bool UpdateAddProgress(int amount)
    {
        return UpdateSetProgress(missionSaveData.amount + amount);
    }

    public bool UpdateSetProgress(int amount)
    {
        missionSaveData.amount = amount;

        if (missionSaveData.amount >= missionData.completeAmount)
        {
            SetAsProgressOrComplete();
            Debug.Log("Mission Returns as Completed");
            return true;
        }
        UpdateProgressVisuals();
        return false;
    }

    private void SetAsProgressOrComplete(bool set = true)
    {
        IncompleteObject.SetActive(!set);
        CompleteObject.SetActive(set);
    }

    private void UpdateProgressVisuals()
    {
        //Debug.Log("Updating progress visuals to amount = "+missionSaveData.amount);
        progressSlider.value = missionSaveData.amount;
        progressText.text = progressSlider.value + " / " + progressSlider.maxValue;
    }

    public void SetActive(bool activate = true)
    {
        missionSaveData.active = activate;
        UpdateProgressVisuals();
        gameObject.SetActive(activate);
    }

    public void CheckForTimedReactivation()
    {
        // If already active or time has not yet passed to unlock it
        if (missionSaveData.active || !missionSaveData.LockTimeHasPassed(missionData.type)) return;

        // Reactivate Mission
        missionSaveData.active = true;
        Initiate();
    }

}
