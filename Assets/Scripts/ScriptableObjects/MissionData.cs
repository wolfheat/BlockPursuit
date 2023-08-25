using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "New Mission")]
public class MissionData : ScriptableObject
{
    public int ID;
    public int amount;
    public MissionType type;        
    public Sprite missionLogoSprite;
    public string missionName;
    public string missionDescription;
    public MissionRewardData missionRewardData;
    public DateInfo lastCompletion;

    public bool TimerUnlocked => DateInfo.TimeHasPassed(lastCompletion, type);
}
[Serializable]
public class DateInfo
{
    public string timeString;

    public static bool TimeHasPassed(DateInfo lastCompletion, MissionType type)
    {
        switch (type)
        {
            case MissionType.Single:
                return lastCompletion.timeString != "2000 - 01 - 01 00: 00: 00"; // Not set yet make better?
            case MissionType.Hourly:
                return DateTime.Now.Subtract(lastCompletion.GetDateTime()).TotalHours >= 1;
            case MissionType.Daily:
                return DateTime.Now.Subtract(lastCompletion.GetDateTime()).TotalHours >= 22;
            case MissionType.Weekly:
                return DateTime.Now.Subtract(lastCompletion.GetDateTime()).TotalDays >= 7;
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
public enum MissionType{Single, Hourly, Daily, Weekly}