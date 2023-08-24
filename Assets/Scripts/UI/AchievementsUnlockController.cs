using UnityEngine;

public class AchievementsUnlockController : MonoBehaviour
{
    private bool[] achievements;
    //[SerializeField] private UnlockRequirement[] requirements;// = new UnlockRequirementData[0];
    [SerializeField] private AchievementsDefinitions requirements;// = new UnlockRequirementData[0];

    private void OnEnable()
    {
        PlayerGameData.InventoryUpdate += CheckForUnlock;
    }

    public void CheckForUnlock()
    {
        achievements = SavingUtility.playerGameData.AchievementData.Data;
        Debug.Log("Checking to unlock Achievement, amount of requirements: "+requirements.definitions.Length);
        Debug.Log(" * * * Current Total Gold gained = " + SavingUtility.playerGameData.TotalGoldCollected);

        for (int i = 0; i < achievements.Length; i++)
        {
            if (!achievements[i]) // Achievement not yet completed
            {
                if(i >= requirements.definitions.Length)
                    continue;

                bool unlock = CheckForUnlockOfIndex(i);
                if (unlock)
                {
                    Debug.Log("Achievement "+ i + " has been unlocked, set saved data (and update visuals)");
                    SavingUtility.playerGameData.UnlockAchievement(i);
                }
            }
        }

    }

    private bool CheckForUnlockOfIndex(int i)
    {
        UnlockRequirementType type = requirements.definitions[i].type;

        //Debug.Log("Checking to unlock achievement index: "+i);

        int value = requirements.definitions[i].value;

        switch (type)
        {
            case UnlockRequirementType.CompleteTier:
                return(SavingUtility.playerGameData.PlayerLevelDataList.CheckTierCompleted(value));                
            case UnlockRequirementType.GainTotalGold:
                return(SavingUtility.playerGameData.TotalGoldCollected > value);
        }

        return false;
    }
}
