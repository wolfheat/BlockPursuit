using MyGameAds;
using System;
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
    [SerializeField] GameObject tileGain;
    [SerializeField] GameObject[] personalBests;
    [SerializeField] TextMeshProUGUI[] improvements;
    [SerializeField] HorizontalLayoutGroup[] layoutgroups;

    LevelSelect levelSelect;
    private int latestCoins;

    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        OkClicked();
    }

    private void Awake()
    {
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
	public void OkClicked()
	{
		Debug.Log("Select Level Clicked");
        TransitionScreen.Instance.StartTransition(GameAction.ShowLevelSelect);
    }
	public void MainMenuClicked()
	{
		Debug.Log("Main Menu Clicked");
        TransitionScreen.Instance.StartTransition(GameAction.LoadStartMenu);

    }

    [SerializeField] GameObject loadBoostButton;
    [SerializeField] GameObject loading;
    [SerializeField] RewardedController rewardedController;
    private bool checkForLoadedAd;
    [SerializeField] GameObject loadedAdCheck;
    private LevelDefinition current;

    public void LoadBoostClicked()
    {
        Debug.Log("LoadBoostClicked");
        loadBoostButton.SetActive(false);
        loading.SetActive(true);
        checkForLoadedAd = true;
        rewardedController.LoadAd();
    }

    public void UpdateLoadBoostButton()
    {
        Debug.Log("Update Load Boost Button, when opening level complete");
        if (loadedAdCheck.activeSelf)
            loadBoostButton.SetActive(false);
        else
            loadBoostButton.SetActive(true);

        loading.SetActive(false);
        checkForLoadedAd = false;
    }

    private void Update()
    {
        if (checkForLoadedAd)
        {
            if (loadedAdCheck.activeSelf)
            {
                loadBoostButton.SetActive(false);
                loading.SetActive(false);
                checkForLoadedAd = false;
                BoostRequest();
            }
        }
    }
    public void BoostRequest()
    {
        Debug.Log("Request Boost Ad, show ad and return here");
        rewardedController.ShowAd();
    }

    internal void UpdateStats(int coins, int tiles)
    {
        // Set all text for Level Complete
        int timeTaken = Mathf.RoundToInt(Time.time - GameSettings.LevelStartTime);
        timeText.text = StringConverter.TimeAsString(timeTaken);        

        int moves = GameSettings.MoveCounter;
        movesText.text = moves.ToString();

        int steps = GameSettings.StepsCounter;
        stepsText.text = steps.ToString();

        latestCoins = coins;

        coinGainText.text = coins.ToString();
        if(tiles==0)
            tileGain.gameObject.SetActive(false);
        else
        {
            tileGain.gameObject.SetActive(true);
            tileGainText.text = tiles.ToString();
        }
        current = GameSettings.CurrentLevelDefinition;

        SoundController.Instance.PlaySFX(SFX.GainCoin);

        //FIX
        levelText.text = StringConverter.LevelAsString(current.LevelDiff, current.LevelIndex);

        // Create a level Data file from this completion
        PlayerLevelData levelData = new PlayerLevelData(current.levelID, steps, moves, timeTaken);

        //Add Data into SaveFile (Compare to old one and update)
        PlayerLevelData oldLevelData = SavingUtility.playerGameData.PlayerLevelDataList.GetByID(levelData.levelID);
        PlayerLevelData bestLevelData = SavingUtility.playerGameData.PlayerLevelDataList.AddOrUpdateLevel(levelData);

        //Show new record if applicable
        ShowPersonalBestIfRecord(oldLevelData, bestLevelData);

        // Update Button info as well
        levelSelect.UpdateButtonPlayerLevelData(bestLevelData,current.LevelDiff,current.LevelIndex);

        SetSelected();
    }

    private IEnumerator ForceTransformsPositionUpdate()
    {
        //TODO Change this to same method used in setting Information screen size
        Debug.Log("ForceTransformsPositionUpdate: ");
        yield return new WaitForSeconds(0.1f);
        improvements[0].GetComponent<RectTransform>().localPosition = GetNewRectPosition(movesText);
        improvements[1].GetComponent<RectTransform>().localPosition = GetNewRectPosition(stepsText);
        improvements[2].GetComponent<RectTransform>().localPosition = GetNewRectPosition(timeText);   
        
    }

    private Vector3 GetNewRectPosition(TextMeshProUGUI parentTextField)
    {
        float padding = 10f;
        RectTransform rect = parentTextField.gameObject.GetComponent<RectTransform>();
        return new Vector3(rect.localPosition.x + rect.sizeDelta.x + padding, rect.localPosition.y, 0);
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
        if (!Enabled()) return;
        
        Debug.Log("Level complete is active reward Player with double coins");
        // Determin reward
        coinGainText.text = (latestCoins*2).ToString();
        SavingUtility.playerGameData.AddCoins(latestCoins);
        SoundController.Instance.PlaySFX(SFX.GainCoin);
        
    }

    internal void FixTextFields()
    {
        StartCoroutine(ForceTransformsPositionUpdate());
    }
}
