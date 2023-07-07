using UnityEngine;

public static class StringConverter
{
    public static string TimeAsString(float time)
    {
        return TimeAsString(Mathf.RoundToInt(time));
    }
    public static string TimeAsString(int timeAsInt)
    {
        int minutes = (timeAsInt / 60);
        int sec = (timeAsInt % 60);
        return (minutes > 0 ? (minutes + "m") : "") + sec + "s";
    }
    public static string LevelAsString(int diff, int lev)
    {
        return ((char)(diff + 'A')) + "." + (lev + 1).ToString();
    }
    public static string LevelAsStringWithParantheses(int diff, int lev)
    {
        return "(" + LevelAsString(diff,lev)+")"; 
    }
}
