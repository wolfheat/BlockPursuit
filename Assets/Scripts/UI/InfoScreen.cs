using System;
using TMPro;
using UnityEngine;

public class InfoScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] TextMeshProUGUI unlockCostText;
    [SerializeField] TextMeshProUGUI bestTime;
    internal void UpdateInfo(int levelID, int levelDiff, int unlockCost,PlayerLevelDefinition playerLevelDefinition)
    {
        char level = (char)(levelDiff+'A');
        levelNameText.text = "Level (" + level+"." +(levelID + 1)+")";
        unlockCostText.text = "x" + unlockCost;

        if (playerLevelDefinition.levelID != -1)
        {
            bestTime.text = "Best: " + playerLevelDefinition.bestTime;
        }

    }
}
