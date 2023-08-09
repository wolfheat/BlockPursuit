using MyGameAds;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] RewardedController rewardedController;
    [SerializeField] InterstitialController interstitialController;

    [SerializeField] StartMenu startMenu;
    [SerializeField] LevelComplete levelComplete;
    [SerializeField] LevelSelect levelSelect;
    [SerializeField] TransitionScreen transitionScreen;
    [SerializeField] ConfirmResetScreen confirmResetScreen;
    [SerializeField] LevelCreator levelCreator;
    [SerializeField] IngameUIController ingameUIController;
    [SerializeField] BoostController boostController;
    [SerializeField] InventoryBar inventoryBar;
    [SerializeField] PauseUI inventoryUI;
    [SerializeField] SettingsController settings;
    [SerializeField] CreditsController credits;
    [SerializeField] AchievementsController achievements;    
    [SerializeField] UnlockScreen unlockScreen;

    [SerializeField] GameObject initialSelected;

    private Time lastInterstitial;
    private Time lastRewarded;

    private GameObject latestSelected;

    private void OnEnable()
    {
        latestSelected = initialSelected.gameObject;
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
    public void SetSelected()
    {
        if(startMenu.Enabled())
            EventSystem.current.SetSelectedGameObject(latestSelected.gameObject);
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
        inventoryBar.UpdateInventory();

    }

    private void HideAllPanels()
    {
        levelComplete.HidePanel();
        startMenu.HidePanel();
        levelSelect.HidePanel();
        inventoryUI.HidePanel();
        ingameUIController.HidePanel();
        boostController.HidePanel();
    }

    internal void DarkEventComplete()
    {
        // Runs at the very wnd of transition animation
        if(GameSettings.StoredAction == GameAction.HideBoostPanel)
        {
            GameSettings.IsPaused = false;
            GameSettings.CurrentGameState = GameState.RunGame;
            GameSettings.LevelStartTime = Time.time;
            GameSettings.MoveCounter = 0;
            GameSettings.StepsCounter = 0;
        }
        else if(GameSettings.StoredAction == GameAction.HideInventory)
            GameSettings.IsPaused = false;

    }

    internal void DoStoredAction()
    {
        
        switch (GameSettings.StoredAction)
        {
            case GameAction.LoadSelectedLevel:
                HideAllPanels();
                levelCreator.LoadSelectedLevel();
                boostController.ShowPanel();
                boostController.SetSelected();
                rewardedController.LoadAd();

                break;
            case GameAction.RestartLevel:
                levelCreator.RestartLevel();
                boostController.ShowPanel();
                boostController.SetSelected();
                break;
            case GameAction.LoadStartMenu:
                HideAllPanels();
                startMenu.ShowPanel();
                startMenu.SetSelected();
                break;
            case GameAction.ShowLevelSelect:
                HideAllPanels();   
                levelSelect.ShowPanel();
                levelSelect.SetSelectedLevelToLastPlayed();
                break;
            case GameAction.ShowLevelComplete:
                levelCreator.ClearLevel();
                HideAllPanels();   
                levelComplete.ShowPanel();
                // Load Ad here? This is in the middle of the transition To show Level Complete
                // This is good because it is a delay between player completes level and ad shows
                interstitialController.ShowAd();
                rewardedController.LoadAd();

                break;
             case GameAction.ShowInventory:
                inventoryUI.ShowPanel();
                inventoryUI.UpdateInventoryUI();
                inventoryUI.SetSelected();
                ingameUIController.HidePanel();
                break;
            case GameAction.HideBoostPanel:
                // Game unpaused and all values reset when closing this panel
                boostController.HidePanel();
                ingameUIController.ShowPanel();
                interstitialController.LoadAd();
                break;
            case GameAction.HideInventory:
                inventoryUI.HidePanel();
                ingameUIController.ShowPanel();
                break;
            case GameAction.ShowResetConfirm:
                settings.HidePanel();
                confirmResetScreen.ShowPanel();
                break;
            case GameAction.HideResetConfirm:
                confirmResetScreen.HidePanel();
                settings.ShowPanel();
                settings.UpdatePanelFromStored();
                settings.UpdateSavingValues();
                break;
            case GameAction.ShowSettings:
                settings.ShowPanel();
                settings.SetSelected();
                break;
             case GameAction.HideSettings:
                settings.HidePanel();
                SetSelected();
                break;
            case GameAction.ShowCredits:
                credits.ShowPanel();
                credits.SetSelected();
                break;
             case GameAction.HideCredits:
                credits.HidePanel();
                SetSelected();
                break;
            case GameAction.ShowAchievements:
                achievements.ShowPanel();
                achievements.SetSelected();
                break;
             case GameAction.HideAchievements:
                achievements.HidePanel();
                SetSelected();
                break;
            case GameAction.ShowUnlock:
                unlockScreen.ShowPanel();
                unlockScreen.SetSelected();
                break;
             case GameAction.HideUnlock:
                unlockScreen.HidePanel();
                levelSelect.SetSelected();
                break;
            case GameAction.none:
                break;
            default:
                break;
        }
    }

    internal void UpdateStats()
    {
        inventoryBar.UpdateInventory();
    }

    public void RequestDoubleAds()
    { 
        Debug.Log("Requested Show Double Reward Ads");
        FindObjectOfType<RewardedController>().LoadAd();
    }

    internal void RequestAchievements()
    {
        latestSelected = EventSystem.current.currentSelectedGameObject;
        TransitionScreen.Instance.StartTransition(GameAction.ShowAchievements);
    }
    internal void RequestCredits()
    {
        latestSelected = EventSystem.current.currentSelectedGameObject;
        TransitionScreen.Instance.StartTransition(GameAction.ShowCredits);
    }
    internal void RequestSettings()
    {
        latestSelected = EventSystem.current.currentSelectedGameObject;
        settings.UpdatePanelFromStored();
        TransitionScreen.Instance.StartTransition(GameAction.ShowSettings);
    }
}
