using System;
using TMPro;
using UnityEngine;

public class InfoScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] TextMeshProUGUI levelNameTextB;
    [SerializeField] TextMeshProUGUI unlockCostText;
    [SerializeField] TextMeshProUGUI bestTime;
    [SerializeField] TextMeshProUGUI bestMove;
    [SerializeField] TextMeshProUGUI bestStep;

    [SerializeField] GameObject locked;
    [SerializeField] GameObject normal;

    internal void UpdateInfo(int level, int levelDiff, int unlockCost,LevelDefinition levelDefinition,PlayerLevelData playerLevelDefinition)
    {
        
        if (levelDefinition.unlocked)
        {
            locked.gameObject.SetActive(false);
            normal.gameObject.SetActive(true);
            bestTime.text = StringConverter.TimeAsString(playerLevelDefinition.bestTime);
            bestMove.text = playerLevelDefinition.bestMoves.ToString();
            bestStep.text = playerLevelDefinition.bestSteps.ToString();
            levelNameTextB.text = "Level "+ StringConverter.LevelAsStringWithParantheses(levelDiff,level);
        }
        else
        {
            normal.gameObject.SetActive(false);
            locked.gameObject.SetActive(true);

            levelNameText.text = "Level " + StringConverter.LevelAsStringWithParantheses(levelDiff, level);
            unlockCostText.text = "x" + unlockCost;

        }

    }
}
