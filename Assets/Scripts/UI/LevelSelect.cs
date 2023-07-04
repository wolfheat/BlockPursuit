using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class LevelSelect : BasePanel
{
    [SerializeField] GameObject levelButtonHolder;
    [SerializeField] LevelButton levelButtonPrefab;

    [SerializeField] UIController UIController;

    private int selectedLevel = 0;

    private List<LevelButton> buttonList = new List<LevelButton>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateButtonLevels();
    }

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.Move.performed += MoveInLevelSelect;
        Inputs.Instance.Controls.Main.Interact.performed += PerformSelected;
        Inputs.Instance.Controls.Main.ESC.performed += RequestGoToMainMenu;
    }
    
    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.Move.performed -= MoveInLevelSelect;
        Inputs.Instance.Controls.Main.Interact.performed -= PerformSelected;
        Inputs.Instance.Controls.Main.ESC.performed -= RequestGoToMainMenu;
    }

    private void PerformSelected(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("Perform Selected: "+ selectedLevel);
        RequestStartSelectedLevel();
    }
    private void MoveInLevelSelect(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        
        Debug.Log("Move in Level Select");
        Vector2 direction = context.ReadValue<Vector2>();
        MoveInputAsVector(direction);
    }
    private void MoveInputAsVector(Vector2 direction)
    {
        if (direction.x > 0)
        {
            Debug.Log("Select Next Level");
            selectedLevel = (selectedLevel + 1) % 15;
        }
        if (direction.y > 0)
        {
            Debug.Log("Selecte previous Row");
            selectedLevel = (15 + selectedLevel - 5) % 15;
        }
        if (direction.x < 0)
        {
            Debug.Log("Select previous level");
            selectedLevel = (15 + selectedLevel - 1) % 15;
        }
        if (direction.y < 0)
        {
            Debug.Log("Select Next Row");
            selectedLevel = (selectedLevel + 5) % 15;
        }
        UpdateSelectedLevel();
    }

    private void GenerateButtonLevels()
    {
        for (int i = 0; i< 15; i++)
        {
            LevelButton newButton = Instantiate(levelButtonPrefab,levelButtonHolder.transform);
            newButton.SetLevel(i);
            buttonList.Add(newButton);
        }
    }

    public void UpdateLevelVisability()
    {
        selectedLevel = GameSettings.CurrentLevel+1;
        UpdateSelectedLevel();
    }
    
    public void UpdateSelectedLevel()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].Select(false);
        }
        Debug.Log("Next Level: "+GameSettings.CurrentLevel);
        if(selectedLevel>=0) buttonList[selectedLevel].Select();
    }


    public void RequestGoToMainMenu()
    {
        if (!Enabled()) return;
        Debug.Log("Request main menu");
        GameSettings.StoredAction = GameAction.LoadStartMenu;
        UIController.StartTransition();
    }
    
    public void RequestGoToMainMenu(InputAction.CallbackContext context)
    {
        RequestGoToMainMenu();
    }
    
    public void RequestStartSelectedLevel()
    {
        Debug.Log("Request start selected level");
        GameSettings.CurrentLevel = selectedLevel;
        GameSettings.StoredAction = GameAction.LoadSelectedLevel;
        UIController.StartTransition();
    }
    
    public void RequestStartNextLevel()
    {
        Debug.Log("Request start nex level");
        GameSettings.StoredAction = GameAction.LoadSelectedLevel;
        UIController.StartTransition();
    }


}
