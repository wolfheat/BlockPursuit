using System;
using UnityEngine;

public enum MusicType{Menu,Normal,Boss}

public enum SFX { ShipDestroyedA, GetHit, PlayerDeath, MenuStep, MenuSelect, MenuError, FireRocket, FireBullet, StarPickup, Unplacable, PlacedTile, NoStep, TakeStep, Unlock, GainCoin, RecieveBoost}
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] menu;
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] moves;
    [SerializeField] private AudioClip[] unlock;
    [SerializeField] private AudioClip[] boost;
    [SerializeField] private AudioClip[] coin;
    [SerializeField] private AudioClip[] music;

    private AudioSource musicSource;
    private AudioSource musicSourceIntense;
    private AudioSource sfxSource;
    private bool doPlayMusic = false;
    private bool doPlaySFX = true;
    private bool doingFadeout = false;

    public MusicType activeMusic = MusicType.Menu;
    public static SoundController Instance { get; set; }

	private float presetVolume = 1.0f;
    //private float presetSFXVolume = 0.1f;
    private float presetSFXStepVolume = 0.5f;
    private float presetSFXVolume = 1.0f;

    private float totalFadeOutTime = 3.5f;
    private float fadeOutMargin = 0.01f;
    private float currentFadeOutTime;

    private void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); return;}
        else Instance = this;

        GameObject musicSourceHolder = new GameObject("Music");
        GameObject sfxSourceHolder = new GameObject("SFX");
        musicSourceHolder.transform.SetParent(this.transform);
		//musicSourceHolder.name = "Music";
        sfxSourceHolder.transform.SetParent(this.transform);
        //sfxSourceHolder.name = "SFX";

        musicSourceIntense = gameObject.AddComponent<AudioSource>();
        musicSource = musicSourceHolder.AddComponent<AudioSource>();
        sfxSource = sfxSourceHolder.AddComponent<AudioSource>();

        SavingUtility.LoadingComplete += SetVolumesFromStoredValues; 
    }
    private void Start()
    {
        musicSourceIntense.loop = true;
        musicSourceIntense.volume = 0.5f;
        musicSource.loop = true;
	}

    public void SetVolumesFromStoredValues()
    {
        doPlayMusic = SavingUtility.playerGameData.soundSettings.UseMusic;  
        doPlaySFX = SavingUtility.playerGameData.soundSettings.UseSFX;
        presetVolume = SavingUtility.playerGameData.soundSettings.MusicVolume;

        if (presetSFXVolume != SavingUtility.playerGameData.soundSettings.SFXVolume) PlaySFXChangedVolume();
        presetSFXVolume = SavingUtility.playerGameData.soundSettings.SFXVolume;
        UpdateSoundSettings();
    }

    private void PlaySFXChangedVolume()
    {
        PlaySFX(SFX.GainCoin,false);
    }

    private void UpdateSoundSettings()
    {
        musicSource.volume = presetVolume;
        sfxSource.volume = presetSFXVolume;

        if (doPlayMusic ) PlayMusic();
    }

    public void SetMusicType(MusicType t)
    {
        activeMusic = t;
        DoMusicSetting();
    }
    public void UseMusic(bool use)
    {
        doPlayMusic = use;
        DoMusicSetting();
	}
    private void MuteToggle()
    {
		doPlayMusic = !doPlayMusic;
        DoMusicSetting();
	}
    private void DoMusicSetting()
    {
		if (doPlayMusic) PlayMusic();
		else
		{
			musicSource.Stop();
		}
    }

	private void Update()
    {
        if(doingFadeout) DoFadeout();
    }

	private void DoFadeout()
	{
        currentFadeOutTime += Time.deltaTime;

        float newVolume = presetVolume*(1 - currentFadeOutTime / totalFadeOutTime);
        musicSourceIntense.volume = newVolume;    
        if (currentFadeOutTime + fadeOutMargin >= totalFadeOutTime)
        {
            //Fadeout complete
            musicSourceIntense.volume = presetVolume;
            musicSourceIntense.Stop();
            doingFadeout = false;
        }
    }

	public void SetVolume(float vol)
	{
        musicSource.volume = vol;
        musicSourceIntense.volume = vol;
	}
	public void SetSFXVolume(float vol)
	{
        sfxSource.volume = vol;
        sfxSource.volume = presetSFXStepVolume;
	}
	private void PlayMusic()
	{
        if (doPlayMusic)
        {
            if (musicSource.isPlaying) return;

            if ((int)activeMusic >= music.Length) { Debug.LogWarning("To few Music files assigned to SoundController"); return;}
            musicSource.clip = music[(int)activeMusic];
            musicSource.Play();
        }
        else musicSource.Stop(); 
	}

    public void StopSFX()
    {
        sfxSource.Stop();
    }
    public void PlaySFX(SFX type, bool playMulti=true)
	{
        // If not able to play multiple sounds exit if already playing
        if (!playMulti) if (sfxSource.isPlaying) return;

        if (!doPlaySFX) return;


        switch (type)
		{
            case SFX.MenuStep:
                sfxSource.PlayOneShot(menu[0]);
                break;
			case SFX.MenuSelect:
                sfxSource.PlayOneShot(menu[1]);
                break;
			case SFX.MenuError:
                sfxSource.PlayOneShot(menu[2]);
                break;
			case SFX.TakeStep:
                //sfxSource.PlayOneShot(moves[0]);
                break;
			case SFX.PlacedTile:
                sfxSource.PlayOneShot(moves[1]);
                break;
			case SFX.NoStep:
                sfxSource.PlayOneShot(moves[2]);
                break;
			case SFX.Unplacable:
                sfxSource.PlayOneShot(moves[3]);
                break;
			case SFX.Unlock:
                sfxSource.PlayOneShot(unlock[0]);
                break;
			case SFX.GainCoin:
                sfxSource.PlayOneShot(coin[0]);
                break;
			case SFX.RecieveBoost:
                sfxSource.PlayOneShot(boost[0]);
                break;
			default:
				break;
		}
	}


}
