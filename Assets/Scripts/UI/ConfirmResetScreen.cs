using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmResetScreen : EscapableBasePanel
{
    [SerializeField] SettingsController settings;

     public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from confirm reset screen");
        CloseMenu();
    }

    public void CloseMenu()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideResetConfirm);
    }
    public void ConfirmResetClicked()
    {
        // Reset Save
        SavingUtility.Instance.ResetSaveFile();

        // Reset Level Select
        FindObjectOfType<LevelSelect>().ResetLevelSelect();
        // Reset Settings Screen   
        // Music Settings
        // Reset Achievements

        CloseMenu();
    }
}
