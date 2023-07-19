using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IngameUIController : BasePanel
{
    [SerializeField] TextMeshProUGUI level;
    //[SerializeField] GameObject activeBoostsIcons;

    private PauseUI pauseMenu;
    private UIController UIController;
    private RestartPanelController restartPanel;
    //RestartPanelController restartPanel;


    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.performed += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.performed -= RequestESC;
    }

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseUI>();
        UIController = FindObjectOfType<UIController>();
        restartPanel = FindObjectOfType<RestartPanelController>();
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from in game");
        ShowInventory();
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
    }

    public void RestartLevelRequest()
    {
        if (GameSettings.InTransition) return;

        restartPanel.ShowPanel();
        restartPanel.SetSelected();
        GameSettings.IsPaused = true;
    }
    public void ShowInventory()
    {
        if (GameSettings.InTransition) return;

        Debug.Log("Main Menu Clicked");
        GameSettings.IsPaused = true;
        TransitionScreen.Instance.StartTransition(GameAction.ShowInventory);
    }
}
