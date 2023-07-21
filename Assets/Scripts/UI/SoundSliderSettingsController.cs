using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderSettingsController : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI percent;
    private float oldSliderValue = 0;


    public bool IsOn()
    {
        return toggle.isOn;
    }
    
    public float SliderValue()
    {
        return slider.value;
    }

    public void SliderChange()
    {
        Debug.Log("Slider changed: "+slider.value);
        SetVolumeFromStoredValue(slider.value);
    }

    public void SetVolumeFromStoredValue(float v)
    {
        Debug.Log("SetVolumeFromStoredValue: "+v+" for "+name);
        if (v == 0)
        {
            percent.text = "OFF";
            return;
        }
        slider.value = v;
        percent.text = ((int)(v*100))+"%";
    }

}
