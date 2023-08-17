using System;
using System.Collections.Generic;
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
    public PlayerLevelData(int ID)
    {
        levelID = ID;
        bestSteps = -1;
        bestMoves = -1;
        bestTime = -1;
    }
}

//[CreateAssetMenu(fileName = "PlayerLevelsDefinition", menuName = "New Player LevelDefinition")]
public class PlayerLevelDataList
{
    public List<PlayerLevelData> LevelsList { get; private set; }

    // Can this be removed?
    private SavingUtility savingUtility;

    public static Action PlayerLevelDataListUpdate;

    public PlayerLevelDataList()
    {
        LevelsList = new List<PlayerLevelData>();
        
    }
    
    public void DefineSavingUtility(SavingUtility s)
    {
        savingUtility = s;
    }

    public PlayerLevelData AddNewLevel(int ID)
    {
        Debug.Log("Adding new Level data to save file");

        if (LevelExists(ID,out int index))
        {
            Debug.LogWarning("Should not exist already");
            return LevelsList[index];
        }
        else
        {
            PlayerLevelData data = new PlayerLevelData(ID);
            LevelsList.Add(data);
            PlayerLevelDataListUpdate?.Invoke();
            return data;
        }
    }
    
    public PlayerLevelData AddOrUpdateLevel(PlayerLevelData data)
    {
        Debug.Log("Adding Level completion data to save file");

        if (LevelExists(data.levelID,out int index))
        {
            bool hasUpdatedData = false;
            PlayerLevelData foundLevel = LevelsList[index];
            //Update existing
            if (foundLevel.bestTime == -1 || data.bestTime < foundLevel.bestTime)
            {
                foundLevel.bestTime = data.bestTime;
                hasUpdatedData = true;
            }
            if (foundLevel.bestMoves == -1 || data.bestMoves < foundLevel.bestMoves)
            {
                foundLevel.bestMoves = data.bestMoves;
                hasUpdatedData = true;
            }
            if (foundLevel.bestSteps == -1 || data.bestSteps < foundLevel.bestSteps)
            {
                foundLevel.bestSteps = data.bestSteps;
                hasUpdatedData = true;
            }
            if (hasUpdatedData)
            {
                LevelsList[index] = foundLevel;
                PlayerLevelDataListUpdate?.Invoke();
            }
            // Place the modified level into the list
            return foundLevel;
        }
        else
        {
            LevelsList.Add(data);
            return data;
        }
    }

    public bool LevelExists(int levelID, out int index)
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
                //Debug.Log("Found Level definition for "+id);
                return lvl;
            }
        }
        //Debug.Log("Did not find Level definition for "+id);
        return new PlayerLevelData() { levelID = -1};
    }
}
