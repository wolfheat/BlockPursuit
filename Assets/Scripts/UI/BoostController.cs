using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoostController : BasePanel
{
    [SerializeField] Button mainSelectedButton;
    [SerializeField] Boost boostPrefab;

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void BoostRequest()
    {
        Debug.Log("Request Boost Ad, show ad and return here");
        SavingUtility.playerGameData.SetABoostTime(DateTime.Now);
    }

    public void StartRequest()
    {
        FindObjectOfType<TransitionScreen>().StartTransition(GameAction.HideBoostPanel);
    }
}
