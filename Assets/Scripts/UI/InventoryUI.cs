using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : BasePanel
{

    [SerializeField] List<TextMeshProUGUI> inventoryTilesTexts = new List<TextMeshProUGUI>();

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
}
