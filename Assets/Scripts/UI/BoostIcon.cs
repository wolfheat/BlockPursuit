using TMPro;
using UnityEngine;

public class BoostIcon : MonoBehaviour
{
    [SerializeField] GameObject visualObject;
    [SerializeField] TextMeshProUGUI timeText;

    protected BoostData data;
    internal void SetData(BoostData boostData)
    {
        data = boostData;
    }

    private void Update()
    {
        if(data.active == true)
        {
            visualObject.SetActive(true);
            timeText.text = "Active";
        }
        else
        {
            visualObject.SetActive(false);
            timeText.text = "Not Active";
        }
    }



}
