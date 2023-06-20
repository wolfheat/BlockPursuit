using System;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    int[,] cubesID;
    [field: SerializeField] public List<GameTile> GameTiles { get; set; }
    //public List<Vector2Int> Occupying { get; private set; }
    int rotation = 0;
    [SerializeField] GameTile floorPrefab;


    internal void DefinePremade()
    {
        foreach (GameTile tile in GameTiles)
        {
            tile.Pos = new Vector2Int(Mathf.RoundToInt(tile.transform.localPosition.x), Mathf.RoundToInt(tile.transform.localPosition.y));
        }
    }

    internal void CreateAsSectionType(SectionType type)
    {
        switch (type)
        {
            case SectionType.I:
                cubesID = new int[4, 1] { { 1 }, { 1 }, { 1 }, { 1 }};
                break;
            case SectionType.O:
                cubesID = new int[2, 2] { { 1, 1 }, { 1, 1 }};
                break;
            case SectionType.L:
                cubesID = new int[3, 2] { { 1, 1 }, { 1, 0 }, { 1, 0}};
                break;
            default:
                break;
        }
        Create();
    }

    private void Create()
    {
        GameTiles = new List<GameTile>();
        for (int i = 0; i < cubesID.GetLength(0); i++) 
        {
            for (int j = 0; j < cubesID.GetLength(1); j++)
            {
                if (cubesID[i, j] == 0) continue;

                GameTile newFloor = Instantiate(floorPrefab,transform,false);
                newFloor.transform.localPosition = new Vector3(i, j, 0);
                newFloor.Pos = new Vector2Int(i,j);
                GameTiles.Add(newFloor);
            }
        }
    }

    public void PlaceAt(Vector2Int pos)
    {
        // Sets occupying index by approximating the box current position
        transform.localPosition = new Vector3(pos.x,pos.y,transform.position.z);
        foreach (GameTile tile in GameTiles)
            tile.Pos = new Vector2Int(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y));
    }
}
