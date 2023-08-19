using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MusicType{Normal, Foresty, Dungeony, Spacy}
public enum AmbientType{Ocean, Forest, Dungeon, Space}

public enum SFX { ShipDestroyedA, GetHit, PlayerDeath, MenuStep, MenuSelect, MenuError, FireRocket, FireBullet, StarPickup, Unplacable, PlacedTile, NoStep, TakeStep, Unlock, GainCoin, RecieveBoost,
    FootStep,
    LevelComplete,
    MenuCancel,
    PickupTile
}
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] menu;
    [SerializeField] private AudioClip[] levelComplete;
    [SerializeField] private AudioClip[] placeTile;
    [SerializeField] private AudioClip[] unPlaceableTile;
    [SerializeField] private AudioClip[] pickTile;
    [SerializeField] private AudioClip[] moves;
    [SerializeField] private AudioClip[] cantMove;
    [SerializeField] private AudioClip[] unlock;
    [SerializeField] private AudioClip[] boost;
    [SerializeField] private AudioClip[] coin;
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] ambient;
    [SerializeField] private AudioClip[] music;

    private AudioSource musicSource;
    private AudioSource ambientSource;
    private AudioSource sfxSource;
    private bool doPlayMusic = false;
    private bool doPlaySFX = true;
    private bool doingFadeout = false;

    public MusicType activeMusic = MusicType.Normal;
    public static SoundController Instance { get; set; }

	private float presetVolume = 1.0f;
    //private float presetSFXVolume = 0.1f;
    private float presetSFXStepVolume = 0.5f;
    private float presetSFXVolume = 1.0f;

    private float totalFadeOutTime = 3.5f;
    private float fadeOutMargin = 0.01f;
    private float currentFadeOutTime;
    private const float FadeTime = 1f;

    private void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); return;}
        else Instance = this;

        GameObject musicSourceHolder = new GameObject("Music");
        GameObject ambientSourceHolder = new GameObject("Ambient");
        GameObject sfxSourceHolder = new GameObject("SFX");
        musicSourceHolder.transform.SetParent(this.transform);
        ambientSourceHolder.transform.SetParent(this.transform);
        sfxSourceHolder.transform.SetParent(this.transform);

        musicSource = musicSourceHolder.AddComponent<AudioSource>();
        ambientSource = ambientSourceHolder.AddComponent<AudioSource>();
        sfxSource = sfxSourceHolder.AddComponent<AudioSource>();

        SavingUtility.LoadingComplete += SetVolumesFromStoredValues; 
    }
    private void Start()
    {
        musicSource.loop = true;
        ambientSource.loop = true;
	}

    public void SetVolumesFromStoredValues()
    {
        doPlayMusic = SavingUtility.gameSettingsData.soundSettings.UseMusic;  
        doPlaySFX = SavingUtility.gameSettingsData.soundSettings.UseSFX;
        presetVolume = SavingUtility.gameSettingsData.soundSettings.MusicVolume;

        if (presetSFXVolume != SavingUtility.gameSettingsData.soundSettings.SFXVolume) PlaySFXChangedVolume();

        presetSFXVolume = SavingUtility.gameSettingsData.soundSettings.SFXVolume;
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

        DoMusicSetting();
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
        if ((int)activeMusic >= music.Length) { 
            Debug.LogWarning("To few Music files assigned to SoundController"); 
            return; 
        }

        if (!doPlayMusic)
        {
            Debug.Log("Do not play music.");
            musicSource.Stop();
            return;
        }

        if (musicSource.isPlaying)
        {
            Debug.Log("music is playing already");
            if (musicSource.clip == music[(int)activeMusic])
            {
                Debug.Log("asked to play music but requested music is already playing");
                return;
            }
            Debug.Log("music is playing but request for new type, do fade");
            StartCoroutine(FadeMusic());
            return;
        }
        // Assign the clip
        musicSource.clip = music[(int)activeMusic];

        Debug.Log("music is not playing, play");    
        musicSource.Play();
    }

	public void SetVolume(float vol)
	{
        musicSource.volume = vol;
	}
	public void SetSFXVolume(float vol)
	{
        sfxSource.volume = vol;
        sfxSource.volume = presetSFXStepVolume;
	}

    private IEnumerator FadeMusic()
    {
        // fade out into new clip
        float startVolume = musicSource.volume;
        float timer = 0;

        while (musicSource.volume > 0.05f)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume,0,timer/FadeTime);
            yield return null;
        }

        // Assign the clip
        //musicSource.Stop();
        musicSource.clip = music[(int)activeMusic];
        musicSource.Play(); 

        while (musicSource.volume < startVolume)
        {
            timer -= Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume,0,timer/FadeTime);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
    public void StopAmbient()
    {
        ambientSource.Stop();
    }
    public void PlayAmbient(AmbientType type)
    {
        if (!doPlaySFX || ambient.Length==0) return;

        switch (type)
        {
            case AmbientType.Ocean:
                ambientSource.clip = ambient[0];
                break;
            case AmbientType.Forest:
                ambientSource.clip = ambient[1];
                break;
            case AmbientType.Dungeon:
                ambientSource.clip = ambient[2];
                break;
            case AmbientType.Space:
                ambientSource.clip = ambient[3];
                break;
            default:
                Debug.LogWarning("ambient Sound effect not found, playing default");
                ambientSource.clip = ambient[0];
                break;
        }
        ambientSource.Play();

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
			case SFX.MenuCancel:
                sfxSource.PlayOneShot(menu[3]);
                break;
			case SFX.PlacedTile:
                sfxSource.PlayOneShot(placeTile[0]);
                break;
			case SFX.PickupTile:
                sfxSource.PlayOneShot(pickTile[0]);
                break;
			case SFX.Unplacable:
                sfxSource.PlayOneShot(unPlaceableTile[0]);
                break;
			case SFX.TakeStep:
                sfxSource.PlayOneShot(moves[0]);
                break;
			case SFX.NoStep:
                //sfxSource.PlayOneShot(cantMove[0]);
                break;
			case SFX.Unlock:
                sfxSource.PlayOneShot(unlock[0]);
                break;
			case SFX.LevelComplete:
                sfxSource.PlayOneShot(levelComplete[0]);
                break;
			case SFX.GainCoin:
                sfxSource.PlayOneShot(coin[0]);
                break;
			case SFX.RecieveBoost:
                sfxSource.PlayOneShot(boost[0]);
                break;
			case SFX.FootStep:
                int randomSound = Random.Range(0,footsteps.Length);
                sfxSource.PlayOneShot(footsteps[randomSound]);
                break;
			default:
				break;
		}
	}
}
