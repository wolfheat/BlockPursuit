using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] GameObject darkPanel;
    [SerializeField] Canvas canvas;
    [SerializeField] Image image;
    private const float HalfTransitionTime = 0.5f;
    private const float TransitionTime = 0.1f;
    private const float DarkTime = 0.5f;
    private const float FastDarkTime = 0.05f;
    private Color StartColor = new Color(0,0,0,0.2f); // Color Can't be a constant
    private Color EndColor = new Color(0,0,0,1);

    public Action GameDarkEvent;
    public Action GameDarkEventComplete;

    public static TransitionScreen Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }
        

    public void StartTransition()
    {
        if(GameSettings.InTransition) return;
        StartCoroutine(TransitionCoroutine());
    }
    
    public void StartTransition(GameAction action)
    {
        if(GameSettings.InTransition) return; // Double use of this test, remove this?

        GameSettings.StoredAction = action;
        StartCoroutine(TransitionCoroutine());
    }


    private IEnumerator TransitionCoroutine()
    {
        GameSettings.InTransition = true;

        float transitionTime = TransitionTime;
        float darkTime = FastDarkTime;
        
        //Darken
        darkPanel.SetActive(true);
        canvas.enabled = true;
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
        darkPanel.SetActive(false);
        canvas.enabled = false;
        GameSettings.InTransition = false;
        GameDarkEventComplete.Invoke();
    }

}
