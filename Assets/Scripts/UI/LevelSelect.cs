using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelSelect : EscapableBasePanel
{
    [SerializeField] GameObject levelEasyButtonHolder;
    [SerializeField] GameObject levelMediumButtonHolder;
    [SerializeField] GameObject levelHardButtonHolder;
    [SerializeField] GameObject levelABCButtonHolder;
    [SerializeField] private List<GameObject> levelButtonHolders = new List<GameObject>();

    [SerializeField] private GameObject levelButtonHolderPrefab;
    [SerializeField] private GameObject levelButtonHolderParent;
    [SerializeField] private RectTransform levelButtonHolderViewportRecttransform;

    private List<LevelButton>[] buttonLists;

    [SerializeField] private LevelButton[] LevelButtonPrefabs;

    [SerializeField] UIController UIController;
    [SerializeField] LevelCreator levelCreator;
    [SerializeField] UnlockScreen unlockScreen;
    [SerializeField] InfoScreen infoScreen;

    private int selectedLevel = 0;
    private int activeTab = 0;
    [SerializeField] private GameObject startbutton;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TierButton[] tierButtons;

    [SerializeField] private GameObject completedMissionsObject;
    [SerializeField] private TextMeshProUGUI completedMissionsText;

    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        RequestGoToMainMenu();
    }

    private void OnEnable()
    {
        SavingUtility.LoadingComplete += GenerateButtonLevelsWhenLoadingIsComplete; 
        PlayerGameData.MissionCompleted += UpdateCompletedMissionsTo; 
        PlayerGameData.UnlockTier += UnlockTier; 
    }
    
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= GenerateButtonLevelsWhenLoadingIsComplete;
        PlayerGameData.MissionCompleted -= UpdateCompletedMissionsTo; 
        PlayerGameData.UnlockTier -= UnlockTier;
    }

    public void UpdateCompletedMissionsTo(int amt)
    {
        completedMissionsObject.SetActive(amt > 0);
        completedMissionsText.text = amt.ToString();        
    }
    
    public void GenerateButtonLevelsWhenLoadingIsComplete()
    {
        Debug.Log("Loaded transmitted");
        GenerateButtonLevels();
        UpdateTierCompletionCheckMarks();
        infoScreen.latestButton = buttonLists[0][0];
    }

    public void UpdateButtonPlayerLevelData(PlayerLevelData data,int tier, int lev)
    {
        //Find button
        LevelButton buttonToUpdate = buttonLists[tier][lev];
        buttonToUpdate.playerLevelData = data;
        buttonToUpdate.ShowCheckmark();

        UpdateTierCompletionCheckMarks();
    }

    private void UpdateTierCompletionCheckMarks()
    {
        for (int i = 0; i < buttonLists.Length; i++)
        {
            bool completed = true;
            for (int j = 0; j < buttonLists[i].Count; j++)
            {
                if (!buttonLists[i][j].IsCompleted)
                {
                    completed = false;
                    break;
                }
            }
            if(completed) tierButtons[i].SetAsCompleted();
        }
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
        IntializeListsIfNecessary();

        for (int i = 0; i < Levels.LevelDefinitions.Length; i++)
        {   
            for (int j = 0; j < Levels.LevelDefinitions[i].Count; j++)
            {
                //Reset levels to locked by default
                Levels.LevelDefinitions[i][j].unlocked = false;

                LevelButton newButton = Instantiate(LevelButtonPrefabs[i], levelButtonHolders[i].transform);
                newButton.SetLevel(j);
                newButton.difficulty = (DifficultLevel)i;
                newButton.levelDefinition = Levels.LevelDefinitions[i][j];

                PlayerLevelData levelDef = SavingUtility.playerGameData.PlayerLevelDataList.GetByID(newButton.levelDefinition.levelID);
                if (levelDef.levelID != -1 || Levels.LevelDefinitions[i][j].unlockRequirements == 0) // Unlocks levels that is in save or if free
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

    private void IntializeListsIfNecessary()
    {
        if (buttonLists != null) return; // Already initialized

        buttonLists = new List<LevelButton>[Levels.LevelDefinitions.Length];
        
        // Generate Holders for Each Tier
        for (int i = 0; i < Levels.LevelDefinitions.Length; i++)
        {
            buttonLists[i] = new List<LevelButton>();
            GameObject newHolder = Instantiate(levelButtonHolderPrefab, levelButtonHolderParent.transform);
            newHolder.name = "Holder" + (i + 1);
            newHolder.GetComponent<ScrollRect>().viewport = levelButtonHolderViewportRecttransform;
            GameObject holderChild = newHolder.transform.GetChild(0).gameObject;
            holderChild.name = "Holder" + (i + 1);
            levelButtonHolders.Add(holderChild);
        }
    }

    public void UpdateLatestSelectedInfo(LevelButton button)
    {
        Debug.Log("Update SnapPosition");
        Debug.Log("activetab = "+ activeTab);
        Debug.Log("levelButtonHolders[activeTab].transform.parent = " + levelButtonHolders[activeTab].transform.parent.name);

        // Enables keyboard to be used in scrollrect - centering the selected item correctly
        levelButtonHolders[activeTab].transform.parent.localPosition = levelButtonHolders[activeTab].transform.parent.GetComponent<ScrollRect>().GetSnapToVerticalPositionToBringChildIntoView(button.GetComponent<RectTransform>());
        //scrollRect.content.localPosition = scrollRect.GetSnapToVerticalPositionToBringChildIntoView(button.GetComponent<RectTransform>());

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

    public void ShowMissions()
    {
        TransitionScreen.Instance.StartTransition(GameAction.ShowMissions);
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
    
    public void UnlockTier(int tierID)
    {
        Debug.Log("Unlocking tier button "+tierID);
        tierButtons[tierID].gameObject.SetActive(true);
    }

}



public static class ScrollRectExtensions
{
    // Enables keyboard to be used in scrollrect - centering the selected item correctly
    public static Vector2 GetSnapToVerticalPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();
        
        // Force to top row
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Debug.Log("viewportLocalPosition: "+ viewportLocalPosition);
        Vector2 childLocalPosition = child.localPosition;
        Debug.Log("childLocalPosition: " + childLocalPosition);
        Vector2 thisRectPosition = instance.GetComponent<RectTransform>().localPosition;
        Debug.Log("thisRectPosition: " + thisRectPosition);
        float padding = instance.content.gameObject.GetComponent<GridLayoutGroup>().padding.top;
        float correction = instance.viewport.rect.height / 2; //107.15f;
        float contentPosition = instance.content.localPosition.y-correction;

        float parentTopPos = 0;
        float parentBottomPos = instance.viewport.rect.height;
        float childBottomPos = -child.localPosition.y + child.rect.height;
        float childTopPos = -child.localPosition.y;

       // Childs position when taking contents position into account
        float childBottomPosAdjusted = childBottomPos - contentPosition;
        float childTopPosAdjusted = childTopPos - contentPosition;
        
        // If child is outside of viewport scroll it inside
        bool isBelow = childBottomPosAdjusted > instance.viewport.rect.height;
        bool isAbove = childTopPosAdjusted < parentTopPos;
        
        // If below check how much below and move up that amount


        //Debug.Log("Below "+isBelow +" childPos: "+childLocalPosition.y+" ScrollHeight: "+instance.viewport.rect.height);

        Vector2 newPos = new Vector2();

        if (isBelow)
        {
            float amountBelow = childBottomPosAdjusted - parentBottomPos + padding;
            newPos = new Vector2(viewportLocalPosition.x, correction + contentPosition + amountBelow);
            return newPos;
        }
        if (isAbove)
        {
            float amountBelow = childTopPosAdjusted - parentTopPos - padding;
            newPos = new Vector2(viewportLocalPosition.x, correction + contentPosition + amountBelow);
            return newPos;
        }

        // THis is Good
        return new Vector2(0, contentPosition + correction);
        //return new Vector2(viewportLocalPosition.x, viewportLocalPosition.y+0);
    }
}