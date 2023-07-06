using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : BasePanel
{
    [SerializeField] TextMeshProUGUI inventoryTilesText;

    [SerializeField] Button mainSelectedButton;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void UpdateInventoryUI()
    {
        inventoryTilesText.text = playerInventory.Tiles.ToString();
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
