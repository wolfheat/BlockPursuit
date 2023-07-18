using System;
using TMPro;
using UnityEngine;

public enum BoostType { X2, X3, X4 }

public abstract class Boost : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI boostText;
    //[SerializeField] Image boostImage;
    [SerializeField] GameObject boostImageActive;
    [SerializeField] GameObject boostImageInactive;
    protected DateTime storedTime;

    [Range(1,60)]
    protected int boostIntervalMinutes = 5;


    protected void SetText(string text)
    {
        boostText.text = text;
    }

    private void OnEnable()
    {
        PlayerGameData.BoostTimeUpdated += UpdateBoostData;
    }

    private void OnDisable()
    {
        PlayerGameData.BoostTimeUpdated -= UpdateBoostData;
    }

    protected abstract void UpdateBoostData();

    private void FixedUpdate()
    {

        UpdateTimer();

    }

    private void UpdateTimer()
    {
        int TimeDiff = (int)(DateTime.Now - storedTime).TotalSeconds;

        int timeLeft = boostIntervalMinutes * 60 - TimeDiff;

        if(timeLeft < 0)
        {
            SetText("Not Active");
            SetActive(false);
            
        }
        else if(timeLeft > 0 && timeLeft <= boostIntervalMinutes*60)
        {
            SetActive(true);
            // Update time string counting down
            SetText(StringConverter.TimeAsString(timeLeft));
        }
        else
        {
            Debug.Log("Something is wrong, last stored time is in the future?");
        }
    }
    public void SetActive(bool v)
    {
        boostImageActive.SetActive(v);
        boostImageInactive.SetActive(!v);
    }
}
