using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : EscapableBasePanel
{
    [SerializeField] WaterController waterController;
    [SerializeField] SoundSliderSettingsController music;
    [SerializeField] SoundSliderSettingsController SFX;
    [SerializeField] SoundSliderSettingsController brightness;
    [SerializeField] Toggle shakeToggle;
    [SerializeField] Toggle waterToggle;
    [SerializeField] TextMeshProUGUI waterText;
    [SerializeField] ConfirmResetScreen confirmResetScreen;

    private bool listenForSoundSliders = true;

    private const float IntensityScale = 4f;

    public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("SettingsController ESC");
        CloseAndStoreSettings();
    }

    public void UpdatePanelFromStored()
    {
        // When artificially setting the sliders, make sure it is not registrated as a change by the player wich would trigger an update of the save file.
        Listen(false);

        // Opened Settings page place data
        Debug.Log("Setting music and SFX from stored values: "+ SavingUtility.gameSettingsData.soundSettings.MusicVolume+","+ SavingUtility.gameSettingsData.soundSettings.SFXVolume);
        music.SetVolumeFromStoredValue(SavingUtility.gameSettingsData.soundSettings.MusicVolume);
        SFX.SetVolumeFromStoredValue(SavingUtility.gameSettingsData.soundSettings.SFXVolume);
        brightness.SetVolumeFromStoredValue(SavingUtility.gameSettingsData.lightSettings.LightIntensity/ IntensityScale);
        shakeToggle.isOn = SavingUtility.gameSettingsData.gameEffectsSettings.UseShake;
        waterToggle.isOn = SavingUtility.gameSettingsData.gameEffectsSettings.AnimatedWater;
        SetWaterText();
        waterController.SetAnimatedWater(waterToggle.isOn);
        Listen();
    }

    private void SetWaterText()
    {
        waterText.text = waterToggle.isOn ? "Animated Water" : "Simple Water";
    }

    private void Listen(bool listen = true)
    {
        listenForSoundSliders = listen;
        music.listenForChange = listen;
        SFX.listenForChange = listen;
        brightness.listenForChange = listen;
    }

    public void ShakeToggle()
    {
        if (!Enabled())
            return;

        Debug.Log("Shake Settings have been changed");

        SavingUtility.gameSettingsData.gameEffectsSettings.UseShake = shakeToggle.isOn;
    }
    public void WaterToggle()
    {
        if (!Enabled())
            return;

        Debug.Log("Water Settings have been changed");

        SavingUtility.gameSettingsData.gameEffectsSettings.AnimatedWater = waterToggle.isOn;
        SetWaterText();
        waterController.SetAnimatedWater(waterToggle.isOn);

    }
    public void LightSettingsUpdated()
    {
        if (!Enabled())
        {
            Debug.Log("LightSettingsUpdated, but settings panel is not active");
            return;
        }
        Debug.Log("Light Settings have been updated");
        SavingUtility.gameSettingsData.lightSettings.LightIntensity = brightness.SliderValue()* IntensityScale;
        LightController.Instance.SetFromStoredValues();
    }
    public void SoundSettingsUpdated()
    {
        if (!Enabled())
        {
            Debug.Log("SoundSettingsUpdated, but settings panel is not active");
            return;
        }
        if (!listenForSoundSliders) return;

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
        TransitionScreen.Instance.StartTransition(GameAction.ShowResetConfirm);
    }

    public ReturnMenuType ReturnMenu { get; set; } = ReturnMenuType.Main;

    public void CloseAndStoreSettings()
    {
        SavingUtility.Instance.SaveToFile();
        TransitionScreen.Instance.StartTransition(GameAction.HideSettings);
    }

}
public enum ReturnMenuType {Main,InGame}

