using TMPro;
using UnityEngine;

public enum BoostType { TileBoost, CoinBoost}
public class Boost : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI boostTimeText;
    [SerializeField] GameObject boostTimeObject;
    [SerializeField] GameObject boostTimeObjectDisabled;

    [SerializeField] GameObject boostValueEnabled;
    [SerializeField] GameObject boostValueDisabled;

    [SerializeField] GameObject boostImageActive;
    [SerializeField] GameObject boostImageInactive;
    protected BoostData data;

    internal void SetData(BoostData boostData)
    {
        data = boostData;
    }

    protected void SetText(string text)
    {
        boostTimeText.text = text;
    }

    private void FixedUpdate()
    {
        if (data == null) return;
        UpdateTimer();

    }

    private void UpdateTimer()
    {
        if(data.timeLeft <= 0)
        {
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
        //Image
        boostImageActive.SetActive(v);
        boostImageInactive.SetActive(!v);

        // Value
        boostTimeObject.SetActive(v);
        boostTimeObjectDisabled.SetActive(!v);

        // Type
        boostValueEnabled.SetActive(v);
        boostValueDisabled.SetActive(!v);
    }
}
