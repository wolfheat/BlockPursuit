using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct PlayerLevelDefinition
{
    public int levelID;
    public int bestSteps;
    public int bestMoves;
    public float bestTime;
}


[CreateAssetMenu(fileName = "PlayerLevelsDefinition", menuName = "New Player LevelDefinition")]
public class PlayerLevelsDefinition : ScriptableObject
{
    //public List<PlayerLevelDefinition> levelsList = new List<PlayerLevelDefinition>();

    public List<PlayerLevelDefinition> LevelsList { get; private set; }

    public void SetLevelDefinitions(List<PlayerLevelDefinition> levels)
    {
        LevelsList = levels;
    }
    public PlayerLevelDefinition GetDefinitionForID(int id)
    {
        foreach (PlayerLevelDefinition lvl in LevelsList)
        {
            if(lvl.levelID == id)
                return lvl;
        }
        return new PlayerLevelDefinition() { levelID = -1};
    }
}
