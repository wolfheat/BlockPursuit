using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class UnlockScreen : BasePanel
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject okButton;
    [SerializeField] LevelSelect levelSelect;

    private LevelDefinition currentLevel;

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.performed += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.performed -= RequestESC;
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        CancelClicked();
    }


    public void SetInfo(LevelDefinition level)
    {
        currentLevel = level;
        levelText.text = "to unlock level "+ StringConverter.LevelAsString(level.LevelDiff,level.LevelIndex)+"?";
        costText.text = "x"+ (level.unlockRequirements.Count==0?0:level.unlockRequirements[0].amount);
        EventSystem.current.SetSelectedGameObject(okButton);
    }

    public void CancelClicked()
    {
        HidePanel();
        levelSelect.ShowPanel();
        levelSelect.SetSelected();
    }
    
    public void OkClicked()
    {
        if(currentLevel.unlockRequirements.Count == 0)
        {
            Debug.Log("No requirements found");
        }
        else if(SavingUtility.playerGameData.Tiles < currentLevel.unlockRequirements[0].amount)
        {
            Debug.Log("You only got "+ SavingUtility.playerGameData.Tiles+" but update costs is "+ currentLevel.unlockRequirements[0].amount+", you cant afford the update!");
            return;
        }
        else
        {
            //Pay fee and unlock level
            SavingUtility.playerGameData.RemoveTiles(currentLevel.unlockRequirements[0].amount);
        }

        CancelClicked();
        //Maybe show animation lock removed?
        levelSelect.Unlock();
    }
}
