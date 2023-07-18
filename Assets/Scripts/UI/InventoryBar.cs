using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI tiles;

    private int lastCoins = 0;
    private int newCoins;

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

        newCoins = SavingUtility.playerGameData.Coins;
        StartCoroutine(CountCoroutine());
    }
    
    public void UpdateCoinText()
    {
        coins.text = lastCoins.ToString();
    }

    private IEnumerator CountCoroutine()
    {
        int dir = newCoins > lastCoins ? 1 : -1;

        while (lastCoins < newCoins)
        {
            // Play sound?
            // Animate coin stack?
            if(dir*(newCoins - lastCoins) > 100)
                lastCoins += dir*50;
            else
                lastCoins += dir;
            UpdateCoinText();
            yield return null;
        }

    }


    public void ClickedInventory()
    {
        // Maybe make effect when clicking here?
        Debug.Log("Clicked on Inventory");
    }

}
