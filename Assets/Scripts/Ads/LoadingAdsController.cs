using MyGameAds;
using UnityEngine;

public class LoadingAdsController : BasePanel
{


    private void OnEnable()
    {
        RewardedController.Loaded += AdFullyLoaded;
    }

    private void AdFullyLoaded()
    {
        Debug.Log("Ad Fully Loaded Hide Loading Screen");
        HidePanel();
    }
}
