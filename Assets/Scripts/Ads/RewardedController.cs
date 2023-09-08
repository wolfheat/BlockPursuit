using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;

namespace MyGameAds
{
    public class RewardedController : MonoBehaviour
    {
        public GameObject[] AdLoadedStatus;
        private RewardedAd _rewardedAd;

        public static Action Loaded;
        public static Action Closed;
        public void LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
                DestroyAd();

            Debug.Log("Loading rewarded ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            RewardedAd.Load(AdController.AdUnitID_Rewarded, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // If the operation failed with a reason.
                if (error != null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }
                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _rewardedAd = ad;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);

                // Inform the UI that the ad is ready.
                ActivateAdReady(true);


            });
                //StartCoroutine(SimpleWaitForShowAds());
        }

        private void ActivateAdReady(bool value)
        {
            Debug.Log("Activate Ad Ready: "+value);
            if (!value)
            {
                Debug.Log("Ad was not ready");
                return;
            }
            Loaded?.Invoke();
        }

        private IEnumerator SimpleWaitForShowAds()
        {
            yield return new WaitForSeconds(2.5f);

            //Then show ad
            ShowAd();
        }

        public void ShowAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                Debug.Log("Showing rewarded ad.");
                _rewardedAd.Show((Reward reward) =>
                {
                    Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                            reward.Amount,
                                            reward.Type));
                    FindObjectOfType<LevelComplete>().GetReward();

                    SavingUtility.playerGameData.AddWatchedAds();

                });
            }
            else
            {
                //TODO This error is reached fix (Not reached in result screen why)
                Debug.LogError("Rewarded ad is not ready yet.");
                Debug.LogError("_rewardedAd: "+ _rewardedAd);
                Debug.LogError("_rewardedAd.CanShowAd(): " + _rewardedAd.CanShowAd());

                // Inform the UI that the ad is not ready.
                ActivateAdReady(false);
            }

        }

        public void DestroyAd()
        {
            if (_rewardedAd != null)
            {
                Debug.Log("Destroying rewarded ad.");
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            // Inform the UI that the ad is not ready.
            ActivateAdReady(false);
        }

        public void LogResponseInfo()
        {
            if (_rewardedAd != null)
            {
                var responseInfo = _rewardedAd.GetResponseInfo();
                UnityEngine.Debug.Log(responseInfo);
            }
        }

        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };
            // Raised when the ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
                Closed.Invoke();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content with error : "
                    + error);
            };
        }
    }
}
