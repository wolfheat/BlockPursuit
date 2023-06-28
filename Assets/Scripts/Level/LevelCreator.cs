using System;
using System.Collections.Generic;
using UnityEngine;

public enum SectionType{I,O,L}
public enum GameTileType {Hole,Walkable,Stone}


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
    [SerializeField] Section sectionPrefab;
    [SerializeField] Section TshapePrefab;
    [SerializeField] Section OshapePrefab;
    [SerializeField] Section IshapePrefab;
    [SerializeField] Section LshapePrefab;

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
        //CreateLevel();      
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
    }

    public void LoadLevel(int level)
    {
        if(!haveWalls) CreateWalls();


        Debug.Log("Loading Level "+level);
        if(level == 0)
        {
            CreateFillArea(new Vector2Int(6, 6), new Vector2Int(10, 11));

            CreatePremadeSection(LshapePrefab, new Vector2Int(5, 5), 0);
            CreatePremadeSection(LshapePrefab, new Vector2Int(4, 4), 0);
            CreatePremadeSection(IshapePrefab, new Vector2Int(9, 6), 0);
            CreatePremadeSection(OshapePrefab, new Vector2Int(7, 6), 0);
            CreatePremadeSection(TshapePrefab, new Vector2Int(6, 9), 0);
            CreatePremadeSection(OshapePrefab, new Vector2Int(4, 8), 0);
            PlaceAllSections();

            FindObjectOfType<PlayerController>().SetInitPosition(new Vector2Int(5,4));
        }        
        else if(level == 1)
        {
            CreateFillArea(new Vector2Int(6, 6), new Vector2Int(8, 8));

            CreatePremadeSection(OshapePrefab, new Vector2Int(7, 6), 0);
            CreatePremadeSection(TshapePrefab, new Vector2Int(6, 9), 0);
            CreatePremadeSection(OshapePrefab, new Vector2Int(4, 8), 0);
            PlaceAllSections();


            FindObjectOfType<PlayerController>().SetInitPosition(new Vector2Int(7,6));
        }
        else
        {
            CreateFillArea(new Vector2Int(6, 6), new Vector2Int(8, 8));
            CreatePremadeSection(OshapePrefab, new Vector2Int(7, 6), 0);
            CreatePremadeSection(OshapePrefab, new Vector2Int(9, 7), 0);
            PlaceAllSections();
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
        sections.Add(newSection);

    }
    
    private void CreateSection(SectionType sectionType, Vector2Int pos, int rotationIndex)
    {
        Debug.Log("Creating new Section " + sectionType);
        Section newSection = Instantiate(sectionPrefab);
        newSection.SetLevelHolder(levelHolder);
        newSection.CreateAsSectionType(sectionType);
        newSection.PlaceAt(pos, rotationIndex);
        sections.Add(newSection);

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
    
    public void PlaceHeldSectionAt(Vector2Int pos,int rotationIndex)
    {
        if(!HeldPlacable()) return;

        heldSection.Held(false, false);

        heldSection.PlaceAt(pos,rotationIndex);
        foreach (GameTile tile in heldSection.GameTiles)
        {
            TileLevel[tile.Pos.x, tile.Pos.y, 1] = tile;
        }
        heldSection = null;

        bool isComplete = CheckIfComplete();
        if (isComplete)
        {
            GameSettings.IsPaused = true;
            ClearLevel();

            //Next level
            GameSettings.CurrentLevel++;

            UI.ShowLevelComplete();
        }
    }

    private void ClearLevel()
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

    private void PlaceAllSections()
    {
        foreach(var section in sections)
        {
            foreach (GameTile tile in section.GameTiles)
            {
                //Debug.Log("TileLEvel pos: "+tile.Pos);
                TileLevel[tile.Pos.x, tile.Pos.y, 1] = tile;
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

    internal void UpdateHeld(Vector2Int target, int rotationIndex)
    {
        if(heldSection != null)
        {
            //Debug.Log("Update Held section");
            heldSection.SetVisualTo(target, rotationIndex);

            if(HeldPlacable()) heldSection.Held(true, true);
            else heldSection.Held(true, false);
        }
    }

    private bool HeldPlacable()
    {
        //Debug.Log("Held placable check");
        foreach (GameTile tile in heldSection.GameTiles)
        {
            //Debug.Log("Checking if tile placable at: "+tile.Pos+" :"+ IsEmptyAndCanTakeObject(tile.Pos));
            if (!IsEmptyAndCanTakeObject(tile.Pos)) return false;
        }
        return true;    
    }
}
