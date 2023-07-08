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
    [SerializeField] GameObject checkmark;
    [SerializeField] GameObject lockObject;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selecting level button, updating playerdata.time to "+playerLevelData.bestTime);
        FindObjectOfType<LevelSelect>().UpdateLatestSelectedInfo(this);
    }

    public void SetLevel(int l)
    {
        level = l;
        levelIDText.text = (l+1).ToString();
    }
    
    public void ShowCheckmark()
    {
        checkmark.SetActive(true);
    }
    
    public void Unlock()
    {
        lockObject.SetActive(false);
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
