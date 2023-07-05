using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : BasePanel
    {
	[SerializeField] private GameObject buttonHolder;
	[SerializeField] private List<Image> images;

    [SerializeField] Button mainSelectedButton;
    private void Start()
    {
        ShowPanel();
		SetSelected();		
    }

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);	
    }

    public void StartGameClicked()
	{
		Debug.Log("StartGame Clicked");
		GameSettings.StoredAction = GameAction.ShowLevelSelect;
		FindObjectOfType<TransitionScreen>().StartTransition();
	}
	public void SettingsClicked()
	{
		Debug.Log("Settings Clicked");
	}
	public void AchievementsClicked()
	{
		Debug.Log("Achievements Clicked");
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
