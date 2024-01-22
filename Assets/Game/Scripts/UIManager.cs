using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public CoinManager coinManager;
    [Header("Shop-equipBTN")]
    [SerializeField] private List<Button> EquipBTNList;
    [SerializeField] public List<Button> PriceBTNList;
    [SerializeField] public List<TextMeshProUGUI> PriceTextList;
    [SerializeField] private List<TextMeshProUGUI> StatusList;
    [SerializeField] private Sprite EquipStatus;
    [SerializeField] private Sprite OwnStatus;

    [Header("Menu - Game")]
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Game;


    public event EventHandler OnLevelChange;
    [Header("Nitro")]
    [SerializeField] private Button NitroBtn;
    [SerializeField] private Image NitroImg;

    [Header("Gold Text")]
    [SerializeField] private Text GoldText;

    [Header("InGamePanel")]
    [SerializeField] private GameObject LosePanelUI;
    [SerializeField] private GameObject FinishLevelUI;
    [SerializeField] private GameObject GameplayUI;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject SelectMapPanel;
    
    [Header("InMenu")]
    [SerializeField] private GameObject MenuPanelUI;
    [SerializeField] private GameObject ShopPanelUI;
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject DailyRewardPanel;
    [SerializeField] private GameObject OfferPanel;
    [SerializeField] private GameObject OnlineGiftPanel;
    [SerializeField] private GameObject ShopGoldPanel;
    [SerializeField] private GameObject LoadScenePanel;

    [Header("GamePlayUI")]
    [SerializeField] private Toggle SteeingWheelOnBtn;
    [SerializeField] private GameObject ArrowHorizontal;
    [SerializeField] private GameObject SteeringWheel;
    [SerializeField] private Text EarningAmount;

    [Header("ShopUI")]
    [SerializeField] private GameObject DoYouWantToBuyPanel;
    [SerializeField] private Text DoYouWantToBuyText;

    [Header("OnlineGiftUI")]
    [SerializeField] private int GoldEarnAmount;
    [SerializeField] private Button OpenOnlineGiftBTN;
    [SerializeField] private TextMeshProUGUI TimeCountDownText;
    [SerializeField] private float TimeRemaining;
    private float remainingTime;
    [SerializeField] private Button ClaimBTN;
    [SerializeField] private Button ClaimX2BTN;

    [Header("LoadScene")]
    [SerializeField] private Image LoadSceneFillAmount;
    public float LoadSceneTimer = 1f;
    private float LoadSceneLast = 0;
    [Header("AddGoldButton")]
    public List<Button> AddGoldButtonWhenBuy;
    public List<Button> GoldSpamFromMiddle;
    [Header("OfferPanel")]
    [SerializeField] private Button BuyOfferButton;
    void Start()
    {
        StartCoroutine(LoadScene());
        remainingTime = TimeRemaining;
        Home();
        GameController.Ins.Load();
        GameController.Ins.Save();
        LoadCurrentCar();
        LoadAndSetActiveObjects();
        if (PlayerPrefs.GetInt("BoughtOffer") == 0){
            BuyOfferButton.interactable = true;
        }
        else
        {
            BuyOfferButton.interactable = false;
        }
        for (int i = 0; i < AddGoldButtonWhenBuy.Count; i++)
        {
            int buttonIndex = i; // Capture the current index in a local variable for the lambda expression
            AddGoldButtonWhenBuy[i].onClick.AddListener(() => OnButtonClick1(buttonIndex));
        }
        for (int i = 0; i < GoldSpamFromMiddle.Count; i++)
        {
            int buttonIndex = i; // Capture the current index in a local variable for the lambda expression
            GoldSpamFromMiddle[i].onClick.AddListener(() => OnButtonClick2(buttonIndex));
        }
    }
    //Catch RectTransform of button
    void OnButtonClick1(int buttonIndex)
    {
        RectTransform buttonRectTransform = AddGoldButtonWhenBuy[buttonIndex].GetComponent<RectTransform>();
        RewardManager.Ins.RewardPileOfCoinWhenBuy(buttonRectTransform, 6);
    }
    void OnButtonClick2(int buttonIndex)
    {
        RectTransform buttonRectTransform = GoldSpamFromMiddle[buttonIndex].GetComponent<RectTransform>();
        RewardManager.Ins.RewardPileOfCoin(buttonRectTransform, 6);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTimeRemainingForOnlineGift();
        CheckStreeingWheelisOn();
        LoadSceneLast += Time.deltaTime;
        NitroImg.fillAmount = CarController.Ins.NitroTimer / CarController.Ins.NitroLast;
        LoadSceneFillAmount.fillAmount = LoadSceneLast / LoadSceneTimer;
    }

    //LoadScene
    
    public void ActiveLoadScenePanel()
    {
        LoadSceneLast = 0;
        LoadScenePanel.gameObject.SetActive(true);
        ShopGoldPanel.gameObject.SetActive(false);
    }
    public void DeActiveLoadScenePanel()
    {
        LoadScenePanel.gameObject.SetActive(false);
    }
    IEnumerator LoadScene()
    {
        ActiveLoadScenePanel();
        yield return new WaitForSeconds(LoadSceneTimer);
        DeActiveLoadScenePanel();
    }


    //InGameUI
    public void UpdateGoldText()
    {
        //GoldText.text = "Gold : " + CarController.Ins.GoldInGame;
        //EarningAmount.text = "Earning amount this game :" + CarController.Ins.GoldInGame;
    }
    public void Lose()
    {
        LosePanelUI.SetActive(true);
        GameplayUI.SetActive(false);
    }
    public void Finish()
    {
        //CameraFollow.Ins.ActiveFireWorkParticle();
        TestCamera.Ins.ActiveFireWorkParticle();
        FinishLevelUI.SetActive(true);
        GameplayUI.SetActive(false);
    }
    private void CheckStreeingWheelisOn()
    {
        if (SteeringWheel.activeSelf)
        {
            ArrowHorizontal.SetActive(false);
        }
        else
        {
            ArrowHorizontal.SetActive(true);
        }
    }
    public void ReplayStage()
    {
        StartCoroutine(LoadScene());
        //CameraFollow.Ins.ResetCamAng();
        LosePanelUI.SetActive(false);
        GameplayUI.SetActive(true);
        SpawnLevel.Ins.SpawnPlayer();
        SpawnLevel.Ins.SpawnLevelMap();
        CarController.Ins.ResetItemCount();
        UpdateGoldText();
    }

    public void OnNitro()
    {
        if (CarController.Ins.CanActiveNitro())
        {
            Debug.Log("Nitro");
            CarController.Ins.isNitroActive = true;
        }
        else
        {
            Debug.Log("Run out of Nitro");
        }
    }
    public void NextStage()
    {
        StartCoroutine(LoadScene());
        //CameraFollow.Ins.ResetCamAng();
        CarController.Ins.ResetItemCount();
        UpdateGoldText();
        //CameraFollow.Ins.InActiveFireWorkParticle();
        TestCamera.Ins.InActiveFireWorkParticle();
        SpawnLevel.Ins.SpawnPlayer();
        SpawnLevel.Ins.SpawnLevelMap();
        CarController.Ins.ResetItemCount();
        FinishLevelUI.SetActive(false);
        GameplayUI.SetActive(true);
    }
    //InGameUI

    //MenuUI
    public void Shop()
    {
        ShopPanelUI.SetActive(true);
    }
    public void ShopBackToMenu()
    {
        ShopPanelUI.SetActive(false);
    }
    //MenuUI
    public void Home()
    {
        Menu.SetActive(true);
        Game.SetActive(false);
    }
    public void PlayGame()
    {
        Menu.SetActive(false);
        Game.SetActive(true);
        StartCoroutine(LoadScene());
    }

    public void PlayGameAfterSelectMap()
    {
        StartCoroutine(LoadScene());
        SelectMapPanel.gameObject.SetActive(false);
        GameplayUI.gameObject.SetActive(true);
    }

    public void EquipCar()
    {
        GameController.Ins.carSelectionIndex = EquipBTNList.IndexOf(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        Debug.Log(GameController.Ins.carSelectionIndex);
        SpawnLevel.Ins.SpawnPlayer();
        int n = EquipBTNList.IndexOf(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        StatusList[n].text = "Selected";
        EquipBTNList[n].GetComponent<Image>().sprite = EquipStatus;
        for (int i = 0; i < StatusList.Count; i++)
        {
            if (i != n)
            {
                StatusList[i].text = "Owned";
                EquipBTNList[i].GetComponent<Image>().sprite = OwnStatus;
            }
        }
        PlayerPrefs.SetInt("CurrentCar",GameController.Ins.carSelectionIndex);
    }

    public void LoadCurrentCar()
    {
        int n = GameController.Ins.carSelectionIndex;
        StatusList[n].text = "Selected";
        EquipBTNList[n].GetComponent<Image>().sprite = EquipStatus;
        for (int i = 0; i < StatusList.Count; i++)
        {
            if (i != n)
            {
                StatusList[i].text = "Owned";
                EquipBTNList[i].GetComponent<Image>().sprite = OwnStatus;
            }
        }
    }

    int EquipBTNIndex;
    int carPrice;
    public void ExceptBuyCar()
    {
        EquipBTNIndex = PriceBTNList.IndexOf(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        DoYouWantToBuyPanel.SetActive(true);
        string text = PriceTextList[EquipBTNIndex].text;
        if (text.Length > 0)
        {
            // Lấy tất cả ký tự từ đầu đến ký tự trước cuối
            string result = text.Substring(0, text.Length - 1);
            DoYouWantToBuyText.text = "Do you want to buy " +result +"$" + " ?";
            carPrice = int.Parse(result);
        }
    }
    public void NotExceptBuyCar()
    {
        DoYouWantToBuyPanel.SetActive(false);
        carPrice = 0;
    }
    public void YesExceptBuyCar()
    {
        DoYouWantToBuyPanel.SetActive(false);
        if(carPrice <= GameController.Ins.TotalGold)
        {
            GameController.Ins.TotalGold -= carPrice;
            PriceBTNList[EquipBTNIndex].gameObject.SetActive(false);
            SaveObjectStates();
        }
        else
        {
            Debug.Log("ko đủ tiền, ko mua được");
        }
        GameController.Ins.Save();
    }
    public void OnPause()
    {
        PausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void ClosePausePanel()
    {
        PausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void ClosePausePanelButStillPause()
    {
        PausePanel.gameObject.SetActive(false);
    }
    public void OpenSettingPanel()
    {
        SettingPanel.gameObject.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        SettingPanel.gameObject.SetActive(false);
    }

    public void OpenPausePanel()
    {
        if (Game.activeSelf)
        {
            PausePanel.gameObject.SetActive(true);
        }
    }

    public void OpenDailyRewardPanel()
    {
        DailyRewardPanel.gameObject.SetActive(true);
    }
    public void CloseDailyRewardPanel()
    {
        DailyRewardPanel.gameObject.SetActive(false);
    }
    public void OpenMenuPanel()
    {
        MenuPanelUI.gameObject.SetActive(true);
    }
    public void CloseMenuPanel()
    {
        MenuPanelUI.gameObject.SetActive(false);
    }

    public void OpenOfferPanel()
    {
        OfferPanel.gameObject.SetActive(true);
    }
    public void CloseOfferPanel()
    {
        OfferPanel.gameObject.SetActive(false);
    }
    public void OpenOnlineGiftPanel()
    {
        OnlineGiftPanel.gameObject.SetActive(true);
    }
    public void CloseOnlineGiftPanel()
    {
        OnlineGiftPanel.gameObject.SetActive(false);
    }
    public void OpenShopGoldPanel()
    {
        ShopGoldPanel.gameObject.SetActive(true);
    }
    public void CloseShopGoldPanel()
    {
        ShopGoldPanel.gameObject.SetActive(false);
    }


    public void CollectDailyReward()
    {
        Daily.Ins.CollectedDailyReward();
        Daily.Ins.DisableClaimBtn();
    }
    public void Break()
    {
        CarController.Ins.OnBrake();
    }
    public void UnBreak()
    {
        CarController.Ins.OnUnBrake();
    }

    //UpdateTimeForOnlineGift
    private void UpdateTimeRemainingForOnlineGift()
    {
        if(remainingTime > 0.2) {
            OpenOnlineGiftBTN.interactable = false;
            remainingTime -= Time.deltaTime;
            int minute = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            TimeCountDownText.text = string.Format("{0:00}:{1:00}", minute, seconds);
        }
        else
        {
            remainingTime = 0;
            int minute = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            TimeCountDownText.text = string.Format("{0:00}:{1:00}", minute, seconds);
            OpenOnlineGiftBTN.interactable = true;
            ClaimBTN.interactable = true;
            ClaimX2BTN.interactable = true;
        }
    }
    public void ClaimOnlineGift()
    {
        GameController.Ins.TotalGold += GoldEarnAmount;
        ClaimBTN.interactable = false;
        ClaimX2BTN.interactable = false;
        GameController.Ins.Save();
        TimeRemaining *= 2;
        remainingTime = TimeRemaining;
    }
    public void ClaimX2OnlineGift()
    {
        GameController.Ins.TotalGold += GoldEarnAmount * 2;
        ClaimBTN.interactable = false;
        ClaimX2BTN.interactable = false;
        GameController.Ins.Save();
        TimeRemaining *= 2;
        remainingTime = TimeRemaining;
    }
    //Lưu nút giá đã bị tắt hay chưa
    public void SaveObjectStates()
    {
        foreach (Button btn in PriceBTNList)
        {
            // Đặt khóa cho mỗi đối tượng
            string objectKey = btn.name;

            // Lưu trạng thái của đối tượng (isActive)
            int isActiveValue = btn.gameObject.activeSelf ? 1 : 0;

            // Lưu trạng thái vào PlayerPrefs với khóa là tên đối tượng
            PlayerPrefs.SetInt(objectKey, isActiveValue);
        }

        // Lưu thay đổi
        PlayerPrefs.Save();
    }
    public void LoadAndSetActiveObjects()
    {
        foreach (Button btn in PriceBTNList)
        {
            // Đặt khóa cho mỗi đối tượng
            string objectKey = btn.name;

            // Đọc trạng thái từ PlayerPrefs bằng cách sử dụng tên đối tượng làm khóa
            int isActiveValue = PlayerPrefs.GetInt(objectKey);
            //int isActiveValue = 1;
            // Thiết lập trạng thái `SetActive` của đối tượng theo giá trị đã lưu
            btn.gameObject.SetActive(isActiveValue == 1);
        }
    }
    public void PurchaseGold()
    {
            StartCoroutine(SaveGold());
    }
    IEnumerator SaveGold()
    {
        yield return new WaitForSecondsRealtime(1f);
        for(int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 1000;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void OpenSelectMapPanel()
    {
        SelectMapPanel.gameObject.SetActive(true);
    }
    public void CloseSelectMapPanel()
    {
        SelectMapPanel.gameObject.SetActive(false);
    }
    public void SelectToggle()
    {
        Debug.Log(ToggleGroup.Ins.GetActiveToggle());
        GameController.Ins.level = ToggleGroup.Ins.GetActiveToggle();
        ToggleGroup.Ins.SetBorder();
        GameController.Ins.Save();
        OnLevelChange?.Invoke(this, EventArgs.Empty);
    }
    public void BuyOffer()
    {
        StartCoroutine(SaveGold());
        PriceBTNList[10].gameObject.SetActive(false);
        SaveObjectStates();
        BuyOfferButton.interactable = false;
        PlayerPrefs.SetInt("BoughtOffer", 1);
        GameController.Ins.Save();
    }
}
