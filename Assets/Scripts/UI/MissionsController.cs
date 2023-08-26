using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissionsController : EscapableBasePanel
{
    [SerializeField] GameObject missionHolder;
    [SerializeField] Mission missionPrefab;
    [SerializeField] MissionData[] missionDatas;
    private List<MissionData> pooledMissionDatas = new List<MissionData>();
    private List<Mission> missions = new List<Mission>();
    private MissionSaveData[] missionSaveData;
    private float updateTimer = 0;
    private const float UpdateTime = 2f;

    private void OnEnable()
    {
        Mission.OnMissionComplete += GiveMissionReward;
        SavingUtility.LoadingComplete += MissionDataLoaded;
        Inputs.Instance.Controls.UI.C.performed += ForgetAllMissions;
    }

    private void OnDisable()
    {
        Mission.OnMissionComplete -= GiveMissionReward;
        SavingUtility.LoadingComplete -= MissionDataLoaded;
    }

    private void WriteMissionDataToFile()
    {
        Debug.Log("Writing Mission data to file.");
        MissionsSaveData missionsDaveData = GenerateMissionSaveData();
        SavingUtility.playerGameData.MissionsSaveData = missionsDaveData;
        SavingUtility.Instance.SaveToFile();
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
        ClearMissions();
        CreateMissions();
    }
    
    private void ForgetAllMissions(InputAction.CallbackContext context)
    {
        Debug.Log(" *** Forgetting Mission Data ***");
        
        foreach (var data in missionDatas)
        {
            data.lastCompletion.timeString = DateInfo.DefaultString;
        }

        SavingUtility.playerGameData.MissionsSaveData = new MissionsSaveData();
        SavingUtility.Instance.SaveToFile();
    }

    private void SetMissionDataFromFile()
    {
        //Overwrite base data with data from stored values
        if (SavingUtility.playerGameData.MissionsSaveData == null) return; // If no data object exist skip writing it
        
        missionSaveData = SavingUtility.playerGameData.MissionsSaveData.Data;
        if (missionSaveData is null || missionSaveData.Length == 0) return;
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

        foreach (var data in missionDatas)
        {
            if (data.type == MissionType.Pool)
                pooledMissionDatas.Add(data);

            // Check for missions that are one time only and dont add them to the list
            if (data.type == MissionType.Single && data.lastCompletion.Completed)
            {
                // This mission is one time only and already completed do not add to list (keep the data though to keep it savede to file)
                Debug.Log("Mission Data says it is already completed do not make a mission instance. ");
                Debug.Log("Completed on "+data.lastCompletion.timeString);
                Debug.Log("Comnpared to "+DateInfo.DefaultString);
                continue;
            }

            Mission newMission = Instantiate(missionPrefab, missionHolder.transform);
            newMission.SetData(data);
            missions.Add(newMission);
        }
        UpdateMissions();
    }

    private void Update()
    {
        // This hinders mission to be updated and notifying player with flashing icon on level select screen that reward is available
        if (!Enabled()) return;

        updateTimer -= Time.deltaTime;
        // Only update every Second?
        if (missions.Count > 0 && updateTimer <= 0)
        {
            updateTimer = UpdateTime;
            UpdateMissions();
        }
    }

    private void UpdateMissions()
    {
        //TODO Seems like dateTime is not set correctly for the hourly mission check these values
        //Debug.Log("Checking to set missions active!");
        foreach(var mission in missions)
        {
            //Debug.Log("Checking to set mission active: "+mission.Name+" = "+ mission.GetMissionData().TimerUnlocked + " hours since last update: "+ mission.GetMissionData().TimePassed);
            mission.gameObject.SetActive(mission.GetMissionData().TimerUnlocked);
        }
    }
}

