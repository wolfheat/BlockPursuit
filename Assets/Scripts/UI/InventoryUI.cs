using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : BasePanel
{
    [SerializeField] List<TextMeshProUGUI> inventoryTilesTexts = new List<TextMeshProUGUI>();

    [SerializeField] Button mainSelectedButton;

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < GameSettings.PlayerInventory.Tiles.Count; i++)
            inventoryTilesTexts[i].text = GameSettings.PlayerInventory.Tiles[i].ToString();
    }
    public void CloseInventoryRequest()
    {
        GameSettings.StoredAction = GameAction.HideInventory;
        FindObjectOfType<TransitionScreen>().StartTransition();
    }

    public void QuitLevelClicked()
    {
        Debug.Log("Quit Level Clicked"); 
        GameSettings.StoredAction = GameAction.ShowLevelSelect;
        FindObjectOfType<TransitionScreen>().StartTransition();
    }

}
