using UnityEngine;
public class AchievementsController : EscapableBasePanel
{
    [SerializeField] private AchievementGraphics[] achieventGraphics;
    private AchievementData achievements;

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

    private void OnEnable()
    {
        SavingUtility.LoadingComplete += SetAchievementsFromData;
        PlayerGameData.AchievementUnlocked += SetAchievementsFromData;
    }
    
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= SetAchievementsFromData;
        PlayerGameData.AchievementUnlocked -= SetAchievementsFromData;
    }

    private void SetAchievementsFromData()
    {
        Debug.Log(" -- Load Achievements values from file -- ");
        achievements = SavingUtility.playerGameData.AchievementData;

        if (achievements == null)
        {
            Debug.Log("Achievements null");
            // This handles initiation of the array
            // (This should never happen for players since if there is no save file yet the entire save is initialized in saveutility catch)
            achievements = new AchievementData() { Data = new bool[achieventGraphics.Length] };
            SavingUtility.playerGameData.AchievementData = achievements;
        }
        else
        {
            Debug.Log("Achievements not null");
        }

        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        for (int i = 0; i < achieventGraphics.Length; i++)
        {
            if (i > achievements.Data.Length) break;
            achieventGraphics[i].SetCompleted(achievements.Data[i]);
        }
    }
}
