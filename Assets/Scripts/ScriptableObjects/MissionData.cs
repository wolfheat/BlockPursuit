using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "New Mission")]
public class MissionData : ScriptableObject
{
    public int ID;
    public int amount;
    //public OccuranceType occurance;
    public MissionType type;        
    public Sprite missionLogoSprite;
    public string missionName;
    public string missionDescription;
    public MissionRewardData missionRewardData;
    public DateInfo lastCompletion;

    public bool TimerUnlocked => DateInfo.TimeHasPassed(lastCompletion, type);
    public bool Completed => lastCompletion.Completed;
    public float TimePassed => (float)DateTime.UtcNow.Subtract(lastCompletion.GetDateTime()).TotalHours;
}
[Serializable]
public class DateInfo
{
    public const string DefaultString = "2000-01-01 00:00:00";
    public string timeString;
    public bool Completed => timeString != DefaultString;
    public static bool TimeHasPassed(DateInfo lastCompletion, MissionType type)
    {
        switch (type)
        {
            case MissionType.Single:
                return lastCompletion.timeString != DefaultString; // Not set yet make better?
            case MissionType.Hourly:
                return DateTime.UtcNow.Subtract(lastCompletion.GetDateTime()).TotalHours >= 1;
            case MissionType.Daily:
                return DateTime.UtcNow.Subtract(lastCompletion.GetDateTime()).TotalHours >= 22;
            case MissionType.Weekly:
                return DateTime.UtcNow.Subtract(lastCompletion.GetDateTime()).TotalDays >= 7;
            case MissionType.Pool:
                return DateTime.UtcNow.Subtract(lastCompletion.GetDateTime()).TotalDays >= 7;
                break;
            default:
                return false;
        }
    }

    public DateTime GetDateTime()
    {
        if (DateTime.TryParse(timeString, out DateTime result))
        {
            return result;
        }
        else
        {
            Debug.LogError("Failed to parse DateTime string.");
            return DateTime.MinValue;
        }
    }

    public void SetDateTime(DateTime dateTime)
    {
        timeString = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
public enum MissionType{Single, Hourly, Daily, Weekly, Pool}
public enum OccuranceType { Single, Recurring}