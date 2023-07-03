using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDefinition", menuName = "New Level")]
public class LevelDefinition : ScriptableObject
{
    public string levelName;

    // Generated
    public Vector2Int playerStart;
    public List<TilePlacementDefinition> tiles;
    public List<Vector2Int> goals;

    public List<TileTypeRequirement> unlockRequirements;

    public void SetLevelDefinition(string v, List<TilePlacementDefinition> s, List<Vector2Int> g, Vector2Int startPos)
    {
        levelName = v;
        tiles = s;
        goals = g;
        playerStart = startPos;
    }
}
