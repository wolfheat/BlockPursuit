using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum SectionType{I,O,L}
public enum GameTileType {Hole,Walkable,Stone}


public class LevelCreator : MonoBehaviour
{

    [SerializeField] GameObject levelHolder;

    [SerializeField] GameTile holePrefab;
    [SerializeField] GameTile floorPrefab;
    [SerializeField] GameTile wallPrefab;

    [SerializeField] GameObject lockPrefab;
    [SerializeField] GameObject bucketPrefab;
    [SerializeField] Section sectionPrefab;
    [SerializeField] Section TshapePrefab;
    
    public static GameTile[,] TileLevel { get; set; }

    public List<Section> sections = new List<Section>();

    public static int TileSize = 1;
    public static int LevelWidth = 11;
    public static int LevelHeight = 11;

    // Start is called before the first frame update
    void Start()
    {
        TileLevel = new GameTile[LevelWidth, LevelHeight];
        CreateLevel();      
    }

    public static bool IsWalkable(Vector2Int pos)
    {
        if(TileLevel == null ) return false;
        if (pos.x > TileLevel.GetLength(0) || pos.y > TileLevel.GetLength(1)) 
            return false;
        if(TileLevel[pos.x, pos.y] == null) return false;
        return (TileLevel[pos.x, pos.y].walkable) ? true : false;
    }
    
    private void CreateLevel()
    {        
        CreateSection(SectionType.I, new Vector2Int(5, 3));
        CreateSection(SectionType.O, new Vector2Int(3, 3));
        CreateSection(SectionType.L, new Vector2Int(1, 2));
        CreatePremadeSection(TshapePrefab, new Vector2Int(1, 6));
        GenerateLevel();

    }

    private void CreatePremadeSection(Section section, Vector2Int pos)
    {
        Section newSection = Instantiate(section, levelHolder.transform,false);
        newSection.DefinePremade();
        newSection.PlaceAt(pos);
        sections.Add(newSection);

    }
    
    private void CreateSection(SectionType sectionType, Vector2Int pos)
    {
        Debug.Log("Creating new Section " + sectionType);
        Section newSection = Instantiate(sectionPrefab, levelHolder.transform,false);
        newSection.CreateAsSectionType(sectionType);
        newSection.PlaceAt(pos);
        sections.Add(newSection);

    }

    private void GenerateLevel()
    {
        foreach(var section in sections)
        {
            foreach (GameTile tile in section.GameTiles)
            {
                TileLevel[tile.Pos.x, tile.Pos.y] = tile;
            }
        }
    }

    private void CreateBucket(Vector3Int pos)
    {
        GameObject newBucket = Instantiate(bucketPrefab, levelHolder.transform, true);
        newBucket.transform.position = pos;
    }
    private void CreateLock(Vector3Int pos)
    {
        GameObject newLock = Instantiate(lockPrefab, levelHolder.transform, true);
        newLock.transform.position = pos;
    }
    
    private void CreateWall(Vector3Int pos)
    {
        GameTile newWall = Instantiate(wallPrefab, levelHolder.transform, true);
        newWall.transform.position = pos;
    }

    private void CreateFloor(Vector3Int pos)
    {
        GameTile newFloor = Instantiate(floorPrefab, levelHolder.transform, true);
        newFloor.transform.position = pos;
    }
    
    private void CreateHole(Vector3Int pos)
    {
        return;
    }

}
