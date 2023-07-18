using System;

public class TypeBBoost : Boost
{
    private void Awake()
    {
        UpdateBoostData();
    }

    protected override void UpdateBoostData()
    {
        // If there is no DateTime stored store current?
        if (SavingUtility.playerGameData.BtypeBoostTime == null)
            SavingUtility.playerGameData.SetBBoostTime(DateTime.Now.Add(-TimeSpan.FromMinutes(boostIntervalMinutes)));

        storedTime = SavingUtility.playerGameData.BtypeBoostTime;
    }



}
