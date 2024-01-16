using UnityEngine;
using UnityEngine.Events;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    public UnityAction<bool> OnRewardedComplete;
    public UnityAction OnInterstitialClosed;

    [SerializeField]
    private bool ready;

    public bool IsRewardVideoAvailable => Advertisements.Instance.IsRewardVideoAvailable(); //  ready; // 
    public bool IsInterstitialAvailable => Advertisements.Instance.IsInterstitialAvailable(); //ready;//
    public bool WaitForInterstitial { get; set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        Advertisements.Instance.Initialize();
    }

    public void ShowRewardedAd(UnityAction<bool> completeMethod)
    {
        OnRewardedComplete = completeMethod;
        Debug.Log("ShowRewardedAd");
        Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
    }


    public void TryShowInterstitial()
    {
        if (IsInterstitialAvailable)
            ShowInterstitialAd(null);
    }

    public void ShowInterstitialAd(UnityAction completeMethod)
    {
        WaitForInterstitial = false;
        OnInterstitialClosed = completeMethod;

        Debug.Log("ShowInterstitialAd");
        //interstitialAdManager.ShowInterstitialAd();
        Advertisements.Instance.ShowInterstitial(InterstitialClosed);
    }

    private void InterstitialClosed(string advertiser)
    {
        Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
        OnInterstitialClosed?.Invoke();
        OnInterstitialClosed = null;
    }

    private void CompleteMethod(bool completed, string advertiser)
    {
        OnRewardedComplete?.Invoke(completed);
        OnRewardedComplete = null;
    }
}