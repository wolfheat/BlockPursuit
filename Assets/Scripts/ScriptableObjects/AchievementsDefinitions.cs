using System;
using UnityEngine;

public enum UnlockRequirementType{CompleteTier, GainTotalGold}

[CreateAssetMenu(fileName = "AchievementsDefinition", menuName = "New AchievementsDefinition")]
public class AchievementsDefinitions : ScriptableObject
{
    public AchievementDefinition[] definitions;

    public void AddToArray(int amt)
    {
        AchievementDefinition[] oldDefinitions = definitions;
        definitions = new AchievementDefinition[definitions.Length+amt];
        for (int i = 0; i < definitions.Length; i++)
            definitions[i] = oldDefinitions[i];
    }
}

[Serializable]
public class AchievementDefinition
{
    public Sprite completedSprite;
    public string achievementName;
    public string description;
    public UnlockRequirementType type;
    public int value;
}
