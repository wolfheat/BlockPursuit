using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionsController : EscapableBasePanel
{
    [SerializeField] GameObject missionHolder;
    [SerializeField] Mission missionPrefab;
    [SerializeField] MissionData[] missionDatas;
    private List<Mission> missions = new List<Mission>();
    private MissionSaveData[] missionSaveData;

    private void OnEnable()
    {
        Mission.OnMissionComplete += GiveMissionReward;
        SavingUtility.LoadingComplete += MissionDataLoaded;
    }

    private void OnDisable()
    {
        Mission.OnMissionComplete -= GiveMissionReward;
        SavingUtility.LoadingComplete -= MissionDataLoaded;
    }

    private void WriteMissionDataToFile()
    {
        MissionsSaveData missionsDaveData = GenerateMissionSaveData();
        SavingUtility.playerGameData.MissionsSaveData = missionsDaveData;

    }

    private MissionsSaveData GenerateMissionSaveData()
    {
        // Create MissionsSaveData from ingame data
        MissionsSaveData missionsDaveData = new MissionsSaveData();
        missionsDaveData.Data = new MissionSaveData[missionDatas.Length];

        for (int i = 0; i < missionDatas.Length; i++)
            missionsDaveData.Data[i] = new MissionSaveData() { ID = missionDatas[i].ID, amount = missionDatas[i].amount, latest = missionDatas[i].lastCompletion.timeString };
        return missionsDaveData;
    }

    public override void RequestESC()
    {   
        if (!Enabled()) return;
        Debug.Log("Close Missions ESC");
        CloseMenu();
    }
    public void CloseMenu()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideMissions);
    }

    private void UpdateMission(Mission mission)
    {
        // Did something that evolved the mission progress 
        // Update it
    }

    private void GiveMissionReward(Mission mission)
    {
        Debug.Log("Get mission reward for: "+mission.Name);
        /* // If want to remove single item and not update entire stack
        if(mission.GetMissionData().type == MissionType.Single)
            missions.Remove(mission);
        else
            mission.gameObject.SetActive(false);
        */

        // Mission reward data is not an instance?
        MissionRewardData missionRewardData = mission.GetMissionRewardData();
        Debug.Log("MissionrewardData: "+missionRewardData);
        SavingUtility.playerGameData.HandleMissionReward(missionRewardData);

        int index = missions.IndexOf(mission);
        missionDatas[index].lastCompletion.SetDateTime(DateTime.UtcNow);

        WriteMissionDataToFile();
        SetMissionDataFromFile();
        ClearMissions();
        CreateMissions();
    }

    private void ClearMissions()
    {
        foreach (var mission in missions)
        {
            Destroy(mission.gameObject);
        }
        missions.Clear();
    }

    // INITIALIZING MISSIONS
    private void MissionDataLoaded()
    {
        SetMissionDataFromFile();
        CreateMissions();
    }

    private void SetMissionDataFromFile()
    {
        //Overwrite base data with data from stored values
        if (SavingUtility.playerGameData.MissionsSaveData == null) return; // If no data object exist skip writing it
        
        missionSaveData = SavingUtility.playerGameData.MissionsSaveData.Data;
        foreach (var missionSaveData in missionSaveData)
        {
            foreach (var missionData in missionDatas)
            {
                if (missionData.ID == missionSaveData.ID)
                {
                    missionData.lastCompletion.timeString = missionSaveData.latest;
                    missionData.amount = missionSaveData.amount;
                    break;
                }
            }
        }
    }
    private void CreateMissions()
    {
        Debug.Log("Creating missions");
        DateTime now = DateTime.UtcNow;
        foreach (var data in missionDatas)
        {
            Mission newMission = Instantiate(missionPrefab, missionHolder.transform);
            newMission.SetData(data);
            missions.Add(newMission);
        }
    }

    private void Update()
    {
        // This hinders mission to be updated and notifying player with flashing icon on level select screen that reward is available
        if (!Enabled()) return;

        // Only update every Second?
        if(missions.Count>0)
            UpdateMissions();
    }

    private void UpdateMissions()
    {
        //TODO Seems like dateTime is not set correctly for the hourly mission check these values
        foreach(var mission in missions)
            mission.gameObject.SetActive(mission.GetMissionData().TimerUnlocked);
    }
}

