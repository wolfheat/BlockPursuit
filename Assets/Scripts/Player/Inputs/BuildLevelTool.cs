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

        Inputs.Instance.Controls.Main.ScrollUp.performed += RotateLeft;
        Inputs.Instance.Controls.Main.ScrollDown.performed += RotateRight;
        Inputs.Instance.Controls.Main.LeftClick.performed += Click;
        Inputs.Instance.Controls.Main.RightClick.performed += RightClick;
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
            levelCreator.UpdatePaint(activeTool, GetMouseTileIndex(),rotation);
        }
        else
        {
            levelCreator.RemovePaintIfPresent();
        }
    }

    public void RequestPrevious()
    {
        Debug.Log("Request previous level");
        levelCreator.LoadPrevLevel();
    }
    public void RequestNext()
    {
        Debug.Log("Request next level");
        levelCreator.LoadNextLevel();
    }
    private void RightClick(InputAction.CallbackContext context)
    {
        NextTool();
    }
    private void Click(InputAction.CallbackContext context)
    {
        if (Inputs.Instance.Controls.Main.Shift.IsPressed())
        {
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

    private void NextTool()
    {
        Debug.Log("Next Tool");
        activeTool = (activeTool + 1) % tileTypes.Count;
        UpdateShownTool();
    }


    private Vector2Int GetMouseTileIndex()
    {
        Vector3 clickScreenPosition = Mouse.current.position.ReadValue();
        clickScreenPosition.z = -Camera.main.transform.position.z;
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(clickScreenPosition);

        return new Vector2Int(Mathf.RoundToInt(clickPosition.x), Mathf.RoundToInt(clickPosition.y));
    }
}
