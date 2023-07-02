using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDefinition", menuName = "New Level")]
public class LevelDefinition : ScriptableObject
{
    [SerializeField] string levelName;

    // Generated
    [SerializeField] Vector2Int playerStart;
    [SerializeField] List<TileDefinition> tiles;
    [SerializeField] List<Vector2Int> goals;

    [SerializeField] List<TileTypeRequirement> unlockRequirements;
}
