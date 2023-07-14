using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelComplete : BasePanel
{
    [SerializeField] UIController UIController;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI movesText;
    [SerializeField] TextMeshProUGUI stepsText;
    [SerializeField] TextMeshProUGUI coinGainText;
    [SerializeField] TextMeshProUGUI tileGainText;
    [SerializeField] GameObject[] personalBests;
    [SerializeField] TextMeshProUGUI[] improvements;

    [SerializeField] Button mainSelectedButton;

    LevelSelect levelSelect;
    private int latestCoins;

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.performed += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.performed -= RequestESC;
    }

    private void Awake()
    {
        //Select first Button
        //buttons[selectedButton].Select();
        levelSelect = FindObjectOfType<LevelSelect>();
        SetSelected();
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        SelectLevelClicked();
    }

    public void SetSelected()
    {
        Debug.Log("Selecting LEvelComplete First OK Button");
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void NextLevelClicked()
	{
		Debug.Log("Next Level Clicked");
		GameSettings.StoredAction = GameAction.LoadSelectedLevel;
        UIController.StartTransition();
    }
	public void SelectLevelClicked()
	{
		Debug.Log("Select Level Clicked");
		GameSettings.StoredAction = GameAction.ShowLevelSelect;
        UIController.StartTransition();
    }
	public void MainMenuClicked()
	{
		Debug.Log("Main Menu Clicked");
		GameSettings.StoredAction = GameAction.LoadStartMenu;
        UIController.StartTransition();

    }

    internal void UpdateStats(int coins, int tiles)
    {
        int timeTaken = Mathf.RoundToInt(Time.time - GameSettings.LevelStartTime);
        int minutes = (timeTaken / 60);
        int sec = (timeTaken % 60);
        string timeString = (minutes>0?(minutes+"m"):"")+sec+"s";
        timeText.text = timeString;

        int moves = GameSettings.MoveCounter;
        movesText.text = moves.ToString();

        int steps = GameSettings.StepsCounter;
        stepsText.text = steps.ToString();

        latestCoins = coins;

        coinGainText.text = coins.ToString();
        tileGainText.text = tiles.ToString();
        LevelDefinition current = GameSettings.CurrentLevelDefinition;

        //FIX
        levelText.text = StringConverter.LevelAsString(GameSettings.CurrentLevelDefinition.LevelDiff, GameSettings.CurrentLevelDefinition.LevelIndex);

        PlayerLevelData levelData = new PlayerLevelData(current.levelID, steps, moves, timeTaken);

        //Add Data into SaveFile
        PlayerLevelData oldLevelData = SavingUtility.playerGameData.PlayerLevelDataList.GetByID(levelData.levelID);
        PlayerLevelData bestLevelData = SavingUtility.playerGameData.PlayerLevelDataList.AddOrUpdateLevel(levelData);

        //Show new record if applicable
        ShowPersonalBestIfRecord(oldLevelData, bestLevelData);

        // Update Button info as well
        levelSelect.UpdateButtonPlayerLevelData(bestLevelData);

        SetSelected();

        // Delete intertitial here?


    }

    private void ShowPersonalBestIfRecord(PlayerLevelData oldLevelData, PlayerLevelData bestLevelData)
    {
        foreach (var best in personalBests)
            best.SetActive(false);

        foreach (var improvement in improvements)
            improvement.gameObject.SetActive(false);

        if (bestLevelData.bestMoves < oldLevelData.bestMoves)
        {
            personalBests[0].SetActive(true);
            improvements[0].gameObject.SetActive(true);
            improvements[0].text = (bestLevelData.bestMoves-oldLevelData.bestMoves).ToString();
        }
        if (bestLevelData.bestSteps < oldLevelData.bestSteps) { 
            personalBests[1].SetActive(true);
            improvements[1].gameObject.SetActive(true);
            improvements[1].text = (bestLevelData.bestSteps - oldLevelData.bestSteps).ToString();
        }
        if (bestLevelData.bestTime < oldLevelData.bestTime){
            personalBests[2].SetActive(true);
            improvements[2].gameObject.SetActive(true);
            improvements[2].text = StringConverter.TimeAsString(bestLevelData.bestTime - oldLevelData.bestTime);
        }
    }

    internal void GetReward()
    {
        if (Enabled())
        {
            Debug.Log("Level complete is active reward Player with double coins");

            // Determin reward
            coinGainText.text = (latestCoins*2).ToString();
            SavingUtility.playerGameData.AddCoins(latestCoins);
        }
        else
        {
            Debug.Log("Level complete is not active.");
        }
    }
}
