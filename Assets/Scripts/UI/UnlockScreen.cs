using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
        levelText.text = StringConverter.LevelAsString(level.LevelDiff,level.LevelIndex);
        costText.text = "x"+level.unlockRequirements[0].amount.ToString();
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
        Debug.Log("Pay " + currentLevel.unlockRequirements[0].amount +" tiles to unlock level "+currentLevel.levelID);
        if(SavingUtility.playerGameData.Tiles < currentLevel.unlockRequirements[0].amount)
            Debug.Log("You only got "+ SavingUtility.playerGameData.Tiles+" but update costs "+ currentLevel.unlockRequirements[0].amount+" you cant afford");
            // Show popup here cant afford?
        else
        {
            SavingUtility.playerGameData.Tiles -= currentLevel.unlockRequirements[0].amount;
            CancelClicked();
            //Maybe show animation lock removed?
            levelSelect.Unlock();

        }


    }

}
