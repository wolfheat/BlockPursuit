using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SavingUtility : MonoBehaviour
{

    private const string LocalSaveLocation = "/player-data.json";
    public static SavingUtility Instance { get; set; }

    public static Action LoadingComplete;  

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
        if (dataService.SaveData(LocalSaveLocation, FindObjectOfType<PlayerInventory>(), false))
            Debug.Log("Saved in: "+LocalSaveLocation);
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
            FindObjectOfType<PlayerInventory>().DefineInventory(dataService.LoadData<PlayerInventory>(LocalSaveLocation, false));

            Debug.Log(" - Loading items from file! - "); 
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
            FindObjectOfType<PlayerInventory>().DefineEmptyDefinition();
        }
        finally
        {
            Debug.Log("Items loaded from file: FINALLY");
            LoadingComplete.Invoke();
        }
    }

}
