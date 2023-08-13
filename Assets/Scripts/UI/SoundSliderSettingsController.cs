using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderSettingsController : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI percent;
    public bool listenForChange = true;

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
        if (listenForChange)
        {
            SetVolumeFromStoredValue(slider.value);
            Debug.Log("Slider changed: " + slider.value);
        }
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
