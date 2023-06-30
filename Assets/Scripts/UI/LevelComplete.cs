using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : BasePanel
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI movesText;
    [SerializeField] TextMeshProUGUI stepsText;
	public void NextLevelClicked()
	{
		Debug.Log("Next Level Clicked");
		GameSettings.StoredAction = GameAction.LoadNextLevel;
        FindObjectOfType<TransitionScreen>().StartTransition();
    }
	public void MainMenuClicked()
	{
		Debug.Log("Main Menu Clicked");
		GameSettings.StoredAction = GameAction.LoadStartMenu;
        FindObjectOfType<TransitionScreen>().StartTransition();

    }

    internal void UpdateStats()
    {
        int timeTaken = Mathf.RoundToInt(Time.time - GameSettings.LevelStartTime);
        int minutes = (timeTaken / 60);
        int sec = (timeTaken % 60);
        string timeString = (minutes>0?(minutes+"m"):"")+sec+"s";


        timeText.text = timeString;
        movesText.text = GameSettings.MoveCounter.ToString();
        stepsText.text = GameSettings.StepsCounter.ToString();
    }
}
