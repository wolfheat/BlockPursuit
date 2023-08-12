using UnityEngine;

public class CreditsController : EscapableBasePanel
{

     public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("Credits ESC");
        CloseCredits();
    }

    public void CloseCredits()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideCredits);
    }
}
