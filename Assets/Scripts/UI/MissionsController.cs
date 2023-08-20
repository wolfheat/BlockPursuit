using System.Collections.Generic;
using UnityEngine;

public class MissionsController : EscapableBasePanel
{
    [SerializeField] GameObject missionHolder;
    [SerializeField] Mission missionPrefab;
    [SerializeField] MissionData[] missionDatas;
    private List<Mission> missions = new List<Mission>();
    
    public override void RequestESC()
    {   
        if (!Enabled()) return;
        Debug.Log("Close Missions ESC");
        CloseMenu();
    }
    public void CloseMenu()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideMissions);
    }

    private void Start()
    {
        CreateMissions();
    }

    private void CreateMissions()
    {
        foreach (var mission in missionDatas)
        {
            Mission newMission = Instantiate(missionPrefab, missionHolder.transform);
            newMission.SetData(mission);
            missions.Add(newMission);
        }
    }
}

