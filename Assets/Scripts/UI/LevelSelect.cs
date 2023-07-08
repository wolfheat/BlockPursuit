using System;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private LevelButton[] LevelButtonPrefabs;

    [SerializeField] UIController UIController;
    [SerializeField] LevelCreator levelCreator;
    [SerializeField] UnlockScreen unlockScreen;
    [SerializeField] InfoScreen infoScreen;

    private int selectedLevel = 0;
    private int activeTab = 0;
    private LevelButton selectedButton;

    private SavingUtility savingUtility;

    private List<LevelButton> easybuttonList = new List<LevelButton>();
    private List<LevelButton> mediumButtonList = new List<LevelButton>();
    private List<LevelButton> HardButtonList = new List<LevelButton>();

    // Start is called before the first frame update
    void Start()
    {
        levelButtonHolders = new GameObject[3] { levelEasyButtonHolder,levelMediumButtonHolder,levelHardButtonHolder};
        buttonLists = new List<LevelButton>[3] { easybuttonList, mediumButtonList, HardButtonList };
        //LevelButtonPrefabs = new LevelButton[3] { levelButtonPrefab, mediumLevelButtonPrefab, hardLevelButtonPrefab };
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
        selectedButton = buttonLists[0][0];
    }

    public void UpdateButtonPlayerLevelData(PlayerLevelData data)
    {
        //Find button
        LevelButton buttonToUpdate = buttonLists[GameSettings.CurrentLevelDefinition.LevelDiff][GameSettings.CurrentLevelDefinition.LevelIndex];
        buttonToUpdate.playerLevelData = data;
        buttonToUpdate.ShowCheckmark();
    }

    private void GenerateButtonLevels()
    {
        Debug.Log("Generating Button Levels");

        for (int i = 0; i < Levels.LevelDefinitions.Length; i++)
        {
            for (int j = 0; j < Levels.LevelDefinitions[i].Count; j++)
            {
                //Reset all buttons to locked by default
                Levels.LevelDefinitions[i][j].unlocked = false;

                LevelButton newButton = Instantiate(LevelButtonPrefabs[i], levelButtonHolders[i].transform);
                newButton.SetLevel(j);
                newButton.difficulty = (DifficultLevel)i;
                newButton.levelDefinition = Levels.LevelDefinitions[i][j];

                PlayerLevelData levelDef = SavingUtility.playerGameData.PlayerLevelDataList.GetByID(newButton.levelDefinition.levelID);
                if (levelDef.levelID != -1)
                {
                    newButton.playerLevelData = levelDef;
                    Levels.LevelDefinitions[i][j].unlocked = true;

                    newButton.Unlock();
                    if (levelDef.bestTime != -1)
                        newButton.ShowCheckmark();
                }

                buttonLists[i].Add(newButton);
            }
            if(i>0) levelButtonHolders[i].gameObject.SetActive(false);
        }
    }

    public void UpdateLatestSelectedInfo(LevelButton button)
    {
        selectedButton = button;
        infoScreen.UpdateInfo(button);
    }

    public void SetSelected()   
    {
        EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);

        // Update Level Info
        infoScreen.UpdateInfo(selectedButton);
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
        if (!level.unlocked && level?.unlockRequirements?.Count > 0) {
            Debug.Log("Level Has unlock requirement = " + level.unlockRequirements[0].amount+" tiles");
            unlockScreen.SetInfo(level);
            unlockScreen.ShowPanel();
            HidePanel();
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

        // Change to first not cleared? 
        selectedButton = buttonLists[tabID][0];
        SetSelected();
    }

    internal void Unlock()
    {
       infoScreen.UnLock();
    }
}
