using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : BasePanel
    {
	[SerializeField] private GameObject buttonHolder;
	[SerializeField] private List<Image> images;

    private UIController uIController;
    private void Start()
    {
        uIController = FindObjectOfType<UIController>();
    }

    public void StartGameClicked()
	{
		Debug.Log("StartGame Clicked");
		TransitionScreen.Instance.StartTransition(GameAction.ShowLevelSelect);
	}
	public void SettingsClicked()
	{
		Debug.Log("Settings Clicked");
        uIController.RequestSettings(ReturnMenuType.Main);

    }
	public void AchievementsClicked()
	{
		Debug.Log("Achievements Clicked");
		uIController.RequestAchievements();
	}
	public void CreditsClicked()
	{
		Debug.Log("Credits Clicked");
		uIController.RequestCredits();
	}
	public void CustomizeClicked()
	{
		Debug.Log("Customize Clicked");
		uIController.RequestCustomize();
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
