using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private static AdManager instance;

    //private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private BannerView bannerView;
    public static int guSpeed, playTimes;

    public void Start()
    {
        // 以下の部分を追加
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        MobileAds.SetiOSAppPauseOnBackground(true);
        // Google AdMob Initial
        MobileAds.Initialize(initStatus => { });

        this.loadInterstitialAd();
        loadRewardAd();
    }

    public void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // テスト用広告ユニットID
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716"; // テスト用広告ユニットID
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the bottom of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest();

        // Load the banner with the request.
        bannerView.LoadAd(request);

    }

    public void DestroyBanner()
    {
        bannerView.Destroy();
    }
    

    public void loadInterstitialAd()
    {

#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        InterstitialAd.Load(adUnitId, new AdRequest(),
          (InterstitialAd ad, LoadAdError loadAdError) =>
          {
              if (loadAdError != null)
              {
                  // Interstitial ad failed to load with error
                  interstitial.Destroy();
                  return;
              }
              else if (ad == null)
              {
                  // Interstitial ad failed to load.
                  return;
              }
              ad.OnAdFullScreenContentClosed += () => {
                  HandleOnAdClosed();
              };
              ad.OnAdFullScreenContentFailed += (AdError error) =>
              {
                  HandleOnAdClosed();
              };
              interstitial = ad;
          });
    }

    private void HandleOnAdClosed()
    {
        this.interstitial.Destroy();
        this.loadInterstitialAd();
    }

    public void showInterstitialAd()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial Ad not load");
        }
    }

    public void loadRewardAd()
    {
        string adUnitId;
#if UNITY_ANDROID
    adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif
        // Load a rewarded ad
        RewardedAd.Load(adUnitId, new AdRequest(),
        (RewardedAd ad, LoadAdError loadError) =>
        {
            if (loadError != null)
            {
                // Rewarded ad failed to load with error
                return;
            }
            else if (ad == null)
            {
                // Rewarded ad failed to load.
                return;
            }
            ad.OnAdFullScreenContentClosed += () => {
                HandleRewardedAdAdClosed();
            };

            ad.OnAdFullScreenContentFailed += (AdError error) => {
                //InnerLoadRewardAd(adUnitId);
            };
            if (rewardedAd != null)
            {
                this.rewardedAd.Destroy();
            }
            this.rewardedAd = ad;
        });
    }
    public void showRewardAd()
    {
        if (this.rewardedAd != null && this.rewardedAd.CanShowAd())
        {
            this.rewardedAd.Show((Reward reward) =>
            {
                HandleUserEarnedReward(reward);
            });
        }
        else
        {
            Debug.Log("Rewoad Ad not load");
        }
    }


    public void HandleUserEarnedReward(Reward args)
    {

    }

    public void HandleRewardedAdAdClosed()
    {
        guSpeed = 30;
        SceneManager.LoadScene("GameScene");
        loadRewardAd();
    }
}