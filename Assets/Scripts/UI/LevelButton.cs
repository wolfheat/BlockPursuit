using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI levelIDText;
    public GameObject selectedBorder;

    public void SetLevel(int l)
    {
        level = l;
        levelIDText.text = (l+1).ToString();
    }

    public void RequestStartLevel()
    {
        Debug.Log("Request start level "+level);
    }

    public void Select(bool doSelect = true)
    {
        selectedBorder.SetActive(doSelect);
    }
    
}
