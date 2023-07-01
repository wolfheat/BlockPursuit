using System.Linq;
using TMPro;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI tiles;

    public void UpdateStats()
    {
        coins.text = GameSettings.PlayerInventory.Coins.ToString();

        //Calculate total Tiles
        int sum = GameSettings.PlayerInventory.Tiles.Sum(x=>x.Value);
        tiles.text = sum.ToString();
    }

}
