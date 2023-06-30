using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] StartMenu startMenu;
    [SerializeField] LevelComplete levelComplete;
    [SerializeField] TransitionScreen transitionScreen;
    [SerializeField] LevelCreator levelCreator;


    private void OnEnable()
    {
        transitionScreen.GameDarkEvent += DoStoredAction;        
    }
    private void OnDisable()
    {
        transitionScreen.GameDarkEvent -= DoStoredAction;
    }

    private void Start()
    {
        Debug.Log("toggle on Start Menu");
        startMenu.ShowPanel();
    }
    
    internal void DoStoredAction()
    {
        switch (GameSettings.StoredAction)
        {
            case GameAction.LoadNextLevel:
                levelComplete.HidePanel();
                startMenu.HidePanel();
                levelCreator.LoadNextLevel();
                GameSettings.LevelStartTime = Time.time;
                GameSettings.MoveCounter = 0;
                GameSettings.StepsCounter = 0;
                break;
            case GameAction.LoadStartMenu:
                levelComplete.HidePanel();
                startMenu.ShowPanel();
                break;
            case GameAction.ShowLevelComplete:
                levelCreator.ClearLevel();
                levelComplete.ShowPanel();
                levelComplete.UpdateStats();

                break;
            case GameAction.none:
                break;
            default:
                break;
        }
    }
}
