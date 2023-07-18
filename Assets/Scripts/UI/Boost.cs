using System;
using TMPro;
using UnityEngine;

public enum BoostType { TileBoost, CoinBoost}
public abstract class Boost : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI boostText;
    //[SerializeField] Image boostImage;
    [SerializeField] GameObject boostImageActive;
    [SerializeField] GameObject boostImageInactive;
    protected BoostData data;

    internal void SetData(BoostData boostData)
    {
        data = boostData;
    }

    protected void SetText(string text)
    {
        boostText.text = text;
    }

    private void FixedUpdate()
    {

        UpdateTimer();

    }

    private void UpdateTimer()
    {
        if(data.timeLeft <= 0)
        {
            SetText("Not Active");
            SetActive(false);
            
        }
        else if(data.timeLeft > 0 && data.timeLeft <= data.boostIntervalMinutes*60)
        {
            SetActive(true);
            // Update time string counting down
            SetText(StringConverter.TimeAsString(data.timeLeft));
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
