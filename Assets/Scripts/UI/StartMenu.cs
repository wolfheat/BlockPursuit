using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : BasePanel
    {
	[SerializeField] private GameObject buttonHolder;
	[SerializeField] private List<Image> images;

    private void Start()
    {
        ShowPanel();
    }
    public void StartGameClicked()
	{
		Debug.Log("StartGame Clicked");
		GameSettings.CurrentGameState = GameState.RunGame;
		HidePanel();
		FindObjectOfType<LevelCreator>().LoadNextLevel();
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

#if UNITY_EDITOR
	UnityEditor.EditorApplication.isPlaying = false;
#endif

	Application.Quit();
	}

}
