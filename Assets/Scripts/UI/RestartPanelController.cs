using System.Collections;
using UnityEngine;

public class RestartPanelController : EscapableBasePanel
{
    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        ClosePanel();
    }
    public void ClosePanel()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideRestartMenu);
    }
    public void RestartLevel()
    {
        HidePanel();

        if (GameSettings.InTransition) return;
        TransitionScreen.Instance.StartTransition(GameAction.RestartLevel);
    }
}
