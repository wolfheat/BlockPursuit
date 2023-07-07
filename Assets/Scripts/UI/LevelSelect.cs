using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] InfoScreen infoScreen;

    private int selectedLevel = 0;
    private int activeTab = 0;

    private SavingUtility savingUtility;

    private List<LevelButton> easybuttonList = new List<LevelButton>();
    private List<LevelButton> mediumButtonList = new List<LevelButton>();
    private List<LevelButton> HardButtonList = new List<LevelButton>();

    // Start is called before the first frame update
    void Start()
    {
        levelButtonHolders = new GameObject[3] { levelEasyButtonHolder,levelMediumButtonHolder,levelHardButtonHolder};
        buttonLists = new List<LevelButton>[3] { easybuttonList, mediumButtonList, HardButtonList };
        //GenerateButtonLevels();
    }

    private void OnEnable()
    {
        SavingUtility.LoadingComplete += GenerateButtonLevelsWhenLoadingIsComplete; 
        Inputs.Instance.Controls.Main.ESC.performed += RequestESC;
    }
    
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= GenerateButtonLevelsWhenLoadingIsComplete;
        Inputs.Instance.Controls.Main.ESC.performed -= RequestESC;
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        RequestGoToMainMenu();
    }

    public void GenerateButtonLevelsWhenLoadingIsComplete()
    {
        Debug.Log("Loaded transmitted");
        GenerateButtonLevels();
    }

    public void UpdateButtonPlayerLevelData(PlayerLevelData data)
    {
        //Debug.Log("Updating player data for button ("+GameSettings.CurrentDifficultLevel+","+GameSettings.CurrentLevel+")");   
        Debug.Log("LevelData level index = ("+GameSettings.CurrentLevelDefinition.LevelDiff+","+ GameSettings.CurrentLevelDefinition.LevelIndex + ")");   
        //Find button
        LevelButton buttonToUpdate = buttonLists[GameSettings.CurrentLevelDefinition.LevelDiff][GameSettings.CurrentLevelDefinition.LevelIndex];
        buttonToUpdate.playerLevelData = data;
    }

    private void GenerateButtonLevels()
    {
        Debug.Log("Generating Button Levels");
        for (int i = 0; i < Levels.LevelDefinitions[0].Count; i++)
        {
            LevelButton newButton = Instantiate(levelButtonPrefab, levelEasyButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Easy;
            newButton.levelDefinition = Levels.LevelDefinitions[0][i];

            PlayerLevelData levelDef = SavingUtility.playerGameData.PlayerLevelDataList.GetByID(newButton.levelDefinition.levelID);
            if(levelDef.levelID != -1)
                newButton.playerLevelData = levelDef;

            easybuttonList.Add(newButton);
        }
        for (int i = 0; i < Levels.LevelDefinitions[1].Count; i++)
        {
            LevelButton newButton = Instantiate(mediumLevelButtonPrefab, levelMediumButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Medium;
            newButton.levelDefinition = Levels.LevelDefinitions[1][i];

            PlayerLevelData levelDef = SavingUtility.playerGameData.PlayerLevelDataList.GetByID(newButton.levelDefinition.levelID);
            if (levelDef.levelID != -1)
                newButton.playerLevelData = levelDef;

            mediumButtonList.Add(newButton);
        }
        for (int i = 0; i < Levels.LevelDefinitions[2].Count; i++)
        {
            LevelButton newButton = Instantiate(HardLevelButtonPrefab, levelHardButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Hard;
            newButton.levelDefinition = Levels.LevelDefinitions[2][i];

            PlayerLevelData levelDef = SavingUtility.playerGameData.PlayerLevelDataList.GetByID(newButton.levelDefinition.levelID);
            if (levelDef.levelID != -1)
                newButton.playerLevelData = levelDef;

            HardButtonList.Add(newButton);
        }
        levelMediumButtonHolder.gameObject.SetActive(false);
        levelHardButtonHolder.gameObject.SetActive(false);

    }

    public void SetSelected()   
    {
        Debug.Log("Select Level: "+ selectedLevel+" of type "+activeTab);
        Debug.Log("Buttons: "+ buttonLists[activeTab].Count);
        EventSystem.current.SetSelectedGameObject(buttonLists[activeTab][selectedLevel].gameObject);

        int LevelID = buttonLists[activeTab][selectedLevel].level;
        int LevelDiff = (int)buttonLists[activeTab][selectedLevel].difficulty;
        int unlockCost = 0;
        if(buttonLists[activeTab][selectedLevel].levelDefinition.unlockRequirements.Count > 0)
        {
            LevelDefinition definition = buttonLists[activeTab][selectedLevel].levelDefinition;
            unlockCost = definition.unlockRequirements[0].amount;
        }

        // Update Level Info
        infoScreen.UpdateInfo(LevelID,LevelDiff,unlockCost, buttonLists[activeTab][selectedLevel].levelDefinition, buttonLists[activeTab][selectedLevel].playerLevelData);
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
    
    public void RequestStartSelectedLevel(LevelDefinition level)
    {
        Debug.Log("Request start selected level");
        if (level?.unlockRequirements?.Count > 0) {
            Debug.Log("Level Has unlock requirement = " + level.unlockRequirements[0].amount+" tiles");
            return;
        }
        GameSettings.CurrentLevelDefinition = level;
        UIController.UpdateIngameLevelShown();

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
