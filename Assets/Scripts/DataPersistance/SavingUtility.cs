using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SavingUtility : MonoBehaviour
{

    private const string LocalSaveLovation = "/player-data.json";
    public static SavingUtility Instance { get; set; }

    public Action LoadingComplete;  

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
#else
        SaveToFile();
#endif
    }

    public void SaveToFile()
    {
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(LocalSaveLovation, GameSettings.PlayerInventory, false))
            Debug.Log("Saved in: "+LocalSaveLovation);
        else
            Debug.LogError("Could not save file.");
    }
    public IEnumerator LoadFromFile()
    {
        // Hold the load so Game has time to load
        yield return new WaitForSeconds(0.4f);

        IDataService dataService = new JsonDataService();
        try
        {
            GameSettings.PlayerInventory = dataService.LoadData<PlayerInventory>(LocalSaveLovation, false);
            Debug.Log(" - Loading items from file! - "); 
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
        }
        finally
        {
            Debug.Log("Items loaded from file: FINALLY");
            LoadingComplete.Invoke();
        }
    }

}
