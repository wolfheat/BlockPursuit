using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Players Stash
    //public Dictionary<int, int> Tiles { get; set; } = new Dictionary<int, int>();


    // Players Inventory
    public int Tiles { get; set; } = 0;
    public int Coins { get; set; } = 100;
    public PlayerLevelsDefinition PlayerLevelsDefinition { get; private set; }

    public static Action InventoryUdate;

    public void DefineEmptyDefinition()
    {
        PlayerLevelsDefinition = new PlayerLevelsDefinition(); 
        PlayerLevelsDefinition.SetLevelDefinitions(new List<PlayerLevelDefinition>());
    }
    public void DefinePlayerLevelsDefinition(PlayerLevelsDefinition def)
    {
        PlayerLevelsDefinition = def;
    }
    public void DefineInventory(PlayerInventory def)
    {
        Tiles = def.Tiles;
        Coins = def.Coins;
        PlayerLevelsDefinition = def.PlayerLevelsDefinition;
    }

    public void AddCoins(int amt)
    {
        Coins += amt;
        InventoryUdate.Invoke();
    }

    public bool RemoveCoins(int amt)
    {
        if(Coins < amt) return false;
        Coins -= amt;
        InventoryUdate.Invoke();
        return true;
    }
    public void AddTiles(int amt)
    {
        Tiles += amt;
        InventoryUdate.Invoke();
    }

    public bool RemoveTiles(int amt)
    {
        if(Tiles < amt) return false;
        Tiles -= amt;
        InventoryUdate.Invoke();
        return true;
    }
}
