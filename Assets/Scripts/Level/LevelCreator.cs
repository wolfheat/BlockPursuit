using System;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public enum TileType{O,L,J,I,S,T,Z}
public enum GameTileType {Hole,Walkable,Stone}

[Serializable]
public struct TileTypeRequirement
{
    public TileType tileType;
    public int amount;
}

public class LevelCreator : MonoBehaviour
{
    private UIController UI;

    [SerializeField] GameObject levelHolder;

    [SerializeField] GameTile holePrefab;
    [SerializeField] GameTile floorPrefab;
    [SerializeField] GameTile wallPrefab;
    [SerializeField] GameObject fillAreaPrefab;

    [SerializeField] GameObject lockPrefab;
    [SerializeField] GameObject bucketPrefab;

    [SerializeField] List<Section> sectionPrefabs = new List<Section>();

    public Section paintSection;
    public Section heldSection;
    
    public static GameTile[,,] TileLevel { get; set; }

    public List<Section> sections = new List<Section>();
    public List<GameObject> fillAreas = new List<GameObject>();
    public List<Vector2Int> fillAreasPositions = new List<Vector2Int>();

    public static int TileSize = 1;
    public static int LevelWidth = 20;
    public static int LevelHeight = 20;

    bool haveWalls = false;

    // Start is called before the first frame update
    void Start()
    {
        TileLevel = new GameTile[LevelWidth, LevelHeight,2];
        UI = FindObjectOfType<UIController>();
    }
    
    public static bool IsEmptyAndCanTakeObject(Vector2Int pos)
    {
        if(TileLevel == null ) return false;
        if (pos.x < 0|| pos.y < 0 || pos.x >= TileLevel.GetLength(0) || pos.y >= TileLevel.GetLength(1)) 
            return false;
        if(TileLevel[pos.x, pos.y,1] == null) return true;
        return false;
    }
    
    public static bool IsWalkable(Vector2Int pos)
    {
        if(TileLevel == null ) return false;
        if (pos.x < 0 || pos.y < 0 || pos.x >= TileLevel.GetLength(0) || pos.y >= TileLevel.GetLength(1)) 
            return false;
        if(TileLevel[pos.x, pos.y, 1] == null) return false;
        return (TileLevel[pos.x, pos.y, 1].walkable) ? true : false;
    }
    
    public void LoadNextLevel()
    {         
        LoadLevel(GameSettings.CurrentLevel);
        GameSettings.IsPaused = false;
        GameSettings.CurrentGameState = GameState.RunGame;
        FindObjectOfType<PlayerController>().ShowPlayer();
    }

    public void LoadLevel(int level)
    {
        if(!haveWalls) CreateWalls();


        Debug.Log("Loading Level "+level);
        if(level == 4)
        {
            CreateFillArea(new Vector2Int(6, 6), new Vector2Int(10, 11));

            CreateSectionByTiletype(TileType.L, new Vector2Int(4, 5), 0);
            CreateSectionByTiletype(TileType.L, new Vector2Int(5, 4), 0);
            CreateSectionByTiletype(TileType.I, new Vector2Int(9, 6), 0);
            CreateSectionByTiletype(TileType.O, new Vector2Int(7, 6), 0);
            CreateSectionByTiletype(TileType.T, new Vector2Int(6, 9), 0);
            CreateSectionByTiletype(TileType.O, new Vector2Int(4, 8), 0);
            FindObjectOfType<PlayerController>().SetInitPosition(new Vector2Int(5,4));
        }        
        else if(level == 1)
        {
            CreateFillArea(new Vector2Int(6, 6), new Vector2Int(8, 8));

            CreateSectionByTiletype(TileType.O, new Vector2Int(7, 6), 0);
            CreateSectionByTiletype(TileType.T, new Vector2Int(6, 9), 0);
            CreateSectionByTiletype(TileType.O, new Vector2Int(4, 8), 0);
            FindObjectOfType<PlayerController>().SetInitPosition(new Vector2Int(7,6));
        }
        else
        {
            CreateFillArea(new Vector2Int(6, 6), new Vector2Int(8, 8));
            CreateSectionByTiletype(TileType.O, new Vector2Int(7, 6), 0);
            CreateSectionByTiletype(TileType.O, new Vector2Int(9, 7), 0);
            FindObjectOfType<PlayerController>().SetInitPosition(new Vector2Int(7, 6));
        }
    }

    private void CreateWalls()
    {
        for (int i = -1; i <= LevelWidth; i++)
        {
            for (int j = -1; j <= LevelHeight; j++)
            {
                if (i < 0 || j < 0 || i == LevelWidth || j == LevelHeight)
                {
                    CreateWall(new Vector3Int(i,j,0));
                }
            }
        }
        haveWalls = true;
    }

    private void CreateFillArea(Vector2Int from, Vector2Int to)
    {
        for (int i = from.x; i < to.x ; i++)
        {
            for (int j = from.y; j < to.y; j++)
            {
                CreateFillAreaTile(new Vector2Int(i,j));
            }
        }
    }

    private void CreateFillAreaTile(Vector2Int pos)
    {
        GameObject newFillAreaTile = Instantiate(fillAreaPrefab, levelHolder.transform,false);
        newFillAreaTile.transform.position = new Vector3(pos.x,pos.y,0.5f);
        fillAreas.Add(newFillAreaTile);
        fillAreasPositions.Add(pos);
    }
    
    private void CreatePremadeSection(Section section, Vector2Int pos, int rotationIndex)
    {
        Section newSection = Instantiate(section, levelHolder.transform,false);
        newSection.SetLevelHolder(levelHolder);
        newSection.PlaceAt(pos, rotationIndex);
        PlaceSectionInArray(newSection);
        sections.Add(newSection);

    }

    public void CreateSectionByTiletype(TileType sectionType, Vector2Int pos, int rotationIndex=0)
    {
        Debug.Log("Creating Section By Tiletype" + sectionType+" id: "+ (int)sectionType+" size of array: "+ sectionPrefabs.Count);  

        CreatePremadeSection(sectionPrefabs[(int)sectionType],pos,rotationIndex);
    }

    private bool PossiblePickup(Vector2Int from, Vector2Int target)
    {
        if (target.x < 0 || target.y < 0 || target.x >= TileLevel.GetLength(0) || target.y >= TileLevel.GetLength(1)) return false;
        if (TileLevel[target.x,target.y, 1] == null) return false;
        if (TileLevel[from.x,from.y, 1].section != TileLevel[target.x,target.y, 1].section) return true;
        return false;
    }
    
    public void PickupSectionAt(Vector2Int from, Vector2Int target, int rotationIndex)
    {
        //Debug.Log("Request to pick up from " + from + " target:" + target + " Possible = "+ PossiblePickup(from, target)+" in direction: "+rotationIndex);
        if(!PossiblePickup(from,target)) return;

        GameTile pickedTile = TileLevel[target.x, target.y, 1];
        heldSection = pickedTile.section;

        heldSection.SetPivotPosition(pickedTile);
        //Debug.Log("Set Rotation to "+rotationIndex);
        heldSection.PickedUpAtRotationIndex(rotationIndex);

        foreach (GameTile tile in heldSection.GameTiles)
        {
            TileLevel[tile.Pos.x, tile.Pos.y, 1] = null;
        }

        heldSection.Held(true, true);
    }
    
    public void PlacePaintSectionIfPossibleAt(Vector2Int pos,int rotationIndex)
    {
        if (paintSection == null) return;

        if(!SectionPlacable(paintSection)) return;

        paintSection.Held(false, false);
        paintSection.PlaceAt(pos,rotationIndex);

        sections.Add(paintSection);

        foreach (GameTile tile in paintSection.GameTiles)
        {
            TileLevel[tile.Pos.x, tile.Pos.y, 1] = tile;
        }
        // Release tile
        paintSection = null;
    }
    
    public void PlaceHeldSectionAt(Vector2Int pos,int rotationIndex)
    {
        if(!SectionPlacable(heldSection)) return;

        heldSection.Held(false, false);

        heldSection.PlaceAt(pos,rotationIndex);
        foreach (GameTile tile in heldSection.GameTiles)
        {
            TileLevel[tile.Pos.x, tile.Pos.y, 1] = tile;
        }
        heldSection = null;

        GameSettings.MoveCounter++;

        bool isComplete = CheckIfComplete();
        if (isComplete)
        {
            FindObjectOfType<PlayerController>().HidePlayer();
            GameSettings.IsPaused = true;

            //Next level
            GameSettings.CurrentLevel++;
            GameSettings.StoredAction = GameAction.ShowLevelComplete;
            FindObjectOfType<TransitionScreen>().StartTransition();
        }
    }
    

    public void ClearLevel()
    {
        for (int i = sections.Count-1; i >= 0; i--)
        {
            sections[i].DestroyParts();
            Destroy(sections[i].gameObject);
        }
        sections.Clear();   
        for (int i = fillAreas.Count-1; i >= 0; i--)
        {
            Destroy(fillAreas[i].gameObject);
        }
        fillAreas.Clear();
        fillAreasPositions.Clear();


    }

    private bool CheckIfComplete()
    {
        int counter = 0;
        foreach (Vector2Int pos in fillAreasPositions)
        {
            if (TileLevel[pos.x,pos.y,1]!=null) counter++;
        }
        Debug.Log("Checking if complete: ("+counter+"/"+fillAreasPositions.Count+")");
        if(counter == fillAreasPositions.Count) return true; 
        else return false;
    }

    private void PlaceSectionInArray(Section section)
    {
        foreach (GameTile tile in section.GameTiles)
        {
            TileLevel[tile.Pos.x, tile.Pos.y, 1] = tile;
        }
    }

    private void CreateWall(Vector3Int pos)
    {
        GameTile newWall = Instantiate(wallPrefab, levelHolder.transform, true);
        newWall.transform.position = pos;
    }

    internal void ChangeTool(int type)
    {
        if(paintSection != null)
        {
            paintSection.DestroyParts();
            Destroy(paintSection.gameObject);
        }

        paintSection = Instantiate(sectionPrefabs[type],levelHolder.transform);
        paintSection.SetLevelHolder(levelHolder);
    }
    
    internal void UpdatePaint(int type, Vector2Int target, int rotationIndex)
    {
        if(paintSection == null)
        {
            paintSection = Instantiate(sectionPrefabs[type],levelHolder.transform,false);
        }
        paintSection.SetLevelHolder(levelHolder);
        paintSection.SetVisualTo(target, rotationIndex);

        if(SectionPlacable(paintSection)) paintSection.Held(true, true);
        else paintSection.Held(true, false);

    }
    
    internal void UpdateHeld(Vector2Int target, int rotationIndex)
    {
        if(heldSection != null)
        {
            //Debug.Log("Update Held section");
            heldSection.SetVisualTo(target, rotationIndex);

            if(SectionPlacable(heldSection)) heldSection.Held(true, true);
            else heldSection.Held(true, false);
        }
    }

    private bool SectionPlacable(Section section)
    {
        foreach (GameTile tile in section.GameTiles)
        {
            if (!IsEmptyAndCanTakeObject(tile.Pos)) return false;
        }
        return true;    
    }

    internal void HidePaint()
    {
        paintSection?.Used(false);
    }
}
