using UnityEngine;

public class AchievementsController : EscapableBasePanel
{
    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("Achievements ESC");
        CloseAchievements();
    }
    public void CloseAchievements()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideAchievements);
    }


}
