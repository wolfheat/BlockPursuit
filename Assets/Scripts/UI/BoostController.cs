using MyGameAds;
using System;
using UnityEngine;
public class BoostController : EscapableBasePanel
{
    [SerializeField] TypeABoost aBoost;
    [SerializeField] TypeBBoost bBoost;
    [SerializeField] BoostIcon aBoostIcon;
    [SerializeField] BoostIcon bBoostIcon;
    public BoostData A_BoostData = new BoostData(BoostType.TileBoost, 9);
    public BoostData B_BoostData = new BoostData(BoostType.CoinBoost, 9);

     public override void RequestESC()
    {
        if (!Enabled()) return;
        Debug.Log("ESC from boost");
        BackToLevelSelect();
    }

    private void OnEnable()
    {
        PlayerGameData.BoostTimeUpdated += UpdateBoostData;
        SavingUtility.LoadingComplete += UpdateBoostDataLoading;
    }

    private void OnDisable()
    {
        PlayerGameData.BoostTimeUpdated -= UpdateBoostData;
        SavingUtility.LoadingComplete -= UpdateBoostDataLoading;
    }

    protected void UpdateBoostDataLoading()
    {
        Debug.Log("UpdateBoostData from Loading Complete");
        UpdateBoostData();
    }
    protected void UpdateBoostData()
    {
        A_BoostData.SetNewUsedTime(SavingUtility.playerGameData.AtypeBoostTime);
        B_BoostData.SetNewUsedTime(SavingUtility.playerGameData.BtypeBoostTime);
    }

    private void Update()
    {
        // Manually Update the Boosts
        A_BoostData.UpdateIfActive();
        B_BoostData.UpdateIfActive();

    }

    private void Start()
    {
        aBoost.SetData(A_BoostData);
        bBoost.SetData(B_BoostData);
        aBoostIcon.SetData(A_BoostData);
        bBoostIcon.SetData(B_BoostData);
        RewardedController.Closed += RegainFocus;
    }

    private void RegainFocus()
    {
        if (!Enabled()) return;
        Debug.Log("BoostController Return from ad, Gain focus");
        SetSelected();

        //If first reward is allready active activate second reward?
        if(!A_BoostData.active)
            // Set a new Boost Time
            SavingUtility.playerGameData.SetABoostTime(DateTime.Now);
        else if (!B_BoostData.active)
            SavingUtility.playerGameData.SetBBoostTime(DateTime.Now);
        else
        {
            //Activate the one with less time left?
            if(A_BoostData.timeLeft < B_BoostData.timeLeft)
                SavingUtility.playerGameData.SetABoostTime(DateTime.Now);
            else
                SavingUtility.playerGameData.SetBBoostTime(DateTime.Now);
        }

        SoundController.Instance.PlaySFX(SFX.RecieveBoost);

    }

    public void BoostRequest()
    {
        Debug.Log("Request Boost Ad, show ad and return here");
        FindObjectOfType<RewardedController>().ShowAd();
    }


    public void BackToLevelSelect()
    {
        TransitionScreen.Instance.StartTransition(GameAction.ShowLevelSelect);
    }
    public void StartRequest()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideBoostPanel);
    }
}
