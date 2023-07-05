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
    }
    
    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.performed -= RequestInventory;
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

    public void UpdateStats()
    {
        coins.text = GameSettings.PlayerInventory.Coins.ToString();

        //Calculate total Tiles
        int sum = GameSettings.PlayerInventory.Tiles.Sum(x=>x.Value);
        tiles.text = sum.ToString();

        level.text = "Level "+(GameSettings.CurrentLevel+1).ToString();
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