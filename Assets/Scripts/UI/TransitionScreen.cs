using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Image image;
    private const float HalfTransitionTime = 0.5f;
    private const float FastTransitionTime = 0.1f;
    private const float DarkTime = 0.5f;
    private const float FastDarkTime = 0.05f;
    private Color StartColor = new Color(0,0,0,0.2f);
    private Color EndColor = new Color(0,0,0,1);

    public Action GameDarkEvent;

    public void StartTransition()
    {
        StartCoroutine(TransitionCoroutine());
    }


    private IEnumerator TransitionCoroutine()
    {
        float transitionTime = HalfTransitionTime;
        float darkTime = DarkTime;

        if (GameSettings.UseFast)
        {
            transitionTime = FastTransitionTime;
            darkTime = FastDarkTime;
        }

        //Darken
        panel.SetActive(true);
        float timer = 0;
        while (timer < transitionTime)
        {
            image.color = Color.Lerp(StartColor, EndColor, timer/ transitionTime);
            timer += Time.deltaTime; 
            yield return null;
        }
        image.color = EndColor;
        // Dispatch GameDarkEvent
        GameDarkEvent.Invoke();

        //DarkTime
        timer = 0;
        while (timer < darkTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Lighten
        timer = 0;
        while (timer < transitionTime)
        {
            image.color = Color.Lerp(StartColor, EndColor, 1 - timer / transitionTime);
            timer += Time.deltaTime;
            yield return null;
        }
        image.color = StartColor;
        panel.SetActive(false); 

    }

}
