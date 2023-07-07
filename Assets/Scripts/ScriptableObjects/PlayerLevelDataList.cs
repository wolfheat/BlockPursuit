using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct PlayerLevelData
{
    public int levelID;
    public int bestSteps;
    public int bestMoves;
    public int bestTime;
    public PlayerLevelData(int ID, int step, int move, int time)
    {
        levelID = ID;
        bestSteps = step;
        bestMoves = move;
        bestTime = time;
    }
}


//[CreateAssetMenu(fileName = "PlayerLevelsDefinition", menuName = "New Player LevelDefinition")]
public class PlayerLevelDataList
{
    public List<PlayerLevelData> LevelsList { get; private set; }

    public PlayerLevelDataList()
    {
        LevelsList = new List<PlayerLevelData>();
    }

    public PlayerLevelData AddOrUpdateLevel(PlayerLevelData data)
    {
        Debug.Log("Adding Level completion data to save file");

        if (LevelExists(data.levelID,out int index))
        {
            PlayerLevelData foundLevel = LevelsList[index];
            //Update existing
            if (data.bestTime < foundLevel.bestTime)
                foundLevel.bestTime = data.bestTime;
            if (data.bestMoves < foundLevel.bestMoves)
                foundLevel.bestMoves = data.bestMoves;
            if (data.bestSteps < foundLevel.bestSteps)
                foundLevel.bestSteps = data.bestSteps;

            // Place the modified level into the list
            LevelsList[index] = foundLevel; 
            return foundLevel;
        }
        else
        {
            LevelsList.Add(data);
            return data;
        }
    }

    private bool LevelExists(int levelID, out int index)
    {
        //Find data with this ID
        for (int i = 0; i<LevelsList.Count; i++)
        {
            if (levelID == LevelsList[i].levelID)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public void SetLevels(List<PlayerLevelData> levels)
    {
        LevelsList = levels;
    }
    public PlayerLevelData GetByID(int id)
    {
        foreach (PlayerLevelData lvl in LevelsList)
        {
            if(lvl.levelID == id)
            {
                Debug.Log("Found Level definition for "+id);
                return lvl;
            }
        }
        Debug.Log("Did not find Level definition for "+id);
        return new PlayerLevelData() { levelID = -1};
    }
}
