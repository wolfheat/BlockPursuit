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

    private PlayerInventory playerInventory;
    private SavingUtility savingUtility;

    private List<LevelButton> easybuttonList = new List<LevelButton>();
    private List<LevelButton> mediumButtonList = new List<LevelButton>();
    private List<LevelButton> HardButtonList = new List<LevelButton>();

    // Start is called before the first frame update
    void Start()
    {
        levelButtonHolders = new GameObject[3] { levelEasyButtonHolder,levelMediumButtonHolder,levelHardButtonHolder};
        buttonLists = new List<LevelButton>[3] { easybuttonList, mediumButtonList, HardButtonList };
        playerInventory = FindObjectOfType<PlayerInventory>();
        //GenerateButtonLevels();
    }

    private void OnEnable()
    {
        SavingUtility.LoadingComplete += GenerateButtonLevelsWhenLoadingIsComplete;
    }
    
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= GenerateButtonLevelsWhenLoadingIsComplete;
    }

    public void GenerateButtonLevelsWhenLoadingIsComplete()
    {
        Debug.Log("Loaded transmitted");
        GenerateButtonLevels();
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
            Debug.Log("playerInventory: "+ playerInventory);
            Debug.Log("playerInventory.PlayerLevelsDefinition: " + playerInventory.PlayerLevelsDefinition);
            Debug.Log("newButton.levelDefinition: " + newButton.levelDefinition);

            PlayerLevelDefinition levelDef = playerInventory.PlayerLevelsDefinition.GetDefinitionForID(newButton.levelDefinition.levelID);
            if(levelDef.levelID != -1)
                newButton.playerLevelDefinition = levelDef;

            easybuttonList.Add(newButton);
        }
        for (int i = 0; i < Levels.LevelDefinitions[1].Count; i++)
        {
            LevelButton newButton = Instantiate(mediumLevelButtonPrefab, levelMediumButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Medium;
            newButton.levelDefinition = Levels.LevelDefinitions[1][i];

            PlayerLevelDefinition levelDef = playerInventory.PlayerLevelsDefinition.GetDefinitionForID(newButton.levelDefinition.levelID);
            if (levelDef.levelID != -1)
                newButton.playerLevelDefinition = levelDef;

            mediumButtonList.Add(newButton);
        }
        for (int i = 0; i < Levels.LevelDefinitions[2].Count; i++)
        {
            LevelButton newButton = Instantiate(HardLevelButtonPrefab, levelHardButtonHolder.transform);
            newButton.SetLevel(i);
            newButton.difficulty = DifficultLevel.Hard;
            newButton.levelDefinition = Levels.LevelDefinitions[2][i];

            PlayerLevelDefinition levelDef = playerInventory.PlayerLevelsDefinition.GetDefinitionForID(newButton.levelDefinition.levelID);
            if (levelDef.levelID != -1)
                newButton.playerLevelDefinition = levelDef;

            HardButtonList.Add(newButton);
        }
        levelMediumButtonHolder.gameObject.SetActive(false);
        levelHardButtonHolder.gameObject.SetActive(false);

    }

    public void UnlockActiveLevel()
    {
        Debug.Log("Unlock Current Level " + selectedLevel);
    }
    public void UpdateLevelVisability()
    {
        selectedLevel = GameSettings.CurrentLevel+1;
        SetSelected();
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
        infoScreen.UpdateInfo(LevelID,LevelDiff,unlockCost, buttonLists[activeTab][selectedLevel].playerLevelDefinition);
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
        if (buttonLists[(int)diff][level].levelDefinition?.unlockRequirements?.Count > 0) {
            Debug.Log("Level Has unlock requirement = " + buttonLists[(int)diff][level].levelDefinition.unlockRequirements[0].amount+" tiles");
            return;
        }
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
