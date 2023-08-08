using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ConfirmResetScreen : BasePanel
{
    [SerializeField] SettingsController settings;

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.started += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.started -= RequestESC;
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("ESC from confirm reset screen");
        CancelClicked();
    }

    public void CancelClicked(bool updateSave = false)
    {
        /*
        HidePanel();
        settings.ShowPanel();
        settings.UpdatePanelFromStored();
        if (updateSave) settings.UpdateSavingValues();
        */
        StartRequest();
    }
    public void StartRequest()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideResetConfirm);
    }
    public void OkClicked()
    {
        // Reset Save
        SavingUtility.Instance.ResetSaveFile();

        // Reset Level Select
        FindObjectOfType<LevelSelect>().ResetLevelSelect();
        // Reset Settings Screen   
        // Music Settings
        // Reset Achievements




        CancelClicked(true);
    }
}
