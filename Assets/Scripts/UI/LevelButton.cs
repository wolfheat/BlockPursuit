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
    public PlayerLevelDefinition playerLevelDefinition;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selecteded a button: "+level);
        int cost = levelDefinition.unlockRequirements.Count>0? levelDefinition.unlockRequirements[0].amount : 0;
        FindObjectOfType<InfoScreen>().UpdateInfo(level, (int)difficulty, cost, playerLevelDefinition);
    }

    public void SetLevel(int l)
    {
        level = l;
        levelIDText.text = (l+1).ToString();
    }

    public void RequestStartLevel()
    {
        FindObjectOfType<LevelSelect>().RequestStartSelectedLevel(level, difficulty);
    }

    public void Select(bool doSelect = true)
    {
        selectedBorder.SetActive(doSelect);
    }
    
}
