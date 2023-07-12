using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public enum TileType{O,L,J,I,S,T,Z, Goal}
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

    [SerializeField] GameObject goalHolder;
    [SerializeField] GameObject tileHolder;
    [SerializeField] GameObject toolHolder;
    [SerializeField] GameObject wallHolder;

    [SerializeField] GameTile holePrefab;
    [SerializeField] GameTile floorPrefab;
    [SerializeField] GameTile wallPrefab;
    [SerializeField] GameObject fillAreaPrefab;

    [SerializeField] GameObject lockPrefab;
    [SerializeField] GameObject bucketPrefab;

    [SerializeField] List<Section> sectionPrefabs = new List<Section>();

    LevelComplete levelComplete;
    

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
        levelComplete = FindObjectOfType<LevelComplete>();
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
    
    public void LoadPrevLevel()
    {
        /*
        GameSettings.CurrentLevel--;
        if (GameSettings.CurrentLevel < 0) GameSettings.CurrentLevel = 0;

        LoadLevelByDefinition(GameSettings.CurrentLevel, GameSettings.CurrentDifficultLevel);

        GameSettings.IsPaused = false;
        GameSettings.CurrentGameState = GameState.RunGame;
        FindObjectOfType<PlayerController>().ShowPlayer();
        UI.UpdateStats();
        */
    }
    
    public void RestartLevel()
    {
        Debug.Log("Restarting level, currently just loading same again");
        LoadSelectedLevel();
    }
    
    public void LoadSelectedLevel()
    {   
        LoadLevelByDefinition(GameSettings.CurrentLevelDefinition);

        GameSettings.IsPaused = false;
        GameSettings.CurrentGameState = GameState.RunGame;
        FindObjectOfType<PlayerController>().ShowPlayer();
        UI.UpdateStats();
    }
    
    public void LoadNextLevel()
    {
        /*
        if (GameSettings.CurrentLevel >= Levels.LevelDefinitions[GameSettings.CurrentDifficultLevel].Count) return;

        GameSettings.CurrentLevel++;
        
        LoadLevelByDefinition(GameSettings.CurrentLevel, GameSettings.CurrentDifficultLevel);

        GameSettings.IsPaused = false;
        GameSettings.CurrentGameState = GameState.RunGame;
        FindObjectOfType<PlayerController>().ShowPlayer();
        UI.UpdateStats();
        */
    }

    public void LoadLevelByDefinition(LevelDefinition level)
    {
        if (!haveWalls) CreateWalls();

        ClearLevel();
        LoadLevelDefinition(level);

    }

    private void LoadLevelDefinition(LevelDefinition level)
    {
        Debug.Log("Loading Level: "+level.name);
        foreach (Vector2Int pos in level.goals)
            CreateGoalTile(new Vector2Int(pos.x, pos.y));
        foreach (TilePlacementDefinition tile in level.tiles)
            CreateSectionByTiletype(tile.type, tile.position, tile.rotation);
        FindObjectOfType<PlayerController>().SetInitPosition(level.playerStart);
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
                CreateGoalTile(new Vector2Int(i,j));
            }
        }
    }

    public List<Vector2Int> GetFillAreaPositions()
    {
        List<Vector2Int> test = new List<Vector2Int>();
        return fillAreas.Select(a => new Vector2Int(Mathf.RoundToInt(a.transform.position.x), Mathf.RoundToInt(a.transform.position.y))).ToList();
    }

    public List<TilePlacementDefinition> GetTilePlacementList()
    {
        return sections.Select(a => new TilePlacementDefinition(a.TileType,new Vector2Int(Mathf.RoundToInt(a.OriginalPivot.transform.position.x), Mathf.RoundToInt(a.OriginalPivot.transform.position.y)),a.Rotation)).ToList();
    }

    public void RemoveGoalTile(Vector2Int pos)
    {
        GameObject goalExists = GoalExistsAtPos(pos);
        if (goalExists != null)
        {
            fillAreas.Remove(goalExists);
            fillAreasPositions.Remove(pos);
            Destroy(goalExists);
        }        
    }
    
    private GameObject GoalExistsAtPos(Vector2Int pos)
    {
        foreach (GameObject goal in fillAreas)
        {
            Vector2Int goalPosition = new Vector2Int(Mathf.RoundToInt(goal.transform.position.x), Mathf.RoundToInt(goal.transform.position.y));
            if (pos == goalPosition)
            {
                return goal;
            }
        }
        return null;
    }

    private void CreateGoalTile(Vector2Int pos)
    {
        if (GoalExistsAtPos(pos)) return;

        GameObject newFillAreaTile = Instantiate(fillAreaPrefab, goalHolder.transform,false);
        newFillAreaTile.transform.position = new Vector3(pos.x,pos.y,0.5f);
        fillAreas.Add(newFillAreaTile);
        fillAreasPositions.Add(pos);
    }
    
    private void CreatePremadeSection(Section section, Vector2Int pos, int rotationIndex,TileType t)
    {
        Section newSection = Instantiate(section, tileHolder.transform,false);
        newSection.SetHolder(tileHolder);
        newSection.PlaceAt(pos, rotationIndex);
        newSection.TileType = t;
        PlaceSectionInArray(newSection);
        sections.Add(newSection);
    }

    public void CreateSectionByTiletype(TileType sectionType, Vector2Int pos, int rotationIndex=0)
    {
        Debug.Log("Creating Section By Tiletype" + sectionType+" id: "+ (int)sectionType+" size of array: "+ sectionPrefabs.Count);  

        CreatePremadeSection(sectionPrefabs[(int)sectionType],pos,rotationIndex,sectionType);
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

        if (paintSection.TileType == TileType.Goal)
        {
            CreateGoalTile(pos);
            return;
        } 

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

            int coinGain = 55;
            int tileGain = 1;

            // Determin reward
            SavingUtility.playerGameData.AddCoins(coinGain);
            SavingUtility.playerGameData.AddTiles(tileGain);

            levelComplete.UpdateStats(coinGain,tileGain);

            //Next level
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
        GameTile newWall = Instantiate(wallPrefab, wallHolder.transform, true);
        newWall.transform.position = pos;
    }

    internal void DestroyCurrentTool(int type)
    {
        if(paintSection != null)
        {
            paintSection.DestroyParts();
            Destroy(paintSection.gameObject);
        }
    }
    
    internal void UpdatePaint(int type, Vector2Int target, int rotationIndex)
    {
        if(paintSection == null)
            CreateNewPaintTool(type);
            
        paintSection.SetVisualTo(target, rotationIndex);

        if(paintSection.TileType == TileType.Goal) paintSection.Held(true, true);
        else if (SectionPlacable(paintSection)) paintSection.Held(true, true);
        else paintSection.Held(true, false);

    }

    private void CreateNewPaintTool(int type)
    {
        paintSection = Instantiate(sectionPrefabs[type], toolHolder.transform, false);
        paintSection.SetHolder(toolHolder);
        paintSection.TileType = (TileType)type;
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

    internal void RemovePaintIfPresent()
    {
        if(paintSection != null)
        {
            paintSection.DestroyParts();
            paintSection = null;
        }
    }

    internal void RemoveTileAtPosition(Vector2Int pos)
    {
        if (TileLevel[pos.x, pos.y,1] == null) return;

        Section section = TileLevel[pos.x, pos.y, 1].section;
        Debug.Log("Remove section: "+section);

        foreach (GameTile tile in section.GameTiles)
        {
            TileLevel[tile.Pos.x, tile.Pos.y, 1] = null;
        }
        section.DestroyParts();
        sections.Remove(section);   
        Destroy(section.gameObject);
    }
}
