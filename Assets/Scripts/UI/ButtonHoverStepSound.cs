using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverStepSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler 
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundController.Instance.PlaySFX(SFX.MenuStep);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SoundController.Instance.PlaySFX(SFX.MenuSelect);
	}	

}
