using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestartPanelController : BasePanel
{

    [SerializeField] Button mainSelectedButton;


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
        TransitionScreen.Instance.StartTransition(GameAction.RestartLevel);
    }
}
