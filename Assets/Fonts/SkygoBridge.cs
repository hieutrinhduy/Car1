using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SkygoBridge : MonoBehaviour
{
    public static SkygoBridge instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    public int CanShowAd
    {
        get { return PlayerPrefs.GetInt("CanShowAd", 1); }
        set { PlayerPrefs.SetInt("CanShowAd", value); }
    }

   

    float lastTimeShowAd = 0;

    public float TimeShowAds
    {
        get { return PlayerPrefs.GetFloat("TimeShowAds", 45f); }
        set { PlayerPrefs.SetFloat("TimeShowAds", value); }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    #region ADS

    public void ShowBanner()
    {
    }

    public void HideBanner()
    {
    }

    public bool ShowInterstitial(UnityEvent onClose)
    {
        onClose?.Invoke();
        return true;
    }

    public bool ShowRewarded(UnityEvent onCompleted, UnityEvent onFailed)
    {
        onCompleted.Invoke();
        return true;
    }

    public bool ShowRewardedInterstitial(UnityEvent onSuccess, UnityEvent onFailed)
    {
        onSuccess?.Invoke();
        return true;
    }

    #endregion

    #region IAP

    public void PurchaseIAP(string sku, UnityEvent onSuccess)
    {
        onSuccess.Invoke();
    }

    public void RestorePurchase()
    {

    }

    #endregion

    #region Analytics

    public void LogEvent(string eventName)
    {
        Debug.Log(eventName);
    }

    #endregion

    #region Config

    public string GetConfig(string cfgName)
    {
        return "";
    }

    #endregion

    #region Social

    public void RateGame()
    {
    }

    public void ShareGame()
    {
    }

    #endregion

    #region Promotion

    public void SetupCrossPromotion(string clickUrl, string imageUrl, Vector2 size, int positionID)
    {
    }

    public void ShowPromotion()
    {
    }

    public void HidePromotion()
    {
    }

    #endregion

    //public static bool IsConnected(string hostedURL = "https://www.google.com")
    //{
    //    try
    //    {
    //        var htmlText = SunWebRequests.GetHtmlFromUrl(hostedURL);
    //        return htmlText != "";
    //    }
    //    catch (IOException)
    //    {
    //        return false;
    //    }
    //}
}