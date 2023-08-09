using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildLevelTool : MonoBehaviour
{
    [SerializeField] Image toolImage;
    [SerializeField] List<TileDefinition> tileTypes;
    LevelCreator levelCreator;
    private int GoalIndex;
    private int activeTool = 0;
    private int rotation = 0;

    private void Awake()
    {
        levelCreator = FindObjectOfType<LevelCreator>();
        GoalIndex = tileTypes.Count-1;
    }

    private void OnEnable()
    {
        Debug.Log("ONENABLE!!!!");
        Inputs.Instance.Controls.Main.ScrollUp.performed += RotateLeft;
        Inputs.Instance.Controls.Main.ScrollDown.performed += RotateRight;
        Inputs.Instance.Controls.Main.LeftClick.performed += Click;
        Inputs.Instance.Controls.Main.RightClick.performed += RightClick;
        Inputs.Instance.Controls.Main.Q.performed += QTest;
        UpdateShownTool();
    }
    
    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ScrollUp.performed -= RotateLeft;
        Inputs.Instance.Controls.Main.ScrollDown.performed -= RotateRight;
        Inputs.Instance.Controls.Main.LeftClick.performed -= Click;
        Inputs.Instance.Controls.Main.RightClick.performed -= RightClick;
    }


    private void Update()
    {
        if (Inputs.Instance.Controls.Main.Shift.IsPressed())
        {
            Debug.Log("Mouse Tile Index is: "+ GetMouseTileIndex());
            levelCreator.UpdatePaint(activeTool, GetMouseTileIndex(),rotation);
        }
        else
        {
            levelCreator.RemovePaintIfPresent();
        }
    }

    private void QTest(InputAction.CallbackContext context)
    {
        Debug.Log("Q TEST");
        Debug.Log("Cut This Piece and make it the active tool to relocate");
        // Check what piece the cursor is over
        TileType removedTileType = levelCreator.RemoveTileAtPosition(GetMouseTileIndex());
        if(removedTileType != TileType.none){
            SetTool(removedTileType);
        }

    }
    private void RightClick(InputAction.CallbackContext context)
    {
        Debug.Log("Tool Next");
        NextTool();
    }
    private void Click(InputAction.CallbackContext context)
    {
        Debug.Log("Click");
        if (Inputs.Instance.Controls.Main.Shift.IsPressed())
        {
            Debug.Log("PlacePaintSection if possible at "+ GetMouseTileIndex());
            levelCreator.PlacePaintSectionIfPossibleAt(GetMouseTileIndex(), rotation);
        }
        else if (Inputs.Instance.Controls.Main.LCtrl.IsPressed())
        {            
            if(activeTool == GoalIndex)
                levelCreator.RemoveGoalTile(GetMouseTileIndex());
            else            
                levelCreator.RemoveTileAtPosition(GetMouseTileIndex());
        }
    }
    
    private void RotateLeft(InputAction.CallbackContext context)
    {
        rotation = (4 + rotation - 1) % 4;
    }
    
    private void RotateRight(InputAction.CallbackContext context)
    {
        rotation = (rotation + 1) % 4;
    }

    private void UpdateShownTool()
    {        
        toolImage.sprite = tileTypes[activeTool].sprite;
        if(Inputs.Instance.Controls.Main.Shift.IsPressed())
            levelCreator.DestroyCurrentTool(activeTool);
    }

    private void SetTool(TileType type)
    {
        activeTool = (int)type;
        UpdateShownTool();
    }
    
    private void NextTool()
    {
        Debug.Log("Next Tool");
        activeTool = (activeTool + 1) % tileTypes.Count;
        UpdateShownTool();
    }


    private Vector2Int GetMouseTileIndex()
    {
        Vector3 clickScreenPosition = Mouse.current.position.ReadValue();
        Debug.Log("Mouse position: "+clickScreenPosition);
        clickScreenPosition.z = -Camera.main.transform.position.z;
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(clickScreenPosition);
        Debug.Log("Click position: "+clickPosition);

        return new Vector2Int(Mathf.RoundToInt(clickPosition.x), Mathf.RoundToInt(clickPosition.y));
    }
}
