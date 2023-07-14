using GoogleMobileAds.Sample;
using MyGameAds;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] RewardedController rewardedController;
    [SerializeField] InterstitialController interstitialController;

    [SerializeField] StartMenu startMenu;
    [SerializeField] LevelComplete levelComplete;
    [SerializeField] LevelSelect levelSelect;
    [SerializeField] TransitionScreen transitionScreen;
    [SerializeField] LevelCreator levelCreator;
    [SerializeField] IngameUIController ingameUIController;
    [SerializeField] InventoryUI inventoryUI;

    private Time lastInterstitial;
    private Time lastRewarded;

    private void OnEnable()
    {
        transitionScreen.GameDarkEvent += DoStoredAction;
        transitionScreen.GameDarkEventComplete += DarkEventComplete;
        SavingUtility.LoadingComplete += UpdateInventoryFromStored;        
        Inputs.Instance.Controls.Main.Plus.performed += AddTiles;        
    }
    private void OnDisable()
    {
        transitionScreen.GameDarkEvent -= DoStoredAction;
        transitionScreen.GameDarkEventComplete -= DarkEventComplete;
        SavingUtility.LoadingComplete -= UpdateInventoryFromStored;        
        Inputs.Instance.Controls.Main.Plus.performed -= AddTiles;        
    }

    private void Start()
    {
        Debug.Log("toggle on Start Menu");
        startMenu.ShowPanel();
    }

    internal void UpdateIngameLevelShown()
    {
        ingameUIController.UpdateLevel();
    }
    
    internal void AddTiles(InputAction.CallbackContext context)
    {
        Debug.Log("Adding tile to player");
        SavingUtility.playerGameData.AddTiles(1);

    }
    internal void UpdateInventoryFromStored()
    {
        Debug.Log("Update Inventory from stored");        
        ingameUIController.UpdateInventory();

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

        if(GameSettings.StoredAction == GameAction.HideInventory || GameSettings.StoredAction == GameAction.LoadSelectedLevel)
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

                interstitialController.LoadAd();

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
                levelSelect.SetSelectedLevelToDefaultForActiveTab();
                break;
            case GameAction.ShowLevelComplete:
                levelCreator.ClearLevel();
                HideAllPanels();   
                levelComplete.ShowPanel();
                // Load Ad here? This is in the middle of the transition To show Level Complete
                interstitialController.ShowAd();
                rewardedController.LoadAd();

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
        ingameUIController.UpdateInventory();
    }

    public void RequestDoubleAds()
    { 
        Debug.Log("Requested Show Double Reward Ads");
        FindObjectOfType<RewardedController>().LoadAd();
    }

}
