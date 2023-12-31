using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnlockScreen : EscapableBasePanel
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject okButton;
    [SerializeField] LevelSelect levelSelect;

    private LevelDefinition currentLevel;

    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from menu");
        CancelClicked();
    }

    public void SetInfo(LevelDefinition level)
    {
        currentLevel = level;
        levelText.text = "to unlock level "+ StringConverter.LevelAsString(level.LevelDiff,level.LevelIndex)+"?";
        costText.text = "x"+ level.unlockRequirements;
        EventSystem.current.SetSelectedGameObject(okButton);
    }

    public void CancelClicked()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideUnlock);
    }
    
    public void OkClicked()
    {
        if(currentLevel.unlockRequirements == 0)
        {
            Debug.Log("No requirements found");
            SoundController.Instance.PlaySFX(SFX.Unlock);
        }
        else if(SavingUtility.playerGameData.Tiles < currentLevel.unlockRequirements)
        {
            Debug.Log("You only got "+ SavingUtility.playerGameData.Tiles+" but update costs is "+ currentLevel.unlockRequirements + ", you cant afford the update!");
            return;
        }
        else
        {
            //Pay fee and unlock level[0].amount
            SavingUtility.playerGameData.RemoveTiles(currentLevel.unlockRequirements);
            SoundController.Instance.PlaySFX(SFX.Unlock);
        }

        CancelClicked();
        //Maybe show animation lock removed?
        levelSelect.Unlock();
    }
}
