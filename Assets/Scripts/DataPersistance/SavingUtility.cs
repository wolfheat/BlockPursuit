using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SavingUtility : MonoBehaviour
{

    private const string SaveFileName = "/player-data.txt";
    public static SavingUtility Instance { get; private set; }

    public static Action LoadingComplete;  

    public static PlayerGameData playerGameData;



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
        playerGameData = new PlayerGameData();
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(SaveFileName, playerGameData, false))
            Debug.Log("Player save file was reset: "+SaveFileName);
        else
            Debug.LogError("Could not reset file.");
    }
    
    public void SaveToFile()
    {
        LogSaveInfo();
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(SaveFileName, playerGameData, false))
            Debug.Log("Saved in: "+SaveFileName);
        else
            Debug.LogError("Could not save file.");
    }

    private void LogSaveInfo()
    {
        Debug.Log(" -- Saving To File -- START");
        LogInfo();
        Debug.Log(" -- Saving To File -- END");
    }
    private void LogLoadInfo()
    {
        Debug.Log(" -- Loaded File INFO -- START");
        LogInfo();
        Debug.Log(" -- Loaded File INFO -- END");
    }
    private void LogInfo()
    {
        List<PlayerLevelData> data = playerGameData.PlayerLevelDataList.LevelsList;
        StringBuilder sb = new StringBuilder();
        sb.Append("Levels (");
        foreach (PlayerLevelData levelData in data)
            sb.Append(levelData.levelID);
        sb.Append(")");
        Debug.Log(sb);
        Debug.Log("Boost time saved A: " + playerGameData.AtypeBoostTime+" B: "+ playerGameData.BtypeBoostTime);
        Debug.Log("Volume: " + ((playerGameData.soundSettings==null)?"UNDEFINED": playerGameData.soundSettings.MusicVolume+
                    " SFX: " + ((playerGameData.soundSettings==null)?"UNDEFINED": playerGameData.soundSettings.SFXVolume)));

    }

    public IEnumerator LoadFromFile()
    {
        // Hold the load so Game has time to load
        yield return new WaitForSeconds(0.4f);

        IDataService dataService = new JsonDataService();
        try
        {
            playerGameData = dataService.LoadData<PlayerGameData>(SaveFileName, false);

            Debug.Log("Setting player game data from loaded file");

            //Data is now set Dispatch update event before adding listeners to the save
            //PlayerGameData.InvokeAll();

            // Add listener to update of data to save
            PlayerLevelDataList.PlayerLevelDataListUpdate += OnPlayerSaveDataUpdated;
            PlayerGameData.InventoryUpdate += OnPlayerSaveDataUpdated;
            PlayerGameData.BoostTimeUpdated += OnPlayerSaveDataUpdated;
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
            playerGameData = new PlayerGameData();
        }
        finally
        {
            // Make sure soundSettings is initialized
            if(playerGameData.soundSettings == null)
            {
                Debug.Log("!!  Initialize new sound settings!!! ");
                playerGameData.soundSettings = new SoundSettings();
            }


            Debug.Log(" -- Loading From File -- FINALLY");
            LogLoadInfo();
            LoadingComplete.Invoke();
        }
    }

}
