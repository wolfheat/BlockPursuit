using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissionsController : EscapableBasePanel
{
    [SerializeField] GameObject missionHolder;
    [SerializeField] Mission missionPrefab;
    [SerializeField] MissionDefinition[] missionDefinitions;

    //private List<MissionDefinition> pooledMissionDatas = new List<MissionDefinition>();
    private List<Mission> missions = new List<Mission>();
    private List<Mission> timedMissions = new List<Mission>();
    private List<Mission> pooledMissions = new List<Mission>();
    private Dictionary<int, MissionSaveData> missionSaveDatas = new Dictionary<int, MissionSaveData>();
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
        SavingUtility.playerGameData.MissionsSaveData.Data = missionSaveDatas;
        SavingUtility.Instance.SavePlayerDataToFile();
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

        // Set new last completiontime
        missionSaveDatas[mission.GetMissionData().ID].lastCompletion = DateTime.UtcNow;
        missionSaveDatas[mission.GetMissionData().ID].everCompleted = true;

        mission.SetActive(false);

        WriteMissionDataToFile();

        // Handle Mission Deactivation?
        // just disable should work for all Timed Missions and Single Time missions
        // How about Pooled missions

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
    
        SavingUtility.playerGameData.MissionsSaveData = new MissionsSaveData();
        SavingUtility.Instance.SavePlayerDataToFile();
    }

    private void SetMissionDataFromFile()
    {
        // Overwrite base data with data from stored values if they exists
        if (SavingUtility.playerGameData.MissionsSaveData != null)
            missionSaveDatas = SavingUtility.playerGameData.MissionsSaveData.Data;

        // Generate the data that is missing in the file
        GenerateMissingMissionData();
    }

    private void GenerateMissingMissionData()
    {
        Debug.Log("Generating missing data: MIssion Definitions: "+ missionDefinitions.Length + " Save Data: "+missionSaveDatas.Count);
        foreach (var missionDefinition in missionDefinitions)
        {
            // For each defined mission in game
            // Create a corresponding save data entrance in the dictionary
            if (!missionSaveDatas.ContainsKey(missionDefinition.ID))
            {
                Debug.Log("Save does not contain key "+ missionDefinition.ID+" Adding it to save");
                missionSaveDatas.Add(missionDefinition.ID, new MissionSaveData());
                // If Pool Definition start inactive
                if (missionDefinition.type == MissionType.Pool) missionSaveDatas[missionDefinition.ID].active = false;
                continue;
            }
                Debug.Log("Save contain key "+ missionDefinition.ID+" Value is (time) "+ missionSaveDatas[missionDefinition.ID].lastCompletion.ToString());
        }
    }

    private void CreateMissions()
    {
        Debug.Log("Creating missions");

        foreach (var missionDefinition in missionDefinitions)
        {
            if (!missionSaveDatas.ContainsKey(missionDefinition.ID)) {
                Debug.LogError("Trying to create a mission but save data file for it is missing!");
                continue;
            }

            MissionSaveData correspondingSave = missionSaveDatas[missionDefinition.ID];

            // Check for missions that are one time only and dont add them to the list
            if (missionDefinition.type == MissionType.Single && correspondingSave.everCompleted)
            {
                // This mission is one time only and already completed do not add to list (keep the data though to keep it savede to file)
                Debug.Log("Mission Data says it is already completed do not make a mission instance. ");
                continue;
            }

            // Generate The mission
            Mission newMission = Instantiate(missionPrefab, missionHolder.transform);
            newMission.SetData(missionDefinition, correspondingSave);
            missions.Add(newMission);

            // Add to correct List
            switch (missionDefinition.type)
            {
                case MissionType.Single:
                case MissionType.Hourly:
                case MissionType.Daily:
                case MissionType.Weekly:
                    timedMissions.Add(newMission);
                    newMission.CheckForTimedDeactivation();
                    break;
                case MissionType.Pool:
                    pooledMissions.Add(newMission);
                    break;
            }
        }
        UpdateTimedMissions();
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
            UpdateTimedMissions();
        }
    }

    private void UpdateTimedMissions()
    {
        //TODO Seems like dateTime is not set correctly for the hourly mission check these values
        //Debug.Log("Checking to set missions active!");
        foreach(var mission in timedMissions)
        {
            mission.Tick();
        
        }
    }
}

