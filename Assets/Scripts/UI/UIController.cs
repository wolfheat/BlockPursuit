using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] StartMenu startMenu;
    [SerializeField] LevelComplete levelComplete;
    [SerializeField] LevelSelect levelSelect;
    [SerializeField] TransitionScreen transitionScreen;
    [SerializeField] SavingUtility savingUtility;
    [SerializeField] LevelCreator levelCreator;
    [SerializeField] IngameUIController ingameUIController;
    [SerializeField] InventoryUI inventoryUI;


    private void OnEnable()
    {
        transitionScreen.GameDarkEvent += DoStoredAction;
        transitionScreen.GameDarkEventComplete += DarkEventComplete;
        savingUtility.LoadingComplete += UpdateInventoryFromStored;        
    }
    private void OnDisable()
    {
        transitionScreen.GameDarkEvent -= DoStoredAction;
        transitionScreen.GameDarkEventComplete -= DarkEventComplete;
        savingUtility.LoadingComplete -= UpdateInventoryFromStored;        
    }

    private void Start()
    {
        Debug.Log("toggle on Start Menu");
        startMenu.ShowPanel();
    }

    internal void UpdateInventoryFromStored()
    {
        Debug.Log("Update Inventory from stored");
        if(GameSettings.PlayerInventory == null)
        {
            Debug.Log("Player settings = null");
            int tileVersions = FindObjectOfType<TileLibrary>().tileDefinitions.Count;
            GameSettings.PlayerInventory = new PlayerInventory(tileVersions);
        }else if (GameSettings.PlayerInventory.Tiles.Count == 0)
        {
            Debug.Log("tiles count = 0");
        }

        ingameUIController.UpdateStats();

    }

    private void HideAllPanels()
    {
        levelComplete.HidePanel();
        startMenu.HidePanel();
        levelSelect.HidePanel();
        inventoryUI.HidePanel();
        ingameUIController.HidePanel();
    }

    internal void DarkEventComplete()
    {

        if(GameSettings.StoredAction != GameAction.ShowInventory)
        {
            GameSettings.IsPaused = false;
        }
    }

    internal void DoStoredAction()
    {
        switch (GameSettings.StoredAction)
        {
            case GameAction.LoadSelectedLevel:
                HideAllPanels();
                ingameUIController.ShowPanel();
                levelCreator.LoadSelectedLevel();
                GameSettings.LevelStartTime = Time.time;
                GameSettings.MoveCounter = 0;
                GameSettings.StepsCounter = 0;
                break;
            case GameAction.RestartLevel:
                levelCreator.RestartLevel();
                GameSettings.LevelStartTime = Time.time;
                GameSettings.MoveCounter = 0;
                GameSettings.StepsCounter = 0;
                break;
            case GameAction.LoadStartMenu:
                HideAllPanels();
                startMenu.ShowPanel();
                startMenu.SetSelected();
                break;
            case GameAction.ShowLevelSelect:
                HideAllPanels();   
                levelSelect.ShowPanel();
                levelSelect.SetSelected();
                break;
            case GameAction.ShowLevelComplete:
                levelCreator.ClearLevel();
                HideAllPanels();   
                levelComplete.ShowPanel();
                levelComplete.UpdateStats();
                break;
             case GameAction.ShowInventory:
                GameSettings.IsPaused = true;
                inventoryUI.ShowPanel();
                inventoryUI.UpdateInventoryUI();
                inventoryUI.SetSelected();
                break;
            case GameAction.HideInventory:
                inventoryUI.HidePanel();
                GameSettings.IsPaused = false;
                break;
            case GameAction.none:
                break;
            default:
                break;
        }
    }

    internal void StartTransition()
    {
        transitionScreen.StartTransition();
    }

    internal void UpdateStats()
    {
        ingameUIController.UpdateStats();
    }
}
