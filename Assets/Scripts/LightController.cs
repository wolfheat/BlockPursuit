using System;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Light light;
    public static LightController Instance { get; set; }

    internal void SetFromStoredValues()
    {
        light.intensity = SavingUtility.playerGameData.lightSettings.LightIntensity;
    }

    private void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); return; }
        else Instance = this;
    }
    }
