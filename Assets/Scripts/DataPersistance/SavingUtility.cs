using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SavingUtility : MonoBehaviour
{

    private const string LocalSaveLocation = "/player-data.json";
    public static SavingUtility Instance { get; set; }

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

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        Debug.Log("Exiting in Unity Editor = Do not Save to file");
        SaveToFile();
#else
#endif
    }

    public void SaveToFile()
    {
        LogWhatsSaved();
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(LocalSaveLocation, playerGameData, false))
            Debug.Log("Saved in: "+LocalSaveLocation);
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
    }

    public IEnumerator LoadFromFile()
    {
        // Hold the load so Game has time to load
        yield return new WaitForSeconds(0.4f);

        IDataService dataService = new JsonDataService();
        try
        {
            playerGameData = dataService.LoadData<PlayerGameData>(LocalSaveLocation, false);

            Debug.Log(" - Loading items from file! - "); 
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
