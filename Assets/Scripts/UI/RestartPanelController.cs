using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestartPanelController : BasePanel
{
    TransitionScreen transitionScreen;

    [SerializeField] Button mainSelectedButton;

    private void Start()
    {
        transitionScreen = FindObjectOfType<TransitionScreen>();
    }


    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void ClosePanel()
    {
        HidePanel();
        GameSettings.IsPaused = false;
    }
    public void RestartLevel()
    {
        HidePanel();

        if (GameSettings.InTransition) return;
        GameSettings.StoredAction = GameAction.RestartLevel;
        transitionScreen.StartTransition();
    }
}
