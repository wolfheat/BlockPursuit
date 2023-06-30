using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Image image;
    private const float HalfTransitionTime = 0.5f;
    private const float DarkTime = 0.5f;
    private Color StartColor = new Color(0,0,0,0.2f);
    private Color EndColor = new Color(0,0,0,1);

    public Action GameDarkEvent;

    public void StartTransition()
    {
        StartCoroutine(TransitionCoroutine());
    }


    private IEnumerator TransitionCoroutine()
    {
        //Darken
        panel.SetActive(true);
        float timer = 0;
        while (timer < HalfTransitionTime)
        {
            image.color = Color.Lerp(StartColor, EndColor, timer/HalfTransitionTime);
            timer += Time.deltaTime; 
            yield return null;
        }
        image.color = EndColor;
        // Dispatch GameDarkEvent
        GameDarkEvent.Invoke();

        //DarkTime
        timer = 0;
        while (timer < DarkTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Lighten
        timer = 0;
        while (timer < HalfTransitionTime)
        {
            image.color = Color.Lerp(StartColor, EndColor, 1 - timer / HalfTransitionTime);
            timer += Time.deltaTime;
            yield return null;
        }
        image.color = StartColor;
        panel.SetActive(false); 

    }

}
