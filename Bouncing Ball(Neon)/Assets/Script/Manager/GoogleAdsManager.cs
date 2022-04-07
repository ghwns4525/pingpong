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
    // ���鱤��
    private InterstitialAd First_interstitial;
    private InterstitialAd Second_interstitial;
    [SerializeField]private GameObject GO_Status;
    [SerializeField]private TextMeshProUGUI txt_Status;
    private int nextAd = 1;  

    // ������ ����
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
        // �׽�Ʈ ca-app-pub-3940256099942544/1033173712  ������ ca-app-pub-3041803582951090~4453880592 ca-app-pub-3940256099942544~3347511713
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        // �׽�Ʈ ca-app-pub-3940256099942544/4411468910   ������ ca-app-pub-3041803582951090/5767850314
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

        // ���� ���������� �ε�
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // ���� �ε忡 ����
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // ���� �������� ����
        interstitial.OnAdOpening += HandleOnAdOpened;
        // ���� �������� ����
        interstitial.OnAdClosed += HandleOnAdClosed;

        // ���� �׽�Ʈ
        AdRequest request = new AdRequest.Builder().Build();
        // ���� ���� ���� �ε�
        interstitial.LoadAd(request);

        return interstitial;
    }

    #region ���� ���� �ڵ鷯
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
        // �׽�Ʈ ca-app-pub-3940256099942544/5224354917
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        // �׽�Ʈ ca-app-pub-3940256099942544/1712485313
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
        // ������ ���� �ʱ�ȭ
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        // ���� ���������� �ε� �Ǿ����� ����
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // ���� �ε��ϴµ� ���������� ����
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // ���� ����Ǿ� �������� ����
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // ���� ���� �������� ������ ���� ����
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // ���� �ݾ����� �ε�
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        // ������ ���� ���� �ε�
        rewardedAd.LoadAd(request);

        return rewardedAd;
    }


    #region ������ ���� �ڵ鷯
    private void HandleRewardedAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("�ε��");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("���е�");
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        Debug.Log("�����");
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        // ���⿡ ���� ���� ����
        Debug.Log("���� ����");
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        Debug.Log("����");
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
                // ���� ���� �غ� �ȵ� -> �˾� ����
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
