using MyGameAds;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : EscapableBasePanel
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
    [SerializeField] HorizontalLayoutGroup[] layoutgroups;

    LevelSelect levelSelect;
    private int latestCoins;



    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        SelectLevelClicked();
    }

    private void Awake()
    {
        //Select first Button
        //buttons[selectedButton].Select();
        levelSelect = FindObjectOfType<LevelSelect>();
        SetSelected();

        InterstitialController.Closed += RegainFocus;
        RewardedController.Closed += RegainFocus;
    }

    private void RegainFocus()
    {
        Debug.Log("LevelComplete Regain focus = Setselected");
        SetSelected();
    }
    public void NextLevelClicked()
	{
		Debug.Log("Next Level Clicked");
        TransitionScreen.Instance.StartTransition(GameAction.LoadSelectedLevel);
    }
	public void SelectLevelClicked()
	{
		Debug.Log("Select Level Clicked");
        TransitionScreen.Instance.StartTransition(GameAction.ShowLevelSelect);
    }
	public void MainMenuClicked()
	{
		Debug.Log("Main Menu Clicked");
        TransitionScreen.Instance.StartTransition(GameAction.LoadStartMenu);

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

        SoundController.Instance.PlaySFX(SFX.GainCoin);

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
        //LayoutRebuilder.MarkLayoutForRebuild(completeInformationRect);

        StartCoroutine(ForceUpdate());

    }

    private IEnumerator ForceUpdate()
    {
        EnableLayoutGroups(false);
        yield return null;
        EnableLayoutGroups(true);
    }

    private void EnableLayoutGroups(bool set)
    {
        foreach (var group in layoutgroups)
        {
            group.enabled = set;
        }
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
            SoundController.Instance.PlaySFX(SFX.GainCoin);
        }
        else
        {
            Debug.Log("Level complete is not active.");
        }
    }
}
