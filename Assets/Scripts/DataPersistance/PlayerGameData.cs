using System;

public class AchievementData
{
    public bool[] Data { get; set; } = new bool[0];
}

[Serializable]
public class LightSettings
{
    public float LightIntensity { get; set; } = 1;
}

[Serializable]
public class GameEffectsSettings
{
    public bool UseShake { get; set; } = true;
    public bool AnimatedWater { get; set; } = true;
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

    // Totals
    public int TotalGoldCollected { get; set; } = 0;
    public int TotalTilesCollected { get; set; } = 0;


    // Player Achievement Data
    public AchievementData AchievementData { get; set; }
    
    // Player Levels Data
    public PlayerLevelDataList PlayerLevelDataList { get; private set; }

    // Player Settings
    public AvatarType Avatar { get; set; } = AvatarType.Dino;

    // Action Events
    public static Action InventoryUpdate;
    public static Action<int> AchievementUnlocked;
    public static Action BoostTimeUpdated;
    public static Action AvatarChange;

    public PlayerGameData()
    {
        Tiles = 0;
        Coins = 100;
        PlayerLevelDataList = new PlayerLevelDataList();
        AchievementData = new AchievementData();
    }

    public static void InvokeAll()
    {
        InventoryUpdate?.Invoke();
        BoostTimeUpdated?.Invoke();
    }

    public void UnlockAchievement(int index)
    {
        AchievementData.Data[index] = true;
        AchievementUnlocked?.Invoke(index);
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
        TotalGoldCollected += amt;
        InventoryUpdate.Invoke();
    }
    public void AddCoinsAndTiles(int coins, int tiles)
    {
        Coins += coins;
        Tiles += tiles;
        TotalGoldCollected += coins;
        TotalTilesCollected += tiles;
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
        TotalTilesCollected += amt;
        InventoryUpdate.Invoke();
    }

    public bool RemoveTiles(int amt)
    {
        if(Tiles < amt) return false;
        Tiles -= amt;
        InventoryUpdate.Invoke();
        return true;
    }


    internal void SetCharacter(AvatarType type)
    {
        Avatar = type;
        AvatarChange?.Invoke();
    }

    internal void HandleMissionReward(MissionRewardData missionRewardData)
    {
        switch (missionRewardData.rewardType)
        {
            case RewardType.Gold:
                AddCoins(missionRewardData.amount);
                break;
            case RewardType.Tiles:
                AddTiles(missionRewardData.amount);
                break;
            case RewardType.Unlock:
                // Unlock Tier HERE
                break;
        }
    }
}

[Serializable]
public class GameSettingsData
{
    // General Game Settings
    public int ActiveTouchControl { get; set; } // Having these private set wont let the load method write these values
    public int CameraPos { get; set; } // Having these private set wont let the load method write these values

    public SoundSettings soundSettings = new SoundSettings();
    public LightSettings lightSettings = new LightSettings();
    public GameEffectsSettings gameEffectsSettings = new GameEffectsSettings(); // Use shake etc

    // Action Events
    public static Action InputSettingUpdate;

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
