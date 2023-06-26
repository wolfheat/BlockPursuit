using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
	[SerializeField] private GameObject startMenu;
	[SerializeField] private GameObject buttonHolder;
	[SerializeField] private List<Image> images;

	public void ShowMenu() => startMenu.SetActive(true);
	public void HideMenu() => startMenu.SetActive(false);

    private void ToggleStartMenu()
	{
		Debug.Log("Toggle start Menu.");
		Debug.Log("Gameobject: "+ startMenu.gameObject);
		startMenu.gameObject.SetActive(!startMenu.gameObject.activeSelf);

	}

	public void StartGameClicked()
	{
		Debug.Log("StartGame Clicked");
		GameSettings.CurrentGameState = GameState.RunGame;
		startMenu.gameObject.SetActive(false);
		FindObjectOfType<LevelCreator>().CreateLevel();
	}
	public void SettingsClicked()
	{
		Debug.Log("Settings Clicked");
	}
	public void CreditsClicked()
	{
		Debug.Log("Credits Clicked");
	}
	public void QuitGameClicked()
	{
		Debug.Log("QuitGame Clicked");	
	}

}
