using System;
using UnityEngine;

public class PlayerGameData
{
    // Players Inventory
    public int Tiles { get; set; } = 0;
    public int Coins { get; set; } = 100;
    public DateTime AtypeBoostTime { get; set; }
    public DateTime BtypeBoostTime { get; set; }
    // Player Levels Data
    public PlayerLevelDataList PlayerLevelDataList { get; private set; }

    public static Action InventoryUpdate;
    public static Action BoostTimeUpdated;

    public PlayerGameData()
    {
        Tiles = 0;
        Coins = 100;
        PlayerLevelDataList = new PlayerLevelDataList();
    }

    public void SetABoostTime(DateTime time)
    {
        AtypeBoostTime = time;
        BoostTimeUpdated.Invoke();
    }
    public void SetBBoostTime(DateTime time)
    {
        BtypeBoostTime = time;
        BoostTimeUpdated.Invoke();
    }
    public void AddCoins(int amt)
    {
        Coins += amt;
        InventoryUpdate.Invoke();
    }

    public bool RemoveCoins(int amt)
    {
        if(Coins < amt) return false;
        Coins -= amt;
        InventoryUpdate.Invoke();
        return true;
    }
    public void AddTiles(int amt)
    {
        Tiles += amt;
        InventoryUpdate.Invoke();
    }

    public bool RemoveTiles(int amt)
    {
        if(Tiles < amt) return false;
        Tiles -= amt;
        InventoryUpdate.Invoke();
        return true;
    }

    internal void DefineSavingUtility(SavingUtility savingUtility)
    {
        PlayerLevelDataList.DefineSavingUtility(savingUtility);
    }
}
