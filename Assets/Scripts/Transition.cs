using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    private const float FadeTime = 1f;
    private const float StayBlack = 1f;
	[SerializeField] Image image;


    private void Awake()
    {
        image.canvasRenderer.SetAlpha(0.01f);
		image.gameObject.SetActive(false);
	}

	public IEnumerator Darken()
    {
        image.gameObject.SetActive(true);
        Debug.Log("Fading");
        image.canvasRenderer.SetAlpha(0.01f);        
        image.CrossFadeAlpha(1,FadeTime,true);

        yield return new WaitForSeconds(FadeTime);        
        yield return new WaitForSeconds(StayBlack);        
	}
    
	public IEnumerator Lighten()
    {
		Debug.Log("UnFading");
		image.CrossFadeAlpha(0.01f, FadeTime, true);
		yield return new WaitForSeconds(FadeTime);
		Debug.Log("Unfaded");
		image.gameObject.SetActive(false);
	}

}
