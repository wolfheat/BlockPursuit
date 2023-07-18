using MyGameAds;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoostController : BasePanel
{
    [SerializeField] Button mainSelectedButton;
    [SerializeField] Boost boostPrefab;

    private void Start()
    {
        RewardedController.Closed += RegainFocus;
    }

    private void RegainFocus()
    {
        Debug.Log("Returned from rewarded ad.");
        SetSelected();
        ResetABoost();
    }

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void BoostRequest()
    {
        Debug.Log("Request Boost Ad, show ad and return here");
        FindObjectOfType<RewardedController>().ShowAd();
    }

    public void ResetABoost()
    {
        SavingUtility.playerGameData.SetABoostTime(DateTime.Now);
    }


    public void StartRequest()
    {
        FindObjectOfType<TransitionScreen>().StartTransition(GameAction.HideBoostPanel);
    }
}
