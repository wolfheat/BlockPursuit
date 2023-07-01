using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelect : BasePanel
{
    [SerializeField] GameObject levelButtonHolder;
    [SerializeField] LevelButton levelButtonPrefab;

    [SerializeField] UIController UIController;

    private List<LevelButton> buttonList = new List<LevelButton>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateButtonLevels();
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

    public void SelectFirstLevel()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].Select(false);
        }
        Debug.Log("Next Level: "+GameSettings.CurrentLevel);
        if(GameSettings.CurrentLevel>0) buttonList[GameSettings.CurrentLevel-1].Select();
    }


    public void RequestGoToMainMenu()
    {
        Debug.Log("Request main menu");
        GameSettings.StoredAction = GameAction.LoadStartMenu;
        UIController.StartTransition();

    }
    
    public void RequestStartNextLevel()
    {
        Debug.Log("Request start nex level");
        GameSettings.StoredAction = GameAction.LoadNextLevel;
        UIController.StartTransition();
    }


}
