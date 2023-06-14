using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] GameObject levelHolder;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject wallPrefab;

    public static int[,] Level { get; set; }

    public static int TileSize = 1;
    public static int LevelWidth = 10;
    public static int LevelHeight = 17;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();    
    }

    private void CreateLevel()
    {
        Level = new int[LevelWidth,LevelHeight];
        // 9 x 16
        for (int i = 0; i < LevelWidth; i++)
        {
            for (int j = 0; j < LevelHeight; j++)
            {   
                CreateFloor(new Vector3Int(i*TileSize,j* TileSize, 0));

                if (i == 0 || i == 9 || j == 0 || j == 16)
                {
                    CreateWall(new Vector3(i * TileSize+0.5f, j * TileSize + 0.5f, -0.5f));
                    Level[i, j] = 1;
                }
                else
                {
                    Level[i, j] = 0;
                }
            }
        }
    }

    private void CreateWall(Vector3 pos)
    {
        GameObject newWall = Instantiate(wallPrefab, levelHolder.transform, true);
        newWall.transform.position = pos;
    }

    private void CreateFloor(Vector3Int pos)
    {
        GameObject newFloor = Instantiate(floorPrefab, levelHolder.transform, true);
        newFloor.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal static bool IsWalkable(Vector2Int pos)
    {
        Debug.Log("Checking if tile: "+pos+" is walkable = "+ (Level[pos.x, pos.y] == 0));
        return Level[pos.x, pos.y] == 0;
    }
}
