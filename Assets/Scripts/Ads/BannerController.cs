using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine.UI;

namespace MyGameAds
{
    public class BannerController : MonoBehaviour
    {
        //public GameObject AdLoadedStatus;
        public bool AdLoadedStatus=false;
        public bool AdLoadedShown=false;

        private BannerView _bannerView;

        private void Start()
        {
            // Currently just waiting for 3 seconds to show Banner. Later find out how to read that it has loaded
            LoadAd();
            //StartCoroutine(ShowBanner());
        }

        private IEnumerator ShowBanner()
        {
            yield return new WaitForSeconds(3);
            ShowAd();
        }

        public void CreateBannerView()
        {
            Debug.Log("Creating banner view.");

            // If we already have a banner, destroy the old one.
            DestroyOldBannerIfPresent();

            // Create a 320x50 banner at bottom of the screen.
            _bannerView = new BannerView(AdController.AdUnitID_Banner, AdSize.Banner, AdPosition.Bottom);

            // Listen to events the banner may raise.
            ListenToAdEvents();

            Debug.Log("Banner view created.");

        }
        public void LoadAd()
        {
            // Create an instance of a banner view first.
            if (_bannerView == null)
            {
                CreateBannerView();
            }

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            Debug.Log("Loading banner ad.");
            _bannerView.LoadAd(adRequest);
        }
        public void ShowAd()
        {
            if (AdLoadedStatus && !AdLoadedShown)
            {
                Debug.Log("Showing banner view.");
                // BannerVier.Show seems to create a copy instead of showing the one loaded.
                _bannerView.Show();
                AdLoadedShown = true;
            }
            else if(!AdLoadedStatus)
            {
                Debug.Log("No active Banner Load new");
                LoadAd();
            }
        }

        public void HideAd()
        {
            if (AdLoadedStatus && AdLoadedShown)
            {
                Debug.Log("Hiding banner view.");
                _bannerView.Hide();
                AdLoadedShown = false;
            }
        }

        public void DestroyOldBannerIfPresent()
        {
            if (_bannerView == null)
            {
                Debug.Log("No Banner to destroy");
                return;
            }

            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;

            // Inform the UI that the ad is not ready.
            AdLoadedStatus = false;

        }

        public void LogResponseInfo()
        {
            if (_bannerView != null)
            {
                var responseInfo = _bannerView.GetResponseInfo();
                if (responseInfo != null)
                {
                    UnityEngine.Debug.Log(responseInfo);
                }
            }
        }

        private void ListenToAdEvents()
        {
            // Raised when an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += () =>
            {
                Debug.Log("Banner view loaded an ad with response : "
                    + _bannerView.GetResponseInfo());

                // Inform the UI that the ad is ready.
                AdLoadedStatus = true;
                AdLoadedShown = true;
            };
            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                Debug.LogError("Banner view failed to load an ad with error : " + error);
            };
            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Banner view paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            _bannerView.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Banner view recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            _bannerView.OnAdClicked += () =>
            {
                Debug.Log("Banner view was clicked.");
            };
            // Raised when an ad opened full screen content.
            _bannerView.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Banner view full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            _bannerView.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Banner view full screen content closed.");
            };
        }
    }
}
