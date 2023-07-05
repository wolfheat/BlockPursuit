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

    [SerializeField] Button mainSelectedButton;
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
        SetSelected();
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        
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

    internal void UpdateStats()
    {
        int timeTaken = Mathf.RoundToInt(Time.time - GameSettings.LevelStartTime);
        int minutes = (timeTaken / 60);
        int sec = (timeTaken % 60);
        string timeString = (minutes>0?(minutes+"m"):"")+sec+"s";


        timeText.text = timeString;
        movesText.text = GameSettings.MoveCounter.ToString();
        stepsText.text = GameSettings.StepsCounter.ToString();
        
        //FIX
        levelText.text = (GameSettings.CurrentLevel+1).ToString();

        SetSelected();
    }
}
