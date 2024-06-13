using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    private RewardedAd _rewardedAd;
    private string _adUnitId;
    private string rewardedId = "ca-app-pub-1140869794797769/7243543428";
    private string testId = "ca-app-pub-3940256099942544/5224354917";
    GameManagerCine gameManager;

    void Start()
    {
#if UNITY_ANDROID
        _adUnitId = testId;
#elif UNITY_IPHONE
          string _adUnitId = "unused";
#else
          string _adUnitId = "unused";
#endif

        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();

        MobileAds.Initialize(initStatus => { });
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " + "with error : " + error);
                    return;
                }
                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _rewardedAd = ad;

                // Subscribe to events if needed
            });
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                gameManager.StuffsToDoAfterRewardPlayer();
            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }
}