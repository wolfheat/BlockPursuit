using System;

public class PlayerGameData
{
    // Players Inventory
    public int Tiles { get; set; } = 0;
    public int Coins { get; set; } = 100;
    // Player Levels Data
    public PlayerLevelDataList PlayerLevelDataList { get; private set; }

    public static Action InventoryUpdate;

    public PlayerGameData()
    {
        Tiles = 0;
        Coins = 100;
        PlayerLevelDataList = new PlayerLevelDataList();
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
}
