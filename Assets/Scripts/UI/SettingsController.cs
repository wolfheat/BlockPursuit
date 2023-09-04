using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : EscapableBasePanel, IReturnMenuType
{
    [SerializeField] WaterController waterController;
    [SerializeField] SliderSettingsController music;
    [SerializeField] SliderSettingsController SFX;
    [SerializeField] SliderSettingsController brightness;
    [SerializeField] Toggle shakeToggle;
    [SerializeField] Toggle waterToggle;
    [SerializeField] TextMeshProUGUI waterText;
    [SerializeField] ConfirmResetScreen confirmResetScreen;

    public ReturnMenuType ReturnMenu { get; set; } = ReturnMenuType.Main;

    private bool allowedToListenForChanges = true;
    private bool settingsChangedByPlayer = false;

    private const float IntensityScale = 4f;

    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("SettingsController ESC");
        ReturnToMainMenu();
    }

    public void UpdatePanelFromStored()
    {
        // When artificially setting the sliders, make sure it is not registrated as a change by the player which would trigger an update of the save file.
        Listen(false);

        // Opened Settings page place data
        Debug.Log("Setting music and SFX from stored values: "+ SavingUtility.gameSettingsData.soundSettings.MusicVolume+","+ SavingUtility.gameSettingsData.soundSettings.SFXVolume);
        music.SetSlider(SavingUtility.gameSettingsData.soundSettings.MusicVolume);
        music.SliderChange(); // Makes sure text is updated even if 0 is the stored value
        SFX.SetSlider(SavingUtility.gameSettingsData.soundSettings.SFXVolume);
        SFX.SliderChange();
        brightness.SetSlider(SavingUtility.gameSettingsData.lightSettings.LightIntensity/ IntensityScale);
        brightness.SliderChange();
        shakeToggle.isOn = SavingUtility.gameSettingsData.gameEffectsSettings.UseShake;
        waterToggle.isOn = SavingUtility.gameSettingsData.gameEffectsSettings.AnimatedWater;
        SetWaterText();
        waterController.SetAnimatedWater(waterToggle.isOn);
        Listen();
        settingsChangedByPlayer = false; 
    }

    private void SetWaterText()
    {
        waterText.text = waterToggle.isOn ? "Animated Water" : "Simple Water";
    }

    private void Listen(bool listen = true)
    {
        allowedToListenForChanges = listen;
    }

    public void ShakeToggle()
    {
        HandleChangedValues();
        SavingUtility.gameSettingsData.gameEffectsSettings.UseShake = shakeToggle.isOn;
    }
    public void WaterToggle()
    {
        HandleChangedValues();
        SavingUtility.gameSettingsData.gameEffectsSettings.AnimatedWater = waterToggle.isOn;
        SetWaterText();
        waterController.SetAnimatedWater(waterToggle.isOn);

    }
    public void LightSettingsUpdated()
    {
        HandleChangedValues();
        SavingUtility.gameSettingsData.lightSettings.LightIntensity = brightness.SliderValue()* IntensityScale;
        LightController.Instance.SetFromStoredValues();
    }

    private void HandleChangedValues()
    {
        if (!allowedToListenForChanges || !Enabled()) return;
        settingsChangedByPlayer = true;
    }

    public void SoundSettingsUpdated()
    {
        HandleChangedValues();

        UpdateSavingValues();
    }

    public void UpdateSavingValues()
    {
        Debug.Log("Sound Settings have been updated");
        SavingUtility.gameSettingsData.soundSettings.UseMusic = (music.SliderValue() == 0) ? false : true;
        SavingUtility.gameSettingsData.soundSettings.UseSFX = (SFX.SliderValue() == 0) ? false : true;
        SavingUtility.gameSettingsData.soundSettings.MusicVolume = music.SliderValue();
        SavingUtility.gameSettingsData.soundSettings.SFXVolume = SFX.SliderValue();
        SoundController.Instance.SetVolumesFromStoredValues();
    }

    public void OpenResetScreen()
    {
        SaveSettings();
        TransitionScreen.Instance.StartTransition(GameAction.ShowResetConfirm);
    }


    public void ReturnToMainMenu()
    {
        SaveSettings();
        TransitionScreen.Instance.StartTransition(GameAction.HideSettings);
    }
    public void SaveSettings()
    {
        if(settingsChangedByPlayer) 
            SavingUtility.Instance.SaveSettingsDataToFile();
    }

}

public enum ReturnMenuType { Main, InGame }
public interface IReturnMenuType
{    public ReturnMenuType ReturnMenu { get; set; }

}


