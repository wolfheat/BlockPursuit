using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdController : MonoBehaviour
{
    public static Time LastInterstitial { get; set; }
    public static Time LastRewarded { get; set; }

    // Android phone A40 ad ID
    // 21b57b1a-1c89-4856-a479-c97512ee9234


    // REAL VALUES
    //public static readonly string AdUnitID_Android_Banner           = "ca-app-pub-6702566600406301/9644289520";
    //public static readonly string AdUnitID_Android_Interstitial     = "ca-app-pub-6702566600406301/5705044516";
    //public static readonly string AdUnitID_Android_Rewarded         = "ca-app-pub-6702566600406301/2695737799";
    //iOS
    //public static readonly string AdUnitID_iOS_Banner         = "ca-app-pub-6702566600406301/7785247095";
    //public static readonly string AdUnitID_iOS_Interstitial   = "ca-app-pub-6702566600406301/6089022048";
    //public static readonly string AdUnitID_iOS_Rewarded       = "ca-app-pub-6702566600406301/3526437885";

    //TEST VALUES

#if UNITY_ANDROID

    //ANDROID
    public static readonly string AdUnitID_Banner           = "ca-app-pub-3940256099942544/6300978111";
    public static readonly string AdUnitID_Interstitial     = "ca-app-pub-3940256099942544/1033173712";
    public static readonly string AdUnitID_Rewarded         = "ca-app-pub-3940256099942544/5224354917";

#elif UNITY_IPHONE

    //iOS
    public static readonly string AdUnitID_Banner       = "ca-app-pub-3940256099942544/2934735716";
    public static readonly string AdUnitID_Interstitial = "ca-app-pub-3940256099942544/4411468910";
    public static readonly string AdUnitID_Rewarded     = "ca-app-pub-3940256099942544/1712485313";

#endif

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }


}
