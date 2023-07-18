using System;

public class TypeABoost : Boost
{
    private void Awake()
    {
        UpdateBoostData();
    }

    protected override void UpdateBoostData()
    {
        // If there is no DateTime stored store current?
        if (SavingUtility.playerGameData.AtypeBoostTime == null)
            SavingUtility.playerGameData.SetABoostTime(DateTime.Now.Add(-TimeSpan.FromMinutes(boostIntervalMinutes)));

        storedTime = SavingUtility.playerGameData.AtypeBoostTime;
    }
}
