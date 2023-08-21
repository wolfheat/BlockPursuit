using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InfoPopup : BasePanel
{
    [SerializeField] TextMeshProUGUI infoField;
    [SerializeField] RectTransform infoContentObject;
    private const float PopupTextVerticalMargins = 200f;

    internal void ShowInfo(InfoData infoData)
    {

        infoField.text = infoData.informationText;

        StartCoroutine(DetermineSize());

        ShowPanel();
    }

    private IEnumerator DetermineSize()
    {
        yield return null;
        float infoHeightOLD = infoField.rectTransform.sizeDelta.y;
        float infoHeight = infoField.GetRenderedValues().y;

        SetInfoPanelSize(infoHeight);
        Debug.Log("Height of text is "+infoHeight);

    }

    private void SetInfoPanelSize(float infoHeight)
    {
        infoContentObject.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, infoHeight+ PopupTextVerticalMargins);
    }
}
