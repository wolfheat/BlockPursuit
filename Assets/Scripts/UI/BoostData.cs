using System;
using UnityEngine;

public class BoostData
{
    public BoostType type;
    public DateTime lastUsed;
    public int timeLeft;
    public bool active;
    public int boostIntervalMinutes = 5;
    public float boostMultiplier = 0.25f;

    public BoostData(BoostType t,int intervalMinutes)
    {
        type = t;
        boostIntervalMinutes = intervalMinutes;
    }
    
    public void SetNewUsedTime(DateTime t)
    {
        //Debug.Log("Updating Last Used Time for: "+type+" from: "+lastUsed+" to: "+t);
        lastUsed = t;
        Update();
    }

    public DateTime GetDefaultTime()
    {
        Debug.Log("Runs if no value is set for last datetime");
        return DateTime.UtcNow.Add(-TimeSpan.FromMinutes(boostIntervalMinutes));
    }

    private void Update()
    {
        int TimeDiff = (int)(DateTime.UtcNow - lastUsed).TotalSeconds;

        timeLeft = boostIntervalMinutes * 60 - TimeDiff;

        if (timeLeft <= 0)
        {
            active = false;
        }
        else if (timeLeft > 0 && timeLeft <= boostIntervalMinutes * 60)
        {
            active = true;
        }
        else
        {
            Debug.Log("Something is wrong, last stored time is in the future?");
            Debug.Log("Stored Date: "+lastUsed+"Current Date: "+DateTime.UtcNow+" time left ="+timeLeft);
        }
    }

    public void UpdateIfActive()    
    {
        if(lastUsed != null && active)
        {
            Update();
        }
    }


}
