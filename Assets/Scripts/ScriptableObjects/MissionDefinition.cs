using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "New Mission")]
public class MissionDefinition : ScriptableObject
{
    public int ID;
    public int completeAmount;
    //public OccuranceType occurance;
    public MissionType type;        
    public Sprite missionLogoSprite;
    public string missionName;
    public string missionDescription;
    public MissionRewardData missionRewardData;    
}

public enum MissionType{Single, Hourly, Daily, Weekly, Pool}
public enum OccuranceType { Single, Recurring}