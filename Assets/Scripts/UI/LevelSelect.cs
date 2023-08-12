using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LevelSelect : EscapableBasePanel
{
    [SerializeField] GameObject levelEasyButtonHolder;
    [SerializeField] GameObject levelMediumButtonHolder;
    [SerializeField] GameObject levelHardButtonHolder;
    [SerializeField] GameObject levelABCButtonHolder;
    private GameObject[] levelButtonHolders;
    private List<LevelButton>[] buttonLists;

    [SerializeField] private LevelButton[] LevelButtonPrefabs;

    [SerializeField] UIController UIController;
    [SerializeField] LevelCreator levelCreator;
    [SerializeField] UnlockScreen unlockScreen;
    [SerializeField] InfoScreen infoScreen;

    private int selectedLevel = 0;
    private int activeTab = 0;
    [SerializeField] private GameObject startbutton;
    [SerializeField] private TierButton[] tierButtons;

    private SavingUtility savingUtility;

    private List<LevelButton> easybuttonList = new List<LevelButton>();
    private List<LevelButton> mediumButtonList = new List<LevelButton>();
    private List<LevelButton> HardButtonList = new List<LevelButton>();
    private List<LevelButton> ABCButtonList = new List<LevelButton>();

    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        RequestGoToMainMenu();
    }
    void Start()
    {
        levelButtonHolders = new GameObject[4] { levelEasyButtonHolder,levelMediumButtonHolder,levelHardButtonHolder, levelABCButtonHolder };
        buttonLists = new List<LevelButton>[4] { easybuttonList, mediumButtonList, HardButtonList, ABCButtonList };
        //LevelButtonPrefabs = new LevelButton[3] { levelButtonPrefab, mediumLevelButtonPrefab, hardLevelButtonPrefab };
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
        infoScreen.latestButton = buttonLists[0][0];
    }

    public void UpdateButtonPlayerLevelData(PlayerLevelData data)
    {
        //Find button
        LevelButton buttonToUpdate = buttonLists[GameSettings.CurrentLevelDefinition.LevelDiff][GameSettings.CurrentLevelDefinition.LevelIndex];
        buttonToUpdate.playerLevelData = data;
        buttonToUpdate.ShowCheckmark();
    }

    public void ResetLevelSelect()
    {
        RemoveAllButtons();
        GenerateButtonLevels();
        infoScreen.latestButton = buttonLists[0][0];
    }

    private void RemoveAllButtons()
    {
        for (int i = buttonLists.Length-1; i >= 0; i--)
        {
            for (int j = buttonLists[i].Count-1; j >= 0; j--)
            {
                Destroy(buttonLists[i][j].gameObject);
            }
            buttonLists[i].Clear();
        }
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
        infoScreen.UpdateInfo(button);
    }

    public void RequestGoToMainMenu()
    {
        if (!Enabled()) return;
        Debug.Log("Request main menu");
        TransitionScreen.Instance.StartTransition(GameAction.LoadStartMenu);
    }
    
    public void RequestGoToMainMenu(InputAction.CallbackContext context)
    {
        RequestGoToMainMenu();
    }
    
    public void RequestStartSelectedRandomLevel(int tier)
    {
        Debug.Log("Request start Random level of tier: "+tier);
    }

    public void ConfirmLevelSelectJumpToStartButton(LevelButton button)
    {
        if (Inputs.Instance.Controls.UI.Submit.WasPressedThisFrame())
            ClickLevelByKeyboard(button);
        else
            ClickLevelByTouch(button);
    }

    private void ClickLevelByTouch(LevelButton button)
    {
        Debug.Log("Click by Mouse");
        // Click level by Mouse or touch
        // Do nothing? Only want to select with touch

    }

    private void ClickLevelByKeyboard(LevelButton button)
    {
        // Info screen already showing now start level or show unlock screen
        Debug.Log("Click by Keyboard");
        if (!infoScreen.latestButton.levelDefinition.unlocked)
            ShowUnlockPanel();
        else
            RequestStartSelectedLevel();

        // USed to jump to Start button when using keyboard for unlocked level, changed to start level directly
            //EventSystem.current.SetSelectedGameObject(startbutton.gameObject);
    }

    public void RequestStartSelectedLevel()
    {

        // Else start the level
        LevelDefinition level = infoScreen.latestButton.levelDefinition;

        GameSettings.CurrentLevelDefinition = level;
        UIController.UpdateIngameLevelShown();

        TransitionScreen.Instance.StartTransition(GameAction.LoadSelectedLevel);
    }

    public void ShowUnlockPanel()
    {
        unlockScreen.SetInfo(infoScreen.latestButton.levelDefinition);
        TransitionScreen.Instance.StartTransition(GameAction.ShowUnlock);
    }

    public void RequestShowTab(TierButton tierButton)
    {
        SwitchToTab(tierButton);

        base.SetSelected();
    }

    private void SwitchToTab(TierButton tierButton)
    {
        int tabID = tierButton.ID;// tierButton.ID;
        Debug.Log("Request show tab " + tabID);

        //Disable current Tab
        levelButtonHolders[activeTab].SetActive(false);

        //Enable requested tab
        levelButtonHolders[tabID].SetActive(true);
        activeTab = tabID;

        SetSelectedLevelToDefaultForActiveTab();
        

        HighLightCorrectTierButtons(tierButton);

    }

    public override void SetSelected()
    {
        selectedLevel = infoScreen.latestButton.level;
        infoScreen.latestButton = buttonLists[activeTab][selectedLevel];
        EventSystem.current.SetSelectedGameObject(buttonLists[activeTab][selectedLevel].gameObject);
    }
    
    public void SetSelectedLevelToDefaultForActiveTab()
    {
        selectedLevel = FindDefaultLevelToSelectForTab(activeTab);
        infoScreen.latestButton = buttonLists[activeTab][selectedLevel];
        SetSelected();
    }

    private void HighLightCorrectTierButtons(TierButton tierButton)
    {
        foreach (TierButton button in tierButtons)
            button.HighLight(button.ID==tierButton.ID?true:false);  
    }

    private int FindDefaultLevelToSelectForTab(int tabID)
    {
        // Make the selected level the first one that is not completed
        for (int i = 0; i < buttonLists[tabID].Count; i++)
        {
            // Unlocked but not completed
            if (buttonLists[tabID][i].playerLevelData.bestTime == -1)
            {
                return i;
            }

        }

        for (int i = 0; i < buttonLists[tabID].Count; i++)
        {
            // First locked level
            if (!buttonLists[tabID][i].levelDefinition.unlocked)
            {
                return i;
            }

        }
        return 0;
    }

    internal void Unlock()
    {
       infoScreen.UnLock();
    }
}
