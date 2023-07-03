using System.Linq;
using TMPro;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI tiles;
    [SerializeField] TextMeshProUGUI level;

    public void UpdateStats()
    {
        coins.text = GameSettings.PlayerInventory.Coins.ToString();

        //Calculate total Tiles
        int sum = GameSettings.PlayerInventory.Tiles.Sum(x=>x.Value);
        tiles.text = sum.ToString();

        level.text = "Level "+GameSettings.CurrentLevel.ToString();


    }

    public void ShowInventoryClicked()
    {
        GameSettings.StoredAction = GameAction.ShowInventory;
        FindObjectOfType<TransitionScreen>().StartTransition();
    }
}
