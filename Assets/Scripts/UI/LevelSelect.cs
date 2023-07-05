using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LevelSelect : BasePanel
{
    [SerializeField] GameObject levelEasyButtonHolder;
    [SerializeField] GameObject levelMediumButtonHolder;
    [SerializeField] GameObject levelHardButtonHolder;
    private GameObject[] levelButtonHolders;
    private List<LevelButton>[] buttonLists;

    [SerializeField] LevelButton levelButtonPrefab;
    [SerializeField] LevelButton mediumLevelButtonPrefab;
    [SerializeField] LevelButton HardLevelButtonPrefab;

    [SerializeField] UIController UIController;
    [SerializeField] LevelCreator levelCreator;

    private int selectedLevel = 0;
    private int activeTab = 0;

    private List<LevelButton> easybuttonList = new List<LevelButton>();
    private List<LevelButton> mediumButtonList = new List<LevelButton>();
    private List<LevelButton> HardButtonList = new List<LevelButton>();

    // Start is called before the first frame update
    void Start()
    {
        levelButtonHolders = new GameObject[3] { levelEasyButtonHolder,levelMediumButtonHolder,levelHardButtonHolder};
        buttonLists = new List<LevelButton>[3] { easybuttonList, mediumButtonList, HardButtonList };
        GenerateButtonLevels();
    }

    private void OnEnable()
    {
//        Inputs.Instance.Controls.Main.Move.performed += MoveInLevelSelect;
//        Inputs.Instance.Controls.Main.Interact.performed += PerformSelected;
//        Inputs.Instance.Controls.Main.ESC.performed += RequestGoToMainMenu;
    }
    
    private void OnDisable()
    {
 //       Inputs.Instance.Controls.Main.Move.performed -= MoveInLevelSelect;
 //       Inputs.Instance.Controls.Main.Interact.performed -= PerformSelected;
 //       Inputs.Instance.Controls.Main.ESC.performed -= RequestGoToMainMenu;
    }

    private void GenerateButtonLevels()
    {
        for (int i = 0; i < levelCreator.levelsEasy.Count; i++)
        {
            LevelButton newButton = Instantiate(levelButtonPrefab, levelEasyButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Easy;
            easybuttonList.Add(newButton);
        }
        for (int i = 0; i < levelCreator.levelsMedium.Count; i++)
        {
            LevelButton newButton = Instantiate(mediumLevelButtonPrefab, levelMediumButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Medium;
            mediumButtonList.Add(newButton);
        }
        for (int i = 0; i < levelCreator.levelsHard.Count; i++)
        {
            LevelButton newButton = Instantiate(HardLevelButtonPrefab, levelHardButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Hard;
            HardButtonList.Add(newButton);
        }
        levelMediumButtonHolder.gameObject.SetActive(false);
        levelHardButtonHolder.gameObject.SetActive(false);

    }

    public void UpdateLevelVisability()
    {
        selectedLevel = GameSettings.CurrentLevel+1;
        SetSelected();
    }
    
    public void SetSelected()
    {
        Debug.Log("Select Level: "+ selectedLevel+" of type "+activeTab);
        EventSystem.current.SetSelectedGameObject(buttonLists[activeTab][selectedLevel].gameObject);
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
    
    public void RequestStartSelectedRandomLevel(int tier)
    {
        Debug.Log("Request start Random level of tier: "+tier);
    }
    
    public void RequestStartSelectedLevel(int level, DifficultLevel diff)
    {
        Debug.Log("Request start selected level");
        GameSettings.CurrentLevel = level;
        GameSettings.CurrentDifficultLevel = (int)diff;
        GameSettings.StoredAction = GameAction.LoadSelectedLevel;
        UIController.StartTransition();
    }
    
    public void RequestStartNextLevel()
    {
        Debug.Log("Request start nex level");
        GameSettings.StoredAction = GameAction.LoadSelectedLevel;
        UIController.StartTransition();
    }
    
    public void RequestShowTab(int tabID)
    {
        Debug.Log("Request show tab "+tabID);

        //Disable current Tab
        levelButtonHolders[activeTab].SetActive(false);

        //Enable requested tab
        levelButtonHolders[tabID].SetActive(true);
        activeTab = tabID;
        selectedLevel = 0;
        SetSelected();
    }


}
