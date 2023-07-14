using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdController : MonoBehaviour
{
    private static bool _isInitialized;
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
        // Demonstrates how to configure Google Mobile Ads.
        // Google Mobile Ads needs to be run only once and before loading any ads.
        if (_isInitialized)
        {
            return;
        }

        // On Android, Unity is paused when displaying interstitial or rewarded video.
        // This setting makes iOS behave consistently with Android.
        MobileAds.SetiOSAppPauseOnBackground(true);

        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.
        // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // Set your test devices.
        // https://developers.google.com/admob/unity/test-ads
        List<string> deviceIds = new List<string>()
            {
                AdRequest.TestDeviceSimulator,
                // Add your test device IDs (replace with your own device IDs).
                #if UNITY_IPHONE
                "96e23e80653bb28980d3f40beb58915c"
                #elif UNITY_ANDROID
                "75EF8D155528C04DACBBA6F36F433035"
                #endif
            };

        // Configure your RequestConfiguration with Child Directed Treatment
        // and the Test Device Ids.
        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            TestDeviceIds = deviceIds
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        Debug.Log("Google Mobile Ads Initializing.");
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            _isInitialized = true;
        });
    }


}
