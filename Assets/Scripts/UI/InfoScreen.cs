using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

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

    LevelSelect levelSelect;
    LevelDefinition latestLevelSelected;
    LevelDefinition latestPlayerLevelData;

    LevelButton latestButton;

    private void Start()
    {
        levelSelect = FindObjectOfType<LevelSelect>();
    }

    internal void UpdateInfo(LevelButton button)
    {
        latestButton = button;
        
        if (button.levelDefinition.unlocked)
        {
            locked.gameObject.SetActive(false);
            normal.gameObject.SetActive(true);
            if(button.playerLevelData.bestTime == -1)
            {
                bestTime.text = "-";
                bestMove.text = "-";
                bestStep.text = "-";
            }
            else
            {
                bestTime.text = StringConverter.TimeAsString(button.playerLevelData.bestTime);
                bestMove.text = button.playerLevelData.bestMoves.ToString();
                bestStep.text = button.playerLevelData.bestSteps.ToString();
            }

            levelNameTextB.text = "Level "+ StringConverter.LevelAsStringWithParantheses(button.levelDefinition.LevelDiff, button.levelDefinition.LevelIndex);
        }
        else
        {
            normal.gameObject.SetActive(false);
            locked.gameObject.SetActive(true);

            levelNameText.text = "Level " + StringConverter.LevelAsStringWithParantheses(button.levelDefinition.LevelDiff, button.levelDefinition.LevelIndex);

            unlockCostText.text = "x" + (button.levelDefinition.unlockRequirements.Count > 0 ? button.levelDefinition.unlockRequirements[0].amount:0);

        }

    }

    public void UnLock()
    {
        latestButton.levelDefinition.unlocked = true;

        //Add Data into SaveFile
        PlayerLevelData bestLevelData = SavingUtility.playerGameData.PlayerLevelDataList.AddNewLevel(latestButton.levelDefinition.levelID);

        latestButton.playerLevelData = bestLevelData;

        UpdateInfo(latestButton);
        EventSystem.current.SetSelectedGameObject(latestButton.gameObject);
    }

}
