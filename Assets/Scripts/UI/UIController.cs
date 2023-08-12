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
    [SerializeField] CustomizationController customize;
    [SerializeField] TransitionScreen transitionScreen;
    [SerializeField] ConfirmResetScreen confirmResetScreen;
    [SerializeField] LevelCreator levelCreator;
    [SerializeField] IngameUIController ingameUIController;
    [SerializeField] BoostController boostController;
    [SerializeField] InventoryBar inventoryBar;
    [SerializeField] PauseUI pauseScreen;
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
        startMenu.ShowPanel();
        startMenu.SetSelected();
        inventoryBar.UpdateInventory();
    }

    private void HideAllPanels()
    {
        startMenu.HidePanel();
        levelSelect.HidePanel();
        levelComplete.HidePanel();
        pauseScreen.HidePanel();
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
        else if(GameSettings.StoredAction == GameAction.HidePauseScreen)
            GameSettings.IsPaused = false;

    }

    internal void DoStoredAction()
    {
        
        switch (GameSettings.StoredAction)
        {
            case GameAction.LoadSelectedLevel:
                levelSelect.HidePanel();
                ingameUIController.HidePanel();
                boostController.ShowPanel();
                rewardedController.LoadAd();

                break;
            case GameAction.RestartLevel:
                // Do I need to unload current game here?
                ingameUIController.HidePanel();
                boostController.ShowPanel();
                break;
            case GameAction.LoadStartMenu:
                startMenu.ShowPanel();
                break;
            case GameAction.ShowLevelSelect:
                startMenu.HidePanel();
                pauseScreen.HidePanel();
                levelComplete.HidePanel();
                boostController.HidePanel();
                levelSelect.ShowPanel();
                //levelSelect.SetSelected();
                break;
            case GameAction.ShowLevelComplete:
                ingameUIController.HidePanel();
                levelCreator.ClearLevel();
                levelComplete.ShowPanel();
                // Load Ad here? This is in the middle of the transition To show Level Complete
                // This is good because it is a delay between player completes level and ad shows
                interstitialController.ShowAd();
                rewardedController.LoadAd();

                break;
             case GameAction.ShowPauseScreen:
                ingameUIController.HidePanel();
                pauseScreen.ShowPanel();
                break;
            case GameAction.HidePauseScreen:
                GameSettings.IsPaused = false;
                pauseScreen.HidePanel();
                ingameUIController.ShowPanel();
                break;
            case GameAction.HideBoostPanel:
                // Game unpaused and all values reset when closing this panel
                // Have level load when player press start level instead of when showing boost panel?
                levelCreator.LoadSelectedLevel();

                boostController.HidePanel();
                ingameUIController.ShowPanel();

                interstitialController.LoadAd(); // Loaded async now but showed after player complete level
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
                startMenu.HidePanel();
                ingameUIController.HidePanel();
                settings.ShowPanel();
                break;
            case GameAction.HideSettings:
                settings.HidePanel();
                if(settings.ReturnMenu == ReturnMenuType.Main)
                    startMenu.ShowPanel();
                else
                    ingameUIController.ShowPanel();
                break;
             case GameAction.HideSettingsInGame:
                settings.HidePanel();
                break;
            case GameAction.ShowCredits:
                startMenu.HidePanel();
                credits.ShowPanel();
                break;
             case GameAction.HideCredits:
                credits.HidePanel();
                startMenu.ShowPanel();
                break;
            case GameAction.ShowAchievements:
                startMenu.HidePanel();
                achievements.ShowPanel();
                break;
             case GameAction.HideAchievements:
                achievements.HidePanel();
                startMenu.ShowPanel();
                break;
            case GameAction.ShowUnlock:
                levelSelect.HidePanel();
                unlockScreen.ShowPanel();
                break;
             case GameAction.HideUnlock:
                unlockScreen.HidePanel();
                levelSelect.ShowPanel();
                break;
            case GameAction.ShowCustomize:
                startMenu.HidePanel();
                customize.ShowPanel();
                break;
             case GameAction.HideCustomize:
                customize.HidePanel();
                startMenu.ShowPanel();
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
    internal void RequestCustomize()
    {
        latestSelected = EventSystem.current.currentSelectedGameObject;
        TransitionScreen.Instance.StartTransition(GameAction.ShowCustomize);
    }
    internal void RequestCredits()
    {
        latestSelected = EventSystem.current.currentSelectedGameObject;
        TransitionScreen.Instance.StartTransition(GameAction.ShowCredits);
    }
    internal void RequestSettings(ReturnMenuType origin)
    {
        settings.UpdatePanelFromStored();
        settings.ReturnMenu = origin;
        TransitionScreen.Instance.StartTransition(GameAction.ShowSettings);        
    }
}
