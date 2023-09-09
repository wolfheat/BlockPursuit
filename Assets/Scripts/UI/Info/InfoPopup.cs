using System.Collections;
using TMPro;
using UnityEngine;

public class InfoPopup : BasePanel
{
    [SerializeField] TextMeshProUGUI infoField;
    [SerializeField] RectTransform infoContentObject;
    private ISetSelected sourcePanel;
    private const float PopupTextVerticalMargins = 230f;

    public void ShowInfo(InfoData infoData, ISetSelected source)
    {
        sourcePanel = source;
        infoField.text = infoData.informationText;

        StartCoroutine(DetermineSize());

        ShowPanel();
    }
    
    public void HideInfo()
    {   
        HidePanel();
        
        sourcePanel?.SetSelected();
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
