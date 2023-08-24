using MyGameAds;
using UnityEngine;

public class LoadingAdsController : BasePanel
{
    [SerializeField] RewardedController rewardedController;
    private void OnEnable()
    {
        RewardedController.Loaded += AdFullyLoaded;
    }

    private void AdFullyLoaded()
    {
        if (!Enabled()) return;
        Debug.Log("Ad Fully Loaded Hide Loading Screen");
        HidePanel();
        ShowLoadedAd();
    }

    public void ShowLoadedAd()
    {
        Debug.Log("Request Boost Ad, show ad and return here");
        rewardedController.ShowAd();
    }

}
