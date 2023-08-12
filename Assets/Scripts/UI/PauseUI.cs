using TMPro;
using UnityEngine;

public class PauseUI : EscapableBasePanel
{

    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        ClosePauseUIRequest();
    }

    public void ClosePauseUIRequest()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HidePauseScreen);
    }

    public void QuitLevelClicked()
    {
        Debug.Log("Quit Level Clicked"); 
        TransitionScreen.Instance.StartTransition(GameAction.ShowLevelSelect);
    }

}
