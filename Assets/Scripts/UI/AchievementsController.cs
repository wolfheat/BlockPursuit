using System.Collections.Generic;
using UnityEngine;
public class AchievementsController : EscapableBasePanel
{
    [SerializeField] private AchievementGraphics achieventGraphicsPrefab;
    [SerializeField] private AchievementsDefinitions achievementsDefinitions;
    [SerializeField] private GameObject achievementsHolder;
    private List<AchievementGraphics> achieventGraphics = new List<AchievementGraphics>();

    private AchievementData achievements;

    public AchievementDefinition GetAchievementDefinition(int index)
    {
        return achievementsDefinitions.definitions[index];
    } 

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
        PlayerGameData.AchievementUnlocked += UnlockAchievement;
    }
    
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= SetAchievementsFromData;
        PlayerGameData.AchievementUnlocked -= UnlockAchievement;
    }
    private void Start()
    {
        // Generate achieventGraphicss
        foreach (var achievement in achievementsDefinitions.definitions)
        {
            AchievementGraphics newGraphics = Instantiate(achieventGraphicsPrefab, achievementsHolder.transform);
            newGraphics.descriptionText.text = achievement.description;
            newGraphics.completedImage.sprite = achievement.completedSprite;
            achieventGraphics.Add(newGraphics);
        }
    }
    private void UnlockAchievement(int index)
    {
        Debug.Log("UNLOCKING INDEX "+index);
        achievements.Data[index] = true;
        UpdateGraphic(achieventGraphics[index], achievements.Data[index]);
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
            achievements = new AchievementData() { Data = new bool[achieventGraphics.Count] };
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
        for (int i = 0; i < achieventGraphics.Count; i++)
        {
            if (i >= achievements.Data.Length) break;
            UpdateGraphic(achieventGraphics[i], achievements.Data[i]);
        }
    }
    private void UpdateGraphic(AchievementGraphics graphics, bool set) => graphics.SetCompleted(set);        

}
