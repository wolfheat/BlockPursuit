using System;
using UnityEngine;

[Serializable]
public struct TilePlacementDefinition
{
    public TileType type;
    public Vector2Int position;
    public int rotation;

    public TilePlacementDefinition(TileType t, Vector2Int p, int r)
    {
        type = t;
        position = p;
        rotation = r;
    }
}
