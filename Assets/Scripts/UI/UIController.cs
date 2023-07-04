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
        savingUtility.LoadingComplete += UpdateInventoryFromStored;        
    }
    private void OnDisable()
    {
        transitionScreen.GameDarkEvent -= DoStoredAction;
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

    internal void DoStoredAction()
    {
        switch (GameSettings.StoredAction)
        {
            case GameAction.LoadSelectedLevel:
                levelComplete.HidePanel();
                startMenu.HidePanel();
                levelSelect.HidePanel();
                levelCreator.LoadSelectedLevel();
                GameSettings.LevelStartTime = Time.time;
                GameSettings.MoveCounter = 0;
                GameSettings.StepsCounter = 0;
                break;
            case GameAction.LoadStartMenu:
                levelComplete.HidePanel();
                levelSelect.HidePanel();
                startMenu.ShowPanel();
                break;
            case GameAction.ShowLevelSelect:
                startMenu.HidePanel();
                levelSelect.ShowPanel();
                levelSelect.UpdateSelectedLevel();
                break;
            case GameAction.ShowLevelComplete:
                levelCreator.ClearLevel();
                levelComplete.ShowPanel();
                levelComplete.UpdateStats();
                levelSelect.UpdateLevelVisability();
                break;
             case GameAction.ShowInventory:
                GameSettings.IsPaused = true;
                inventoryUI.ShowPanel();
                inventoryUI.UpdateInventoryUI();
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
