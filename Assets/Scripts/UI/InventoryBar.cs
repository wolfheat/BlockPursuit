using TMPro;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI tiles;

    private void OnEnable()
    {
        PlayerGameData.InventoryUpdate += UpdateInventory;
    }

    private void OnDisable()
    {
        PlayerGameData.InventoryUpdate -= UpdateInventory;
    }

    public void UpdateInventory()
    {
        coins.text = SavingUtility.playerGameData.Coins.ToString();
        tiles.text = SavingUtility.playerGameData.Tiles.ToString();
    }

    public void ClickedInventory()
    {
        // Maybe make effect when clicking here?
        Debug.Log("Clicked on Inventory");
    }

}
