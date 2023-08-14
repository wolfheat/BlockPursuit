using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SavingUtility : MonoBehaviour
{

    private const string PlayerDataSaveFile = "/player-data.txt";
    private const string GameSettingsDataSaveFile = "/player-settings.txt";
    public static SavingUtility Instance { get; private set; }

    public static Action LoadingComplete;  

    public static PlayerGameData playerGameData;
    public static GameSettingsData gameSettingsData;


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

    public void ResetSaveFile()
    {
        Debug.Log("Resetting save data, keeps the game settings data");
        playerGameData = new PlayerGameData();
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(PlayerDataSaveFile, playerGameData, false))
            Debug.Log("Player save file was reset: "+PlayerDataSaveFile);
        else
            Debug.LogError("Could not reset file.");
            
        LoadingComplete?.Invoke(); // Call this to update all ingame values
    }
    
    public void SaveToFile()
    {
        LogSaveInfo();
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(PlayerDataSaveFile, playerGameData, false))
            Debug.Log("Saved player data in: "+PlayerDataSaveFile);
        else
            Debug.LogError("Could not save file: PlayerData");

        if (dataService.SaveData(GameSettingsDataSaveFile, gameSettingsData, false))
            Debug.Log("Saved settings data in: "+GameSettingsDataSaveFile);
        else
            Debug.LogError("Could not save file: GameSettingsData");
    }

    private void LogSaveInfo()
    {
        Debug.Log(" - SAVE INFO - START");
        LogInfo();
        Debug.Log(" - SAVE INFO - END");
    }
    private void LogLoadInfo()
    {
        Debug.Log(" - LOAD INFO - START");
        LogInfo();
        Debug.Log(" - LOAD INFO - END");
    }
    private void LogInfo()
    {
        List<PlayerLevelData> data = playerGameData.PlayerLevelDataList.LevelsList;
        StringBuilder sb = new StringBuilder();
        sb.Append(" -- Levels (");
        foreach (PlayerLevelData levelData in data)
            sb.Append(levelData.levelID);
        sb.Append(")");
        Debug.Log(sb);
        Debug.Log(" -- Input Touch Setting: " + gameSettingsData.ActiveTouchControl+" Camera position: "+ gameSettingsData.CameraPos);
        Debug.Log(" -- Boost time saved A: " + playerGameData.AtypeBoostTime+" B: "+ playerGameData.BtypeBoostTime);
        Debug.Log(" -- Volume: " + ((gameSettingsData.soundSettings==null)?"UNDEFINED": gameSettingsData.soundSettings.MusicVolume+
                    " SFX: " + ((gameSettingsData.soundSettings==null)?"UNDEFINED": gameSettingsData.soundSettings.SFXVolume)));

    }

    public IEnumerator LoadFromFile()
    {
        // Hold the load so Game has time to load
        yield return new WaitForSeconds(0.4f);

        IDataService dataService = new JsonDataService();
        try
        {
            playerGameData = dataService.LoadData<PlayerGameData>(PlayerDataSaveFile, false);            
        }
        catch   
        {
            playerGameData = new PlayerGameData();                        
        }
        try
        {
            gameSettingsData = dataService.LoadData<GameSettingsData>(GameSettingsDataSaveFile, false);
        }
        catch
        {
            gameSettingsData = new GameSettingsData();
        }
        finally
        {
            // Add listener to update of data to save
            PlayerLevelDataList.PlayerLevelDataListUpdate += OnPlayerSaveDataUpdated;
            PlayerGameData.InventoryUpdate += OnPlayerSaveDataUpdated;
            PlayerGameData.BoostTimeUpdated += OnPlayerSaveDataUpdated;
            PlayerGameData.AvatarChange += OnPlayerSaveDataUpdated;
            GameSettingsData.InputSettingUpdate += OnPlayerSaveDataUpdated;

            Debug.Log(" -- Loading From File -- FINALLY");
            LogLoadInfo();
            LoadingComplete.Invoke();
        }
    }

}
