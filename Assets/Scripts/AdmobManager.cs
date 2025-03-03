using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Overlays;

public class AdmobManager : MonoBehaviour
{
    [SerializeField] private bool isTestMode;
    [SerializeField] private Button RewardAdsBtn;
    [SerializeField] private RandomSelect randomSelect;
    public int count = 0;
    public TextMeshProUGUI countText;

    private RewardedAd rewardedAd;

    private string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    private string rewardID = "ca-app-pub-5589156443476831/5446552773";

    private void Start()
    {
#if UNITY_EDITOR
        isTestMode = true;
#else
    isTestMode = Debug.isDebugBuild;
#endif

        MobileAds.Initialize(initStatus =>
        {
            LoadRewardAd();
        });

        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            TestDeviceIds = new List<string>() { "1DF7B7CC05014E8" }
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);
        RewardAdsBtn.onClick.AddListener(ShowRewardAd);
    }

    private void Update()
    {
        RewardAdsBtn.interactable = rewardedAd != null && rewardedAd.CanShowAd();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest();
    }

    private void LoadRewardAd()
    {
        string adUnitId = isTestMode ? rewardTestID : rewardID;
        RewardedAd.Load(adUnitId, GetAdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                //리워드 광고 로드 실패
                return;
            }
            //리워드 광고 로드;
            rewardedAd = ad;
        });
    }

    public void ShowRewardAd()
    {
        if(count > 0)
        {
            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    randomSelect.FreeSelectCard();
                    count--;
                    countText.text = count.ToString() + " / 5";
                    GameManager.instance.SaveData();
                });
                LoadRewardAd();
            }
            else
            {
                //광고가 준비 중;
                LoadRewardAd();
            }
        }
    }

    public void ResetText(int _count)
    {
        count = _count;
        countText.text = count.ToString() + " / 5";
    }
}
