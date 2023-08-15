using System;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField] GameObject animatedWater;
    [SerializeField] GameObject simpleWater;


    private void OnEnable()
    {
        SavingUtility.LoadingComplete += UpdateFromStored;
    }
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= UpdateFromStored;
    }

    private void UpdateFromStored()
    {
        SetAnimatedWater(SavingUtility.gameSettingsData.gameEffectsSettings.AnimatedWater);
    }

    public void SetAnimatedWater(bool set)
    {
        animatedWater.SetActive(set);
        simpleWater.SetActive(!set);
    }
}
