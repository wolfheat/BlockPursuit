using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    // Players Stash
    public Dictionary<int, int> Tiles { get; set; } = new Dictionary<int, int>();

    // Players Inventory
    public int Coins { get; set; } = 100;

    public PlayerInventory(int tileVariants)
    {


        for (int i = 0; i < tileVariants; i++)
        {
            Tiles[i] = 0;
        }
    }



}
