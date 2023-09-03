using System;
using System.Collections.Generic;

public class AchievementData
{
    public bool[] Data { get; set; } = new bool[0];
}

public class MissionsSaveData
{
    public Dictionary<int,MissionSaveData> Data { get; set; } = new Dictionary<int, MissionSaveData>();
}

public class MissionSaveData
{
    public bool everCompleted = false;
    public bool active = true;
    public int amount = 0;
    public DateTime lastCompletion = DateTime.MinValue;
    public bool LockTimeHasPassed(MissionType type)
    {
        switch (type)
        {
            case MissionType.Hourly:
                return DateTime.UtcNow.Subtract(lastCompletion).TotalHours >= 1;
            case MissionType.Daily:
                return DateTime.UtcNow.Subtract(lastCompletion).TotalHours >= 22;
            case MissionType.Weekly:
                return DateTime.UtcNow.Subtract(lastCompletion).TotalDays >= 7;
            default:
                return false;// Does not care about time
        }
    }
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
    public int PlayTime { get; set; } = 0;
    public int AdsWatched { get; set; } = 0;

    // Totals
    public int TotalGoldCollected { get; set; } = 0;
    public int TotalTilesCollected { get; set; } = 0;

    // Player Mission Saved Data
    public MissionsSaveData MissionsSaveData { get; set; }
    
    // Player Achievement Data
    public AchievementData AchievementData { get; set; }
    
    // Player Levels Data
    public PlayerLevelDataList PlayerLevelDataList { get; private set; }

    // Player Settings
    public AvatarType Avatar { get; set; } = AvatarType.Dino;

    // Action Events
    public static Action InventoryUpdate;
    public static Action MissionUpdate;
    public static Action AdsWatchedAdded;
    public static Action<int> MissionCompleted;
    public static Action<int> AchievementUnlocked;
    public static Action BoostTimeUpdated;
    public static Action AvatarChange;
    public static Action<int> UnlockTier; 

    public PlayerGameData()
    {
        PlayerLevelDataList = new PlayerLevelDataList();
        AchievementData = new AchievementData();
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


    public void SetCharacter(AvatarType type)
    {
        Avatar = type;
        AvatarChange?.Invoke();
    }

    public void HandleMissionReward(MissionRewardData missionRewardData)
    {
        switch (missionRewardData.rewardType)
        {
            case RewardType.Gold:
                AddCoins(missionRewardData.value);
                break;
            case RewardType.Tiles:
                AddTiles(missionRewardData.value);
                break;
            case RewardType.Unlock:
                //UnlockTier
                UnlockTier?.Invoke(missionRewardData.value);
                break;
        }
    }

    public void UpdateMissionCompletion(MissionSaveData missionSaveData)
    {
        // Set new last completiontime
        missionSaveData.lastCompletion = DateTime.UtcNow;
        missionSaveData.everCompleted = true;
        missionSaveData.amount = 0;
        
        MissionUpdate?.Invoke();
    }

    public bool CompleteStepForMission(MissionSaveData missionSaveData,int completeAmount)
    {
        missionSaveData.amount++;

        // Invoke if not completed (if completed UpdateMissionCompletion will be called which invokes the save)
        MissionUpdate?.Invoke();

        return missionSaveData.amount >= completeAmount;
    }

    public void AddWatchedAds()
    {
        AdsWatched++;
        AdsWatchedAdded?.Invoke();
    }
    public void AddPlayTimeMinutes(int amt)
    {
        PlayTime += amt;
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
    public static Action GameSettingsUpdated;

    // General Settings - methods
    internal void ChangeActiveTouchControl(int id)
    {
        ActiveTouchControl = id;
        GameSettingsUpdated?.Invoke();
    }
    internal void ChangeCameraPos(int id)
    {
        CameraPos = id;
        GameSettingsUpdated?.Invoke();
    }
}
