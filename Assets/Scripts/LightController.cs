using System;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Light lightObject;
    public static LightController Instance { get; set; }

    internal void SetFromStoredValues()
    {
        lightObject.intensity = SavingUtility.gameSettingsData.lightSettings.LightIntensity;
    }

    private void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); return; }
        else Instance = this;
    }
    }
