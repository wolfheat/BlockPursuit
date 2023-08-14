using System;

[Serializable]
public class LightSettings
{
    public float LightIntensity { get; set; } = 1;
}

[Serializable]
public class GameEffectsSettings
{
    public bool UseShake { get; set; } = true;
}

[Serializable]
public class SoundSettings
{
    public bool UseMusic { get; set; } = true;
    public float MusicVolume { get; set; } = 0.4f;
    public bool UseSFX { get; set; } = true;
    public float SFXVolume { get; set; } = 0.4f;
}
[Serializable]
public class PlayerGameData
{
    // Players Inventory
    public int Tiles { get; set; } = 0;
    public int Coins { get; set; } = 100;
    public DateTime AtypeBoostTime { get; set; }
    public DateTime BtypeBoostTime { get; set; } // Having these private set wont let the load method write these values
        
    // Player Levels Data
    public PlayerLevelDataList PlayerLevelDataList { get; private set; }

    // Player Settings
    public AvatarType Avatar { get; set; } = AvatarType.Dino;


    // General Game Settings
    public int ActiveTouchControl { get; set; } // Having these private set wont let the load method write these values
    public int CameraPos { get; set; } // Having these private set wont let the load method write these values

    public SoundSettings soundSettings = new SoundSettings();
    public LightSettings lightSettings = new LightSettings();
    public GameEffectsSettings gameEffectsSettings = new GameEffectsSettings();


    // Action Events
    public static Action InventoryUpdate;
    public static Action BoostTimeUpdated;
    public static Action AvatarChange;
    public static Action InputSettingUpdate;

    public PlayerGameData()
    {
        Tiles = 0;
        Coins = 100;
        PlayerLevelDataList = new PlayerLevelDataList();
    }

    public static void InvokeAll()
    {
        InventoryUpdate?.Invoke();
        BoostTimeUpdated?.Invoke();
    }

    public void SetABoostTime(DateTime time)
    {
        AtypeBoostTime = time;
        BoostTimeUpdated.Invoke();
    }
    public void SetBBoostTime(DateTime time)
    {
        BtypeBoostTime = time;
        BoostTimeUpdated.Invoke();
    }
    public void AddCoins(int amt)
    {
        Coins += amt;
        InventoryUpdate.Invoke();
    }

    public bool RemoveCoins(int amt)
    {
        if(Coins < amt) return false;
        Coins -= amt;
        InventoryUpdate.Invoke();
        return true;
    }
    public void AddTiles(int amt)
    {
        Tiles += amt;
        InventoryUpdate.Invoke();
    }

    public bool RemoveTiles(int amt)
    {
        if(Tiles < amt) return false;
        Tiles -= amt;
        InventoryUpdate.Invoke();
        return true;
    }

    internal void DefineSavingUtility(SavingUtility savingUtility)
    {
        PlayerLevelDataList.DefineSavingUtility(savingUtility);
    }

    internal void SetCharacter(AvatarType type)
    {
        Avatar = type;
        AvatarChange?.Invoke();
    }

    // General Settings - methods
    internal void ChangeActiveTouchControl(int id)
    {
        ActiveTouchControl = id;
        InputSettingUpdate?.Invoke();
    }
    internal void ChangeCameraPos(int id)
    {
        CameraPos = id;
        InputSettingUpdate?.Invoke();
    }
}
