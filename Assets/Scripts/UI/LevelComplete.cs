using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : BasePanel
{
	public void NextLevelClicked()
	{
		Debug.Log("Next Level Clicked");
		HidePanel();
        FindObjectOfType<LevelCreator>().LoadNextLevel();
    }
	public void MainMenuClicked()
	{
		Debug.Log("Main Menu Clicked");
		HidePanel();
		FindObjectOfType<UIController>().ShowMainLevel();

	}

}
