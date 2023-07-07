using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DifficultLevel{Easy,Medium,Hard}

public class LevelButton : MonoBehaviour, ISelectHandler
{
    public int level;
    public DifficultLevel difficulty;
    public TextMeshProUGUI levelIDText;
    public GameObject selectedBorder;
    public LevelDefinition levelDefinition;
    public PlayerLevelData playerLevelData;

    public void OnSelect(BaseEventData eventData)
    {
        int cost = 0;
        if(levelDefinition!= null)
            cost = levelDefinition.unlockRequirements.Count>0? levelDefinition.unlockRequirements[0].amount : 0;
        Debug.Log("Selecting level button, updating playerdata.time to "+playerLevelData.bestTime);
        FindObjectOfType<InfoScreen>().UpdateInfo(level, (int)difficulty, cost, levelDefinition, playerLevelData);
    }

    public void SetLevel(int l)
    {
        level = l;
        levelIDText.text = (l+1).ToString();
    }

    public void RequestStartLevel()
    {
        FindObjectOfType<LevelSelect>().RequestStartSelectedLevel(levelDefinition);
    }

    public void Select(bool doSelect = true)
    {
        selectedBorder.SetActive(doSelect);
    }
    
}
