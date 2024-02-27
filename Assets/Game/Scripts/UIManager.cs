using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

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
    public Button NitroBtn;
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

    [Header("GoldImage")]
    public GameObject GoldImage;

    [Header("PlayerPrefs")]
    public Slider SpecialDrive;

    [Header("UnlockMapPanel")]
    public GameObject UnlockMapPanel;
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
        SetUpSteeringWheel();
        StartCoroutine(StartMenu());
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
        if (!Menu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPause();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnNitro();
            }
        }
        SetupSteeringWheelCheck();
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
    [Header("In Lose Panel")]
    public GameObject PlayAgainBtn;
    public GameObject PlayAgainBtnWhenTryLockedLevel;
    public void Lose()
    {
        if (GameController.Ins.IsTryingMap)
        {
            PlayAgainBtn.SetActive(false);
            PlayAgainBtnWhenTryLockedLevel.SetActive(true);
            LosePanelUI.SetActive(true);
            LosePanelAnimate.Ins.StartLosePanelAnimate();
            GameplayUI.SetActive(false);
            //DeathBorder.Ins.TurnOffAllDeathBorder();
            GameController.Ins.IsTryingMap = false;
        }
        else{
            PlayAgainBtn.SetActive(true);
            PlayAgainBtnWhenTryLockedLevel.SetActive(false);
            LosePanelUI.SetActive(true);
            LosePanelAnimate.Ins.StartLosePanelAnimate();
            GameplayUI.SetActive(false);
            //DeathBorder.Ins.TurnOffAllDeathBorder();
        }
    }
    public void Finish()
    {
        //CameraFollow.Ins.ActiveFireWorkParticle();
        TestCamera.Ins.ActiveFireWorkParticle();
        GoldImage.SetActive(true);
        FinishLevelUI.SetActive(true);
        GameplayUI.SetActive(false);
        FinishStageAnimate.Ins.StartFinishStagePanel();
        GameController.Ins.IsTryingMap = false;
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
        if (GameController.Ins.IsTryingMap)
        {
            ReplayStageWhenTryingLockedMap();
        }
        else
        {
            StartCoroutine(LoadScene());
            //CameraFollow.Ins.ResetCamAng();
            LosePanelUI.SetActive(false);
            GameplayUI.SetActive(true);
            GamePlayPanelAnimate.Ins.StartGamePlayPanel();
            //DeathBorder.Ins.TurnOnAllDeathBorder();
            SpawnLevel.Ins.SpawnPlayer();
            SpawnLevel.Ins.SpawnLevelMap();
            CarController.Ins.ResetItemCount();
            UpdateGoldText();
            AudioManager.Ins.SetPlayingMusic();
        }
    }
    public void ReplayStageWhenTryingLockedMap()
    {
        StartCoroutine(LoadScene());
        //CameraFollow.Ins.ResetCamAng();
        LosePanelUI.SetActive(false);
        GameplayUI.SetActive(true);
        GamePlayPanelAnimate.Ins.StartGamePlayPanel();
        //DeathBorder.Ins.TurnOnAllDeathBorder();
        CarController.Ins.ResetItemCount();
        ToggleGroup.Ins.TryLockedLevel();
        UpdateGoldText();
        AudioManager.Ins.SetPlayingMusic();
    }

    public void OnNitro()
    {
        if (CarController.Ins.CanActiveNitro())
        {
            GamePlayPanelAnimate.Ins.StopNitroAnimate();
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
        //GameplayUI.SetActive(true);
        AudioManager.Ins.PlayMainMenuBGM();
        GameController.Ins.IsTryingMap = false;
    }
    //InGameUI

    //MenuUI
    public void Shop()
    {
        ShopPanelUI.SetActive(true);
        ShopCarPanelAnimate.Ins.StartShopCarPanel();
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
        GoldImage.SetActive(true);
        GameplayUI.gameObject.SetActive(false);
        StartCoroutine(StartMenu());
        AudioManager.Ins.PlayMainMenuBGM();
        GameController.Ins.IsTryingMap = false;
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
        GamePlayPanelAnimate.Ins.StartGamePlayPanel();
        GoldImage.SetActive(false);
        AudioManager.Ins.SetPlayingMusic();
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
    [Header("In Pause Panel")]
    public GameObject RestartBTN;
    public GameObject RestartBTNWhenTryingLockedLevel;
    public void OnPause()
    {
        if (GameController.Ins.IsTryingMap)
        {
            RestartBTN.SetActive(false);
            RestartBTNWhenTryingLockedLevel.SetActive(true);
            PausePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
            CarController.Ins.Mute();
            CarController.Ins.CarAudioPause();
        }
        else
        {
            RestartBTN.SetActive(true);
            RestartBTNWhenTryingLockedLevel.SetActive(false);
            PausePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
            CarController.Ins.Mute();
            CarController.Ins.CarAudioPause();
        }
    }

    public void ClosePausePanel()
    {
        PausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
        if(PlayerPrefs.GetInt("Sound") == 1 ){
            CarController.Ins.UnMute();
            CarController.Ins.CarAudioUnPause();
        }
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void NonPause()
    {
        Time.timeScale = 1;
    }
    public void ClosePausePanelButStillPause()
    {
        PausePanel.gameObject.SetActive(false);
    }
    public void OpenSettingPanel()
    {
        SettingPanel.gameObject.SetActive(true);
        //StartSettingPanel();
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
        DailyRewardAnimate.Ins.StartDailyReward();
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
        StartOfferPanel();
    }
    public void CloseOfferPanel()
    {
        OfferPanel.gameObject.SetActive(false);

    }
    public void OpenOnlineGiftPanel()
    {
        OnlineGiftPanel.gameObject.SetActive(true);
        OnlineGiftPanelAnimate.Ins.StartOnlineGift();
    }
    public void CloseOnlineGiftPanel()
    {
        OnlineGiftPanel.gameObject.SetActive(false);
    }
    int tmp;
    public void OpenShopGoldPanel()
    {
        ShopGoldPanel.gameObject.SetActive(true);
        StartShopGoldPanel();
        if (Menu.activeSelf)
        {
            Menu.SetActive(false);
            tmp = 0;
        }
        else if (Game.activeSelf)
        {
            Game.SetActive(false);
            tmp = 1;
        }
    }
    public void CloseShopGoldPanel()
    {
        ShopGoldPanel.gameObject.SetActive(false);
        if(tmp == 0)
        {
            Menu.SetActive(true);
        }
        else
        {
            Game.SetActive(true);
        }
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
            TimeCountDownText.text = "Claim";
            OpenOnlineGiftBTN.interactable = true;
            ClaimBTN.interactable = true;
            ClaimX2BTN.interactable = true;
        }
    }
    public void ClaimOnlineGift()
    {
        PurchaseGold500();
        //GameController.Ins.TotalGold += GoldEarnAmount;
        ClaimBTN.interactable = false;
        ClaimX2BTN.interactable = false;
        GameController.Ins.Save();
        TimeRemaining *= 2;
        remainingTime = TimeRemaining;
    }
    public void ClaimX2OnlineGift()
    {
        PurchaseGold1000();
        ClaimBTN.interactable = false;
        ClaimX2BTN.interactable = false;
        GameController.Ins.Save();
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





    //AddGold
    IEnumerator SaveGold()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 1000;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void PurchaseGold1000()
    {
        StartCoroutine(SaveGold1000());
    }
    IEnumerator SaveGold1000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 100;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void PurchaseGold3000()
    {
        StartCoroutine(SaveGold3000());
    }
    IEnumerator SaveGold3000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for(int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 300;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void PurchaseGold5000()
    {
        StartCoroutine(SaveGold5000());
    }
    IEnumerator SaveGold5000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i <10; i++)
        {
            GameController.Ins.TotalGold += 500;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void PurchaseGold10000()
    {
        StartCoroutine(SaveGold10000());
    }
    IEnumerator SaveGold10000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 1000;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void PurchaseGold15000()
    {
        StartCoroutine(SaveGold15000());
    }
    IEnumerator SaveGold15000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 1500;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void PurchaseGold20000()
    {
        StartCoroutine(SaveGold20000());
    }
    IEnumerator SaveGold20000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 2000;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void PurchaseGold2000()
    {
        StartCoroutine(SaveGold2000());
    }
    IEnumerator SaveGold2000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 200;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void PurchaseGold4000()
    {
        StartCoroutine(SaveGold4000());
    }
    IEnumerator SaveGold4000()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 400;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void PurchaseGold500()
    {
        StartCoroutine(SaveGold500());
    }
    IEnumerator SaveGold500()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 10; i++)
        {
            GameController.Ins.TotalGold += 50;
            GameController.Ins.Save();
            GameController.Ins.Load();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    //AddGold
    ////AddGoldByAmount
    //public void AddGold(int goldAmout)
    //{

    //}
    //IEnumerator AddGold()
    //{
    //    yield return new WaitForSecondsRealtime(1f);
    //    for (int i = 0; i < 10; i++)
    //    {
    //        GameController.Ins.TotalGold += 1000;
    //        GameController.Ins.Save();
    //        GameController.Ins.Load();
    //        yield return new WaitForSecondsRealtime(0.1f);
    //    }
    //}



    public void OpenSelectMapPanel()
    {
        SelectMapPanel.gameObject.SetActive(true);
        SelectMapAnimate.Ins.StartSelectMapAnimate();
    }
    public void OpenSelectMapPanelWhileExitUnlockMapPanel()
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
        ToggleGroup.Ins.TurnOnPlayBtn();
    }
    public void ChangeLevelNotice()
    {
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

    //SoundEffect
    public void PlayClickSoundEffect()
    {
        AudioManager.Ins.PlayClickSoundEffect();
    }
    public void SetUpSteeringWheel()
    {
        if (PlayerPrefs.GetInt("SpecialDrive") == 0)
        {
            SteeringWheel.gameObject.SetActive(false);
            SpecialDrive.value = 0;
        }
        else
        {
            SteeringWheel.gameObject.SetActive(true);
            SpecialDrive.value = 1;
        }
    }
    public void SetupSteeringWheelCheck()
    {
        if(SpecialDrive.value == 1){
            SteeringWheel.gameObject.SetActive(true);
        }
        else
        {
            SteeringWheel.gameObject.SetActive(false);
        }
    }
    public void TurnOnSteeringWheel()
    {
        PlayerPrefs.SetInt("SpecialDrive", 0);
    }
    public void TurnOffSteeringWheel()
    {
        PlayerPrefs.SetInt("SpecialDrive", 1);
    }



    //UI Dotween 
    [Header("Animate UI")]
    public RectTransform OfferBtn;
    public RectTransform GiftBtn;
    public RectTransform GoldImageUI;
    public RectTransform StartGameBtn;
    public RectTransform DailyRewardBtn;
    public RectTransform ShopBtn;
    public RectTransform SettingBtn;


    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(LoadSceneTimer);
        DOTween.Kill(OfferBtn);
        DOTween.Kill(GiftBtn);
        OfferBtn.anchoredPosition = new Vector2(97f, OfferBtn.anchoredPosition.y);
        OfferBtn.DOAnchorPosX(-119.6001f, 0.5f).SetEase(Ease.InOutCubic);
        OfferBtn.DOScale(1.1f,0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        GiftBtn.anchoredPosition = new Vector2(97f, GiftBtn.anchoredPosition.y);
        GiftBtn.DOAnchorPosX(-119.6001f, 0.5f).SetEase(Ease.InOutCubic);
        GiftBtn.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        GoldImageUI.anchoredPosition = new Vector2(GoldImageUI.anchoredPosition.x, 46f);
        GoldImageUI.DOAnchorPosY(-99f, 0.5f).SetEase(Ease.InOutCubic);

        StartGameBtn.anchoredPosition = new Vector2(-285f, StartGameBtn.anchoredPosition.y);
        DailyRewardBtn.anchoredPosition = new Vector2(-297.4999f, DailyRewardBtn.anchoredPosition.y);
        ShopBtn.anchoredPosition = new Vector2(-297.4999f, ShopBtn.anchoredPosition.y);
        SettingBtn.anchoredPosition = new Vector2(-297.4999f, SettingBtn.anchoredPosition.y);

        StartGameBtn.DOAnchorPosX(287.5f, 0.7f).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(0.2f);

        DailyRewardBtn.DOAnchorPosX(275f, 0.7f).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(0.2f);

        ShopBtn.DOAnchorPosX(275f, 0.7f).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(0.2f);

        SettingBtn.DOAnchorPosX(275f, 0.7f).SetEase(Ease.InOutCubic);
    }

    [Header("Animate UI for Shop Gold")]
    public RectTransform SettingPanelRect;
    public void StartSettingPanel()
    {
        SettingPanelRect.localScale = Vector3.zero;
        SettingPanelRect.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
    }

    public RectTransform ShopGoldRect;
    public RectTransform BackFromShopGoldBtn;
    public RectTransform ShopGoldHeader;
    public List<RectTransform> Buttons;
    public void StartShopGoldPanel()
    {
        foreach (RectTransform button in Buttons)
        {
            DOTween.Kill(button);
            button.localScale = new Vector2(1f, 1f);
        }
        ShopGoldRect.localScale = Vector3.zero;
        ShopGoldRect.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        BackFromShopGoldBtn.anchoredPosition = new Vector2(-110f, BackFromShopGoldBtn.anchoredPosition.y);
        ShopGoldHeader.anchoredPosition = new Vector2(-101f, ShopGoldHeader.anchoredPosition.y);
        BackFromShopGoldBtn.DOAnchorPosX(142.5999f, 0.7f).SetEase(Ease.InOutCubic);
        ShopGoldHeader.DOAnchorPosX(324f, 0.7f).SetEase(Ease.InOutCubic);
            foreach (RectTransform button in Buttons)
            {
                button.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }
    }

    [Header("Animate UI for Offer")]
    public RectTransform BackFromOfferBtn;
    public RectTransform SpecialOffer;
    public RectTransform GoldTag;
    public RectTransform CarTag;
    public RectTransform OfferHeader;
    public RectTransform OfferBuyBtn;
    public void StartOfferPanel()
    {
        DOTween.Kill(OfferHeader);
        DOTween.Kill(OfferBuyBtn);
        OfferHeader.localScale = Vector2.one;
        OfferBuyBtn.localScale = Vector2.one;
        GoldTag.anchoredPosition = new Vector2(GoldTag.anchoredPosition.x, 837);
        CarTag.anchoredPosition = new Vector2(CarTag.anchoredPosition.x, -768f);
        SpecialOffer.localScale = Vector3.zero;
        SpecialOffer.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        BackFromOfferBtn.anchoredPosition = new Vector2(-108, BackFromOfferBtn.anchoredPosition.y);
        BackFromOfferBtn.DOAnchorPosX(102f, 0.7f).SetEase(Ease.InOutCubic);
        GoldTag.DOAnchorPosY(0, 0.7f).SetEase(Ease.InOutCubic);
        CarTag.DOAnchorPosY(0, 0.7f).SetEase(Ease.InOutCubic);
        OfferHeader.DOScale(1.09f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        OfferBuyBtn.DOScale(1.03f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    //More Gold Btn
    [SerializeField] Button MoreGoldButton;
    public Button MenuBtn;
    public Button NextLevelBtn;
    public void InactiveMoreGoldBtn()
    {
        MoreGoldButton.interactable = false;
        MenuBtn.interactable = false;
        NextLevelBtn.interactable = false;
    }
    public void ActiveMoreGoldBtn(float n)
    {
        StartCoroutine(ActiveMoreGold(n));
    }
    IEnumerator ActiveMoreGold(float n)
    {
        yield return new WaitForSeconds(n);
        MoreGoldButton.interactable = true;
        MenuBtn.interactable = true;
        NextLevelBtn.interactable = true;
    }

    //UnlockMap Panel
}
//Note for advertise version
//line 203
//line 204
//line 224
//line 274
//line 275
