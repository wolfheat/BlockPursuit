using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class IngameUIController : EscapableBasePanel
{
    [SerializeField] TextMeshProUGUI level;
    //[SerializeField] GameObject activeBoostsIcons;

    private PauseUI pauseMenu;
    private UIController UIController;
    private RestartPanelController restartPanel;
    private FollowPlayer followPlayer;
    //RestartPanelController restartPanel;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseUI>();
        UIController = FindObjectOfType<UIController>();
        restartPanel = FindObjectOfType<RestartPanelController>();
        followPlayer = FindObjectOfType<FollowPlayer>();
    }

     public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from in game");
        ShowPauseScreen();
    }
    public void UpdateLevel()
    {
        level.text = "Level "+StringConverter.LevelAsString(GameSettings.CurrentLevelDefinition.LevelDiff, GameSettings.CurrentLevelDefinition.LevelIndex);
    }
    
    public void RequestBoostMenu()
    {
        if (GameSettings.InTransition) return;
    
        // Show Boost Menu
    
        restartPanel.ShowPanel();
        restartPanel.SetSelected();
        GameSettings.IsPaused = true;
        SoundController.Instance.PlaySFX(SFX.MenuStep);
    }

    public void RestartLevelRequest()
    {
        if (GameSettings.InTransition) return;

        restartPanel.ShowPanel();
        restartPanel.SetSelected();
        GameSettings.IsPaused = true;
        SoundController.Instance.PlaySFX(SFX.MenuStep);
    }
    public void ToggleCamera()
    {
        followPlayer.ChangeView();
        SoundController.Instance.PlaySFX(SFX.MenuStep);
    }
    public void RestartSettigsMenu()
    {
        if (GameSettings.InTransition) return;
        UIController.RequestSettings(ReturnMenuType.InGame);
        SoundController.Instance.PlaySFX(SFX.MenuStep);
    }
    public void ShowPauseScreen()
    {
        if (GameSettings.InTransition) return;

        Debug.Log("Main Menu Clicked");
        GameSettings.IsPaused = true;
        TransitionScreen.Instance.StartTransition(GameAction.ShowPauseScreen);
        SoundController.Instance.PlaySFX(SFX.MenuStep);
    }
}
