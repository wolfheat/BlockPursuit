using UnityEngine;

public enum GameState { Menu, RunGame, Transition}
public enum GameAction { LoadSelectedLevel, LoadStartMenu, ShowInventory, HideInventory, ShowLevelSelect ,ShowLevelComplete, RestartLevel, none }

public class GameSettings : MonoBehaviour
{
	public static float ScreenWidth { get; private set; }
	public static float ScreenHeight { get; private set; }
	public static float AspectRatio { get; private set; }
	public static float GameScale { get; private set; }

	// USED
	public static bool UseMusic { get; private set; } = true;
	public static GameAction StoredAction { get; set; } = GameAction.none;	
	public static float LevelStartTime { get; set; }	
	public static int StepsCounter { get; set; }	
	public static int MoveCounter { get; set; }	
	//public static PlayerInventory PlayerInventory { get; set; }	
	public static bool InTransition { get; set; }	

	public static bool UseFast => StoredAction == GameAction.ShowInventory || StoredAction == GameAction.HideInventory;
    //OLD

    public static bool AtMenu { get; set; } = true;
	public static bool CanShoot { get; set; } = true;
	public static GameState CurrentGameState { get; set; } = GameState.Menu;
	
	public static bool IsPaused { get; set; } = true;
	public static int CurrentLevel { get; set; } = 1;
	public static int CurrentDifficultLevel { get; internal set; } = 0;

    [SerializeField] bool useMusicSetting;


	private void Awake()
	{
		// Set Screenheight from reading of ortoghonal camera
		ScreenHeight = Camera.main.orthographicSize*2;
		ScreenWidth = ScreenHeight * ((float)Screen.width/ (float)Screen.height);
		AspectRatio = ScreenWidth/ScreenHeight;
		GameScale = ScreenHeight/ Screen.height;
		//Inputs.Instance.Controls.MainActionMap.X.performed += _ => CanShoot = !CanShoot;
	}

	private void Update()
	{
		if (UseMusic != useMusicSetting)
		{
			UseMusic = useMusicSetting;
			FindObjectOfType<SoundController>().UseMusic(UseMusic);
		}
	}


}
