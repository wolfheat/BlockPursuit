using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IngameUIController : BasePanel
{
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI tiles;
    [SerializeField] TextMeshProUGUI level;

    InventoryUI inventory;
    TransitionScreen transitionScreen;

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.performed += RequestInventory;
        PlayerGameData.InventoryUpdate += UpdateInventory;
    }
    
    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.performed -= RequestInventory;
        PlayerGameData.InventoryUpdate -= UpdateInventory;
    }

    private void RequestInventory(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        ShowInventoryClicked();
    }

    private void Start()
    {
        inventory = FindObjectOfType<InventoryUI>();
        transitionScreen = FindObjectOfType<TransitionScreen>();
    }

    public void UpdateLevel()
    {
        level.text = "Level "+StringConverter.LevelAsString(GameSettings.CurrentLevelDefinition.LevelDiff, GameSettings.CurrentLevelDefinition.LevelIndex);
    }

    public void UpdateInventory()
    {
        coins.text = SavingUtility.playerGameData.Coins.ToString();
        tiles.text = SavingUtility.playerGameData.Tiles.ToString();        
    }

    public void RestartLevelRequest()
    {
        if (GameSettings.InTransition) return;
        Debug.Log("Restart Level");
        GameSettings.StoredAction = GameAction.RestartLevel;
        transitionScreen.StartTransition();
    }
    public void ShowInventoryClicked()
    {
        if (!inventory.Enabled())
        {
            if (GameSettings.IsPaused) return; // Transitioning already
            GameSettings.IsPaused = true;
            GameSettings.StoredAction = GameAction.ShowInventory;
            transitionScreen.StartTransition();
        }
        else
        {
            GameSettings.StoredAction = GameAction.HideInventory;
            transitionScreen.StartTransition();
        }
    }
}
