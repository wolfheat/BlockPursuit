using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

public class MissionsController : EscapableBasePanel
{
    [SerializeField] private GameObject missionHolder;
    [SerializeField] Mission[] missionPrefabs;
    [SerializeField] MissionDefinition[] missionDefinitions;

    //private List<MissionDefinition> pooledMissionDatas = new List<MissionDefinition>();
    private List<Mission> missions = new List<Mission>();
    private List<Mission> timedMissions = new List<Mission>();
    private List<Mission> pooledMissions = new List<Mission>();
    private List<Mission> playTimeMissions = new List<Mission>();
    private List<Mission> watchAdMissions = new List<Mission>();
    private Dictionary<int, MissionSaveData> missionSaveDatas = new Dictionary<int, MissionSaveData>();
    private float updateTimer = 0;
    private const float UpdateTime = 2f;

    private void OnEnable()
    {
        //missionHolder = gameObject;
        Mission.OnMissionComplete += GiveMissionReward;
        SavingUtility.LoadingComplete += MissionDataLoaded;
        PlayerGameData.AdsWatchedAdded += HandleWatchedAd;
        PlayerGameData.MinuteWatched += HandleMinutePlayed;
        //Inputs.Instance.Controls.UI.C.performed += ForgetAllMissions;
    }

    private void OnDisable()
    {
        Mission.OnMissionComplete -= GiveMissionReward;
        SavingUtility.LoadingComplete -= MissionDataLoaded;
        PlayerGameData.AdsWatchedAdded -= HandleWatchedAd;
        PlayerGameData.MinuteWatched -= HandleMinutePlayed;
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

    public void HandleComletedLevel(LevelDefinition level)
    {
        //Debug.Log("Player completed a level:  Tier: "+level.LevelDiff+" ID: "+level.LevelIndex);
        //Debug.Log("Update Missions from this information");

        bool didUpdateData = false;
        bool completedMission = false;

        foreach (var mission in missions)
        {
            MissionSaveData saveData = mission.GetMissionSaveData();
            if (!saveData.active) continue;

            MissionDefinition missionDefinition = mission.GetMissionDefinition();

            if (missionDefinition.completeType == CompleteType.CompleteTier)
            {
                if (level.LevelDiff != missionDefinition.tier) continue;

                int completeLevelsOfTier = SavingUtility.playerGameData.PlayerLevelDataList.AmountCompletedOfTier(missionDefinition.tier);

                // Mission complete type is complete tier, correct tier
                if(mission.UpdateSetProgress(completeLevelsOfTier))
                    completedMission = true;

                didUpdateData = true;

            }
            else if(mission.GetMissionDefinition().completeType == CompleteType.CompleteAnyLevels)
            {
                if(completedMission & mission.UpdateAddProgress(1))
                    completedMission = true;
                didUpdateData = true;
            }
        }

        if (didUpdateData)
        {
            Debug.Log("SAVE INVOKE - ANY ACTIVE MISSION DATA UPDATED");
            MissionSaveData.MissionUpdate?.Invoke();
        }
        if (completedMission)
        {
            PlayerGameData.MissionCompleted?.Invoke(CompletedMissionsAmount());
        }
    }
    
    public void HandleWatchedAd()
    {
        bool didUpdateData = false;
        bool completedMission = false;

        foreach (var mission in missions)
        {
            if (!mission.IsActive) continue;
            
            MissionDefinition missionDefinition = mission.GetMissionDefinition();

            if (missionDefinition.completeType == CompleteType.WatchAds)
            {
                if(missionDefinition.type == MissionType.Single && mission.UpdateSetProgress(SavingUtility.playerGameData.AdsWatched))
                    completedMission = true;
                if (missionDefinition.type == MissionType.Pool && mission.UpdateAddProgress(1))
                    completedMission = true;
                didUpdateData = true;
            }
        }

        if (didUpdateData)
        {
            Debug.Log("SAVE INVOKE - ANY ACTIVE MISSION DATA UPDATED");
            MissionSaveData.MissionUpdate?.Invoke();
        }
        if (completedMission)
        {
            PlayerGameData.MissionCompleted?.Invoke(CompletedMissionsAmount());
        }
    }
    
    public void HandleMinutePlayed()
    {
        bool didUpdateData = false;
        bool completedMission = false;

        foreach (var mission in missions)
        {
            if (!mission.IsActive) continue;
            
            MissionDefinition missionDefinition = mission.GetMissionDefinition();

            if (missionDefinition.completeType == CompleteType.PlayTime)
            {
                if(missionDefinition.type == MissionType.Single && mission.UpdateSetProgress(SavingUtility.playerGameData.PlayTime))
                    completedMission = true;
                if (missionDefinition.type == MissionType.Pool && mission.UpdateAddProgress(1))
                    completedMission = true;
                didUpdateData = true;
            }
        }

        if (didUpdateData)
        {
            Debug.Log("SAVE INVOKE - ANY ACTIVE MISSION DATA UPDATED");
            MissionSaveData.MissionUpdate?.Invoke();
        }
        if (completedMission)
        {
            PlayerGameData.MissionCompleted?.Invoke(CompletedMissionsAmount());
        }
    }

    private int CompletedMissionsAmount()
    {
        int amt = 0;
        foreach (var mission in missions)
            if (mission.Completed) amt++;
        return amt;
    }

    private void GiveMissionReward(Mission mission)
    {
        Debug.Log("Get mission reward for: "+mission.Name);
        
        // Give the reward
        SavingUtility.playerGameData.HandleMissionReward(mission.GetMissionRewardData());

        // Update completionTime (invokes save)
        missionSaveDatas[mission.GetMissionDefinition().ID].SetMissionCompletionInfo();


        mission.SetActive(false);

        MissionDefinition missionDefinition = mission.GetMissionDefinition();

        if (missionDefinition.type == MissionType.Pool)
            UnlockRandomPooledMission();
        else if (missionDefinition.type == MissionType.Single)
        {

            if(missionDefinition.completeType == CompleteType.PlayTime)
            {
                playTimeMissions.Remove(mission);
                ShowBestMissionForSpecificMissionList(playTimeMissions);
            }

            if (missionDefinition.completeType == CompleteType.WatchAds)
            {
                watchAdMissions.Remove(mission);
                ShowBestMissionForSpecificMissionList(watchAdMissions); 
            }

            missions.Remove(mission);
            Destroy(mission.gameObject);
        }

        MissionSaveData.MissionUpdate?.Invoke();

        PlayerGameData.MissionCompleted?.Invoke(CompletedMissionsAmount());
    }

    private void ClearMissions()
    {
        foreach (var mission in missions)
        {
            Destroy(mission.gameObject);
        }
        missions.Clear();
        timedMissions.Clear();
        pooledMissions.Clear();
        playTimeMissions.Clear();
        watchAdMissions.Clear();
    }

    // INITIALIZING MISSIONS
    private void MissionDataLoaded()
    {
        SetMissionDataFromFile();
        UpdateMissionsWithSpecialConditions();

        ClearMissions();
        CreateMissions();
        PlayerGameData.MissionCompleted?.Invoke(CompletedMissionsAmount());
    }

    private void UpdateMissionsWithSpecialConditions()
    {
        foreach(var missionDefinition in missionDefinitions)
        {
            // For adsWatched and Timeplayed missions update process value
            if (missionDefinition.type == MissionType.Single)
            {
                if(missionDefinition.completeType == CompleteType.CompleteTier)
                {
                    int totalAmountLevelsForTier = Levels.LevelDefinitions[missionDefinition.tier].Count;
                    missionDefinition.completeAmount = totalAmountLevelsForTier;
                }
                if (missionDefinition.completeType == CompleteType.PlayTime)
                    missionSaveDatas[missionDefinition.ID].amount = SavingUtility.playerGameData.PlayTime;
                if (missionDefinition.completeType == CompleteType.WatchAds)
                    missionSaveDatas[missionDefinition.ID].amount = SavingUtility.playerGameData.AdsWatched;
            }
        }
    }

    /*
    private void ForgetAllMissions(InputAction.CallbackContext context)
    {
        Debug.Log(" *** Forgetting Mission Data ***");
    
        SavingUtility.playerGameData.MissionsSaveData = new MissionsSaveData();
        Debug.Log("SAVE CALLED - FORGETTING ALL MISSIONS");
        SavingUtility.Instance.SavePlayerDataToFile();
    }*/

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
        Debug.Log("Generating missing data: Amount of Missions Defined in Game: "+ missionDefinitions.Length + " Amount stored in Save Data: "+missionSaveDatas.Count);
        foreach (var missionDefinition in missionDefinitions)
        {
            // For each defined mission in game
            // Create a corresponding save data entrance in the dictionary
            if (!missionSaveDatas.ContainsKey(missionDefinition.ID))
            {
                Debug.Log("Save does not contain key "+ missionDefinition.ID+" Adding it to save: Missiondefinition is: " + missionDefinition.type);
                missionSaveDatas.Add(missionDefinition.ID, new MissionSaveData());
                // If Pool Definition start inactive
                if (missionDefinition.type == MissionType.Pool) missionSaveDatas[missionDefinition.ID].active = false;
                continue;
            }
            Debug.Log("Save contains ID: "+ missionDefinition.ID+" with latest completion: "+ missionSaveDatas[missionDefinition.ID].lastCompletion.ToString());
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

            if(missionDefinition.completeType == CompleteType.CompleteTier)
                correspondingSave.amount = SavingUtility.playerGameData.PlayerLevelDataList.AmountCompletedOfTier(missionDefinition.tier);

            // Check for missions that are one time only (and completed) and dont add them to the list
            if (missionDefinition.type == MissionType.Single && correspondingSave.everCompleted)
            {
                // This mission is one time only and already completed do not add to list (keep the data though to keep that info saved to file)
                Debug.Log("Mission Data says it is already completed do not make a mission instance. ");
                continue;
            }


            int prefabVariant = 0;
            switch (missionDefinition.type)
            {
                case MissionType.Single:
                    prefabVariant = 1;
                    break;
                case MissionType.Pool:
                    prefabVariant = 2;
                    break;
            }

            Mission missionPrefab = missionPrefabs[prefabVariant];

            // Generate The mission
            Mission newMission = Instantiate(missionPrefab, missionHolder.transform);
            newMission.SetData(missionDefinition, correspondingSave);
            missions.Add(newMission); //Debug.Log("Generated Mission " + missionDefinition.missionName + ", active:"+correspondingSave.active);

            // Add to correct List
            switch (missionDefinition.type)
            {
                case MissionType.Single:
                    if(missionDefinition.completeType == CompleteType.PlayTime)
                        playTimeMissions.Add(newMission);
                    else if (missionDefinition.completeType == CompleteType.WatchAds)
                        watchAdMissions.Add(newMission);
                    break;
                case MissionType.Hourly:
                case MissionType.Daily:
                case MissionType.Weekly:
                    timedMissions.Add(newMission);
                    break;
                case MissionType.Pool:
                    pooledMissions.Add(newMission);
                    break;
            }
        }

        UpdateTimedMissions();
        UpdateOnlyOneMissions();
        UnlockRandomPooledMission();
    }

    private void UpdateOnlyOneMissions()
    {
        ShowBestMissionForSpecificMissionList(playTimeMissions);
        ShowBestMissionForSpecificMissionList(watchAdMissions);
    }

    private void ShowBestMissionForSpecificMissionList(List<Mission> list)
    {
        if (list.Count == 0) return;

        Mission bestMission = list[0];
        bestMission.SetActive(true);

        if (list.Count == 1) return;

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].GetMissionDefinition().completeAmount < bestMission.GetMissionDefinition().completeAmount)
            {
                bestMission.SetActive(false);
                bestMission = list[i];
                continue;
            }
            list[i].SetActive(false);
        }

        // Make sure that the newly enabled mission get its save data updated
        if (bestMission.GetMissionDefinition().completeType == CompleteType.PlayTime)
            missionSaveDatas[bestMission.GetMissionDefinition().ID].amount = SavingUtility.playerGameData.PlayTime;
        if (bestMission.GetMissionDefinition().completeType == CompleteType.WatchAds)
            missionSaveDatas[bestMission.GetMissionDefinition().ID].amount = SavingUtility.playerGameData.AdsWatched;

    }

    private void Update()
    {
        // This hinders mission to be updated and notifying player with flashing icon on level select screen that reward is available
        //if (!Enabled()) return;


        // Always check this, missions can be unlocked even if panel is inactive
        updateTimer -= Time.deltaTime;
        // Only update every Second?
        if (timedMissions.Count > 0 && updateTimer <= 0)
        {
            updateTimer = UpdateTime;
            UpdateTimedMissions();
        }
    }

    private void UpdateTimedMissions()
    {
        foreach(var mission in timedMissions)
            mission.CheckForTimedReactivation();
    }

    private void UnlockRandomPooledMission()
    {
        if (AnyPooledMissionActive()) return;

        int variants = pooledMissions.Count;
        int randomVariant = Random.Range(0,variants);

        Mission mission = pooledMissions[randomVariant];
        mission.SetActive();
        mission.Initiate();
        Debug.Log("Activating pooled mission: " + mission.GetMissionDefinition().missionName);
    }

    private bool AnyPooledMissionActive()
    {
        foreach (var mission in pooledMissions)
            if(mission.gameObject.activeSelf) return true;
        Debug.Log("No active pooled missions");
        return false;
    }    
}