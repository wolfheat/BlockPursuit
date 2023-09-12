using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DifficultLevel{Easy,Medium,Hard,ABC}

public class LevelButton : MonoBehaviour, ISelectHandler
{
    public int level = 0;
    public DifficultLevel difficulty;
    public TextMeshProUGUI levelIDText;
    public LevelDefinition levelDefinition;
    public PlayerLevelData playerLevelData;
    [SerializeField] GameObject checkmark;
    [SerializeField] GameObject lockObject;

    public bool IsCompleted => checkmark.activeSelf;

    public static Action<LevelButton> SelectButton;
    public static Action<LevelButton> ClickedButton;

    public void OnSelect(BaseEventData eventData)
    {
        SelectButton.Invoke(this);        
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
        
        levelDefinition.unlocked = true;
        
        PlayerLevelData bestLevelData = SavingUtility.playerGameData.PlayerLevelDataList.AddNewOrRetrieveLevel(levelDefinition.levelID);

        playerLevelData = bestLevelData;

    }

    public void ClickingButton()
    {
        // Checks if button was "clicked" by keyboard rather than mouse
        if (Inputs.Instance.Controls.UI.Submit.WasPressedThisFrame())
            ClickedButton.Invoke(this);
    }

}
