using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;

public class SavingUtility : MonoBehaviour
{

    private const string SaveFileName = "/player-data.txt";
    public static SavingUtility Instance { get; set; }

    public static Action LoadingComplete;  

    public static PlayerGameData playerGameData;


    private void OnEnable()
    {
        // Unsure if this will run on mobile when exiting
        PlayerLevelDataList.PlayerLevelDataListUpdate += OnPlayerSaveDataUpdated;
        PlayerGameData.InventoryUpdate += OnPlayerSaveDataUpdated;
        PlayerGameData.BoostTimeUpdated += OnPlayerSaveDataUpdated;
    }
    
    private void OnDisable()
    {
        // Unsure if this will run on mobile when exiting
        PlayerLevelDataList.PlayerLevelDataListUpdate -= OnPlayerSaveDataUpdated;
        PlayerGameData.InventoryUpdate -= OnPlayerSaveDataUpdated;
        PlayerGameData.BoostTimeUpdated -= OnPlayerSaveDataUpdated;
    }


    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;    
        }
        StartCoroutine(LoadFromFile());
    }

    private void OnPlayerSaveDataUpdated()
    {
        SaveToFile();
    }

    public void SaveToFile()
    {
        LogWhatsSaved();
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(SaveFileName, playerGameData, false))
            Debug.Log("Saved in: "+SaveFileName);
        else
            Debug.LogError("Could not save file.");
    }

    private void LogWhatsSaved()
    {
        List<PlayerLevelData> data = playerGameData.PlayerLevelDataList.LevelsList;
        foreach (PlayerLevelData levelData in data)
        {
            Debug.Log("Saving level ID: "+levelData.levelID);
        }
        Debug.Log("Saving AtypeBoostTime: "+playerGameData.AtypeBoostTime);
    }

    public IEnumerator LoadFromFile()
    {
        // Hold the load so Game has time to load
        yield return new WaitForSeconds(0.4f);

        IDataService dataService = new JsonDataService();
        try
        {
            playerGameData = dataService.LoadData<PlayerGameData>(SaveFileName, false);

            // Add listener to update of data to save
            PlayerLevelDataList.PlayerLevelDataListUpdate += OnPlayerSaveDataUpdated;
            PlayerGameData.InventoryUpdate += OnPlayerSaveDataUpdated;

            Debug.Log(" - Loading items from file! - ");

            Debug.Log("AtypeBoostTime: " + playerGameData.AtypeBoostTime);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
            playerGameData = new PlayerGameData();
        }
        finally
        {
            Debug.Log("Items loaded from file: FINALLY");
            LoadingComplete.Invoke();
        }
    }

}
