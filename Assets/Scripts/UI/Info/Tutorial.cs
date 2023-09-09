using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : BasePanel
{
    [SerializeField] TextMeshProUGUI infoField;
    [SerializeField] TextMeshProUGUI indexText;
    [SerializeField] RectTransform infoContentObject;
    [SerializeField] Image image;

    [SerializeField] List<InfoSpriteSheetData> infos;
    
    private const float PopupTextVerticalMargins = 920f;

    private int array = 0;
    private int index = 0;

    private Coroutine gifCoroutine;

    public void ShowTutorial()
    {
        GameSettings.IsPaused = true;
        array = 0;
        index = 0;
        ShowInfoForIndex();
        ShowPanel();
        gifCoroutine = StartCoroutine(GifCoroutine());
    }
    
    public void HideTutorial()
    {
        GameSettings.IsPaused = false;
        HidePanel();
        if (gifCoroutine != null)
        {
            StopCoroutine(gifCoroutine);
            gifCoroutine = null;
        }
    }

    private IEnumerator GifCoroutine()
    {
        while (true)
        {
            image.sprite = infos[array].sprites[index];
            index = (index + 1) % infos[array].sprites.Length;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ShowNextInfo()
    {
        array = (array+1)%infos.Count;        
        index = 0;
        ShowInfoForIndex();
    }
    public void ShowPreviousInfo()
    {
        array = (array + infos.Count - 1)%infos.Count;
        index = 0;
        ShowInfoForIndex();
    }

    private void ShowInfoForIndex()
    {
        infoField.text = infos[array].informationText;
        indexText.text = (array+1).ToString();
        StartCoroutine(DetermineSize());
    }

    private IEnumerator DetermineSize()
    {
        yield return null;
        
        float infoHeight = infoField.GetRenderedValues().y;
        SetInfoPanelSize(infoHeight);
    }

    private void SetInfoPanelSize(float infoHeight)
    {
        infoContentObject.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, infoHeight + PopupTextVerticalMargins);
    }
}
