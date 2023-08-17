using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI tiles;

    private int countingTiles = 0;
    private int countingCoins = 0;
    private int newCoins;
    private int newTiles;

    private Coroutine countingCoroutine;

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
        newCoins = SavingUtility.playerGameData.Coins;
        newTiles = SavingUtility.playerGameData.Tiles;
        if (countingCoroutine != null) StopCoroutine(countingCoroutine);
        countingCoroutine = StartCoroutine(CountCoroutine());
    }
    
    public void UpdateCoinText()
    {
        coins.text = countingCoins.ToString();
    }
    
    public void UpdateTileText()
    {
        tiles.text = countingTiles.ToString();  
    }

    private IEnumerator CountCoroutine()
    {
        
        // Count up or down
        int dir = newTiles > countingTiles ? 1 : -1;
        
        // Initial wait
        yield return new WaitForSeconds(0.5f);

        //Tiles Count
        while (countingTiles != newTiles) // 0 < 0 if no new tiles
        {
            // Play sound?
            // Animate tile stack? Effect?
            countingTiles += dir;
            UpdateTileText();
            yield return new WaitForSeconds(0.1f);
        }

        //Maybe have a wait in between the counts?

        // Count up or down
        dir = newCoins > countingCoins ? 1 : -1;

        //Coins Count
        while (countingCoins != newCoins)
        {
            // Play sound?
            // Animate coin stack?
            if(dir*(newCoins - countingCoins) > 100)
                countingCoins += dir*10;
            else
                countingCoins += dir;
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
