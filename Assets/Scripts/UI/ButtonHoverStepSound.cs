using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverStepSound : MonoBehaviour, IPointerClickHandler 
{
	public void OnPointerClick(PointerEventData eventData)
	{
		SoundController.Instance.PlaySFX(SFX.MenuSelect);
	}	

}
