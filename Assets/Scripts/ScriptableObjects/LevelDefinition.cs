using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDefinition", menuName = "New Level")]
public class LevelDefinition : ScriptableObject
{
    public string levelName;
    public int levelID;
    public bool unlocked = false;
    public int LevelDiff { get; private set;}
    public int LevelIndex { get; private set;}

    // Generated
    public Vector2Int playerStart;
    public List<TilePlacementDefinition> tiles;
    public List<Vector2Int> goals;

    public int unlockRequirements;
    public int completeReward;


    public void SetLevelDefinition(string v, List<TilePlacementDefinition> s, List<Vector2Int> g, Vector2Int startPos)
    {
        levelID = GetInstanceID();
        levelName = v;
        tiles = s;
        goals = g;
        playerStart = startPos;
    }
    public void SetLevelIndex(int diff, int index)
    {
        LevelDiff = diff;
        LevelIndex = index;
    }
}
