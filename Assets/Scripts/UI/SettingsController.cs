using UnityEngine;
using UnityEngine.UI;

public class SettingsController : EscapableBasePanel
{
    [SerializeField] SoundSliderSettingsController music;
    [SerializeField] SoundSliderSettingsController SFX;
    [SerializeField] SoundSliderSettingsController brightness;
    [SerializeField] Toggle shakeToggle;
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
        Debug.Log("Setting music and SFX from stored values: "+ SavingUtility.playerGameData.soundSettings.MusicVolume+","+ SavingUtility.playerGameData.soundSettings.SFXVolume);
        music.SetVolumeFromStoredValue(SavingUtility.playerGameData.soundSettings.MusicVolume);
        SFX.SetVolumeFromStoredValue(SavingUtility.playerGameData.soundSettings.SFXVolume);
        brightness.SetVolumeFromStoredValue(SavingUtility.playerGameData.lightSettings.LightIntensity/ IntensityScale);
        shakeToggle.isOn = SavingUtility.playerGameData.gameEffectsSettings.UseShake;

        Listen();
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

        SavingUtility.playerGameData.gameEffectsSettings.UseShake = shakeToggle.isOn;
    }
    public void LightSettingsUpdated()
    {
        if (!Enabled())
        {
            Debug.Log("LightSettingsUpdated, but settings panel is not active");
            return;
        }
        Debug.Log("Light Settings have been updated");
        SavingUtility.playerGameData.lightSettings.LightIntensity = brightness.SliderValue()* IntensityScale;
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
        SavingUtility.playerGameData.soundSettings.UseMusic = (music.SliderValue() == 0) ? false : true;
        SavingUtility.playerGameData.soundSettings.UseSFX = (SFX.SliderValue() == 0) ? false : true;
        SavingUtility.playerGameData.soundSettings.MusicVolume = music.SliderValue();
        SavingUtility.playerGameData.soundSettings.SFXVolume = SFX.SliderValue();
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

