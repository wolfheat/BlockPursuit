using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildLevelTool : MonoBehaviour
{
    [SerializeField] Image toolImage;
    [SerializeField] List<TileDefinition> tileTypes;
    LevelCreator levelCreator;
    private int activeTool = 0;
    private int rotation = 0;

    private void Awake()
    {
        levelCreator = FindObjectOfType<LevelCreator>();
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
            // Figure out position to place tile
            Vector3 clickScreenPosition = Mouse.current.position.ReadValue();
            clickScreenPosition.z = -Camera.main.transform.position.z;

            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(clickScreenPosition);

            Vector2Int clickIndex = new Vector2Int(Mathf.RoundToInt(clickPosition.x), Mathf.RoundToInt(clickPosition.y));
            levelCreator.UpdatePaint(activeTool, clickIndex,rotation);
        }
        else
        {
            levelCreator.HidePaint();
        }
    }

    private void RightClick(InputAction.CallbackContext context)
    {
        NextTool();
    }
    private void Click(InputAction.CallbackContext context)
    {
        if (Inputs.Instance.Controls.Main.Shift.IsPressed())
        {
            Debug.Log("Click");

            // Figure out position to place tile
            Vector3 clickScreenPosition = Mouse.current.position.ReadValue();
            clickScreenPosition.z = -Camera.main.transform.position.z;

            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(clickScreenPosition);

            Vector2Int clickIndex = new Vector2Int(Mathf.RoundToInt(clickPosition.x), Mathf.RoundToInt(clickPosition.y));

            Debug.Log("Click position: "+ clickIndex + " click point is "+clickScreenPosition);

            levelCreator.PlacePaintSectionIfPossibleAt(clickIndex,rotation);

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
        levelCreator.ChangeTool(activeTool);
    }

    private void NextTool()
    {
        Debug.Log("Next Tool");
        activeTool = (activeTool + 1) % tileTypes.Count;
        UpdateShownTool();
    }

}
