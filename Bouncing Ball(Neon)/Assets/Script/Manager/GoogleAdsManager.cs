using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.SceneManagement;

public enum Status
{
    load,
    Fail,
    Open,
    Close
}

public class GoogleAdsManager : MonoBehaviour
{
    // 전면광고
    private InterstitialAd First_interstitial;
    private InterstitialAd Second_interstitial;
    [SerializeField]private GameObject GO_Status;
    [SerializeField]private TextMeshProUGUI txt_Status;
    private int nextAd = 1;  

    // 보상형 광고
    private RewardedAd rewardedAdA;
    private RewardedAd rewardedAdB;

    private Status enum_Status;
    private int Count = 0;

    private void Awake()
    {
        RequestReward();
        RequestInterstitial();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (GO_Status != null)
        {
            txt_Status.text = enum_Status.ToString();
        }

        if (StageManager.instance.ClearNum == 1)
        {
            InterstitialAdTest();
        }

        if(SceneManager.GetActiveScene().name == "2_Chapter")
        {
            GO_Status = GameObject.Find("txt_Status");
            txt_Status = GO_Status.GetComponent<TextMeshProUGUI>();
        }
    }

    private void RequestInterstitial()
    {
        // 테스트 ca-app-pub-3940256099942544/1033173712  받은거 ca-app-pub-3041803582951090~4453880592 ca-app-pub-3940256099942544~3347511713
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        // 테스트 ca-app-pub-3940256099942544/4411468910   받은거 ca-app-pub-3041803582951090/5767850314
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif
        
        if(Count < 2)
        {
            First_interstitial = CreateAndLoadInterstitial(adUnitId);
            Second_interstitial = CreateAndLoadInterstitial(adUnitId);
        }

        if(!First_interstitial.IsLoaded())
        {
            First_interstitial = CreateAndLoadInterstitial(adUnitId);
        }

        if(!Second_interstitial.IsLoaded())
        {
            Second_interstitial = CreateAndLoadInterstitial(adUnitId);
        }

        Count++;
    }

    private InterstitialAd CreateAndLoadInterstitial(string adUnitId)
    {
        InterstitialAd interstitial = new InterstitialAd(adUnitId);

        // 광고가 성공적으로 로드
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // 광고 로드에 실패
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // 광고가 보여질때 실행
        interstitial.OnAdOpening += HandleOnAdOpened;
        // 광고가 닫혀질때 실행
        interstitial.OnAdClosed += HandleOnAdClosed;

        // 보상 테스트
        AdRequest request = new AdRequest.Builder().Build();
        // 전면 광고 최종 로드
        interstitial.LoadAd(request);

        return interstitial;
    }

    #region 전면 광고 핸들러
    private void HandleOnAdLoaded(object sender, EventArgs e)
    {
        enum_Status = Status.load;
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        enum_Status = Status.Fail;
    }

    private void HandleOnAdOpened(object sender, EventArgs e)
    {
        enum_Status = Status.Open;
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        enum_Status = Status.Close;
    }
    #endregion

    private void RequestReward()
    {
        // 테스트 ca-app-pub-3940256099942544/5224354917
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        // 테스트 ca-app-pub-3940256099942544/1712485313
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        if (Count < 2)
        {
            this.rewardedAdA = CreateAndLoadRewardAd(adUnitId);
            this.rewardedAdB = CreateAndLoadRewardAd(adUnitId);
        }

        if (!rewardedAdA.IsLoaded())
        {
            this.rewardedAdA = CreateAndLoadRewardAd(adUnitId);
        }
        if (!rewardedAdB.IsLoaded())
        {
            this.rewardedAdB = CreateAndLoadRewardAd(adUnitId);
        }

        Count++;
    }

    private RewardedAd CreateAndLoadRewardAd(string adUnitId)
    {
        // 보상형 광고 초기화
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        // 광고가 성공적으로 로드 되었을때 실행
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // 광고를 로드하는데 실패했을때 실행
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // 광고가 실행되어 열렸을때 실행
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // 광고를 보고 유저에게 보상이 들어갈때 실행
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // 광고를 닫았을때 로드
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        // 보상형 광고 최종 로드
        rewardedAd.LoadAd(request);

        return rewardedAd;
    }


    #region 보상형 광고 핸들러
    private void HandleRewardedAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("로드됨");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("실패됨");
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        Debug.Log("실행됨");
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        // 여기에 보상 내역 적기
        Debug.Log("보상 들어옴");
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        Debug.Log("닫힘");
    }
    #endregion

    public void InterstitialAdTest()
    {
        if (SceneManager.GetActiveScene().name == "2_Chapter")
        {
            if (this.First_interstitial.IsLoaded() && nextAd == 1)
            {
                this.First_interstitial.Show();
                nextAd = 2;
            }
            else if(this.Second_interstitial.IsLoaded() && nextAd == 2)
            {
                this.Second_interstitial.Show();
                nextAd = 1;
            }
            else
            {
                // 아직 광고가 준비 안됨 -> 팝업 실행
            }
            StageManager.instance.ClearNum = 0;
            RequestInterstitial();
        }
    }

    public void RewardAdTest()
    {
        if(this.rewardedAdA.IsLoaded())
        {
            this.rewardedAdA.Show();
        }
        RequestReward();
    }

    public void RewardBAdTest()
    {
        if (this.rewardedAdB.IsLoaded())
        {
            this.rewardedAdB.Show();
        }
        RequestReward();
    }
}
