using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;

namespace MyGameAds
{
    [AddComponentMenu("GoogleMobileAds/Samples/InterstitialAdController")]
    public class InterstitialController : MonoBehaviour
    {
        public GameObject AdLoadedStatus;
        private InterstitialAd _interstitialAd;
        private InfoText info;

        public static Action Closed;

        public bool Loaded { get; private set; }

        private void Awake()
        {
            info = FindObjectOfType<InfoText>();
        }

        public void LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
                DestroyAd();

            Debug.Log("Loading interstitial ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            InterstitialAd.Load(AdController.AdUnitID_Interstitial, adRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                // If the operation failed with a reason.
                if (error != null)
                {
                    Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
                    info?.DisplayText("Interstitial ad failed to load an ad with error : " + error);
                    return;
                }
                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
                    info?.DisplayText("Unexpected error: Interstitial load event fired with null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
                //info.DisplayText("Interstitial ad loaded with response : " + ad.GetResponseInfo());
                _interstitialAd = ad;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);

                // Inform the UI that the ad is ready.
                AdLoadedStatus?.SetActive(true);

                Loaded = true;
            });
        }

        public void ShowAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                info?.DisplayText("Showing interstitial ad.");
                _interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
                info?.DisplayText("Showing interstitial anot ready yet.");
            }

            // Inform the UI that the ad is not ready.
            AdLoadedStatus?.SetActive(false);
        }

        public void DestroyAd()
        {
            if (_interstitialAd != null)
            {
                Debug.Log("Destroying interstitial ad.");
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Loaded = false;

            // Inform the UI that the ad is not ready.
            AdLoadedStatus?.SetActive(false);
        }

        public void LogResponseInfo()
        {
            if (_interstitialAd != null)
            {
                var responseInfo = _interstitialAd.GetResponseInfo();
                UnityEngine.Debug.Log(responseInfo);
            }
        }

        private void RegisterEventHandlers(InterstitialAd ad)
        {

            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad full screen content closed.");
                Closed.Invoke();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content with error : "
                    + error);
            };
        }
    }
}
