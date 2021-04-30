using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "4088056";
#elif UNITY_ANDROID
    private string gameId = "4088057";
#endif

    [Header("Test Mode:")]
    [SerializeField] bool testMode = true;
    string mySurfacingId = "rewardedVideo";

    [Header("Button info:")]
    public GameObject adsBtn;
    public Text adsTxt;

    private int pts;

    [Header("UI Controller:")]
    public UiController uCont;

    [HideInInspector] public bool noAds;   //Purchase this from the shop!!!

    [Header("Icons:")]
    public Image adsIcon;
    public Sprite[] icons = new Sprite[2];
    void Start()
    {
        noAds = SaveData.instance.noAds;
        if (!noAds)
        {
            Advertisement.AddListener(this);
            // Initialize the Ads service:
            Advertisement.Initialize(gameId, testMode);

            int chance = Random.Range(0, 10);
            if (chance > 7)
            {
                ShowInterstitialAd();
            }
        }

        int points = Random.Range(200, 501);
        int[] ptsList = new int[] { 200, 225, 250, 275, 300, 325, 350 };
        int randPtsId = Random.Range(0, ptsList.Length);
        pts = (points % 5 == 0) ? points : ptsList[randPtsId];
    }

    public void ShowReward()
    {
        adsTxt.text = $"+{pts}";
        adsIcon.sprite = noAds ? icons[0] : icons[1];
    }

    public void ShowInterstitialAd()
    {
        if (!noAds)
        {
            // Check if UnityAds ready before calling Show method:
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
            else
            {
                Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
            }
        }
    }

    public void ShowRewardedVideo()
    {
        //Hide The Button:
        adsBtn.SetActive(false);

        if (!noAds)
        {
            // Check if UnityAds ready before calling Show method:
            if (Advertisement.IsReady(mySurfacingId))
            {
                Advertisement.Show(mySurfacingId);
            }
            else
            {
                Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            }
        } else
            uCont.RewardAdPts(pts);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished && surfacingId == mySurfacingId)
        {
            // Reward the user for watching the ad to completion.
            uCont.RewardAdPts(pts);
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        if(!noAds)
            Advertisement.RemoveListener(this);
    }
}
