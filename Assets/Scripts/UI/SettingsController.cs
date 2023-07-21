using UnityEngine;
using UnityEngine.UI;

public class SettingsController : BasePanel
{
    [SerializeField] SoundSliderSettingsController music;
    [SerializeField] SoundSliderSettingsController SFX;

    public void UpdatePanelFromStored()
    {
        //if(SavingUtility.playerGameData.soundSettings==null)

        // Opened Settings page place data
        Debug.Log("Setting music anf SFX from stored values: "+ SavingUtility.playerGameData.soundSettings.MusicVolume+","+ SavingUtility.playerGameData.soundSettings.SFXVolume);
        music.SetVolumeFromStoredValue(SavingUtility.playerGameData.soundSettings.MusicVolume);
        SFX.SetVolumeFromStoredValue(SavingUtility.playerGameData.soundSettings.SFXVolume);
    }

    public void SoundSettingsUpdated()
    {
        Debug.Log("Sound Settings have been updated");
        SavingUtility.playerGameData.soundSettings.UseMusic = (music.SliderValue()==0) ? false : true;
        SavingUtility.playerGameData.soundSettings.UseSFX = (SFX.SliderValue() == 0) ? false : true;
        SavingUtility.playerGameData.soundSettings.MusicVolume = music.SliderValue();
        SavingUtility.playerGameData.soundSettings.SFXVolume = SFX.SliderValue();
        SoundController.Instance.SetVolumesFromStoredValues();
    }

    public void CloseAndStoreSettings()
    {
        SavingUtility.Instance.SaveToFile();
        HidePanel();
    }

}
