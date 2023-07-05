using TMPro;
using UnityEngine;


public enum DifficultLevel{Easy,Medium,Hard}

public class LevelButton : MonoBehaviour
{
    public int level;
    public DifficultLevel difficulty;
    public TextMeshProUGUI levelIDText;
    public GameObject selectedBorder;

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
