using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseUI : BasePanel
{
    [SerializeField] TextMeshProUGUI inventoryTilesText;
    [SerializeField] TextMeshProUGUI inventoryCoinsText;

    [SerializeField] Button mainSelectedButton;

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void UpdateInventoryUI()
    {
        inventoryTilesText.text = SavingUtility.playerGameData.Tiles.ToString();
        inventoryCoinsText.text = SavingUtility.playerGameData.Coins.ToString();
    }
    public void CloseInventoryRequest()
    {
        FindObjectOfType<TransitionScreen>().StartTransition(GameAction.HideInventory);
    }

    public void QuitLevelClicked()
    {
        Debug.Log("Quit Level Clicked"); 
        FindObjectOfType<TransitionScreen>().StartTransition(GameAction.ShowLevelSelect);
    }

}
