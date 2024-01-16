//----------------------------------------------
//           		 Stunt Crasher
//
// Copyright © 2014 - 2020 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Ui.Components;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game manager. Controls overall behavior of the scene. Spawns player vehicles, controls UI, calculates score, etc...
/// </summary>
public class C_GameManager : MonoBehaviour
{
    #region Singleton

    private static C_GameManager instance;

    public static C_GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<C_GameManager>();
            return instance;
        }
    }

    #endregion

    [SerializeField]
    private GameObject canvas;
    
    [Header("Menus")]
    public GameObject mainMenu;

    public GameObject vehicleSelectMenu;
    public GameObject sceneSelectMenu;
    public GameObject lockedSceneMenu;
    public GameObject gameMenu;
    public GameObject finishMenu;

    [Space()]
    [Header("Player")]
    public C_CarController playerVehicle;

    public C_Camera playerCamera;
    public Transform spawnPoint;
    public Transform finishPoint;
    private Vector3 launchPosition;
    private C_CarController[] allPlayerVehicles;

    [Space()]
    [Header("Stats")]
    public float speed = 0f;

    public float launchSpeed = 0f;
    public float distance = 0f;
    public float maxDistance = 500f;

    private int selectedVehicle = 0;
    private int selectedScene = 1;

    [Header("Score")]
    public int score = 0;

    public int crashScore = 0;
    public int bonusScore = 0;
    public float scoreMultiplier = 1f;

    public Text collectValueText;
    
    [Space()]
    [Header("UI")]
    public Text[] cashText;

    public Text[] speedText;
    public Text[] scoreText;
    public Text[] launchspeedText;
    public Text[] distanceText;
    public Text[] maxDistanceText;
    public Text[] destructionText;
    public Text[] bonusText;

    public Button selectVehicleButton;
    public Button buyVehicleButton;
    public Button restartSceneButton;
    public Button nextSceneButton;

    [Space()]
    public Slider launchSpeedSlider;

    public Slider distanceSlider;

    public GameObject speedNeedle;
    public float speedNeedleMinAngle = 0f;
    public float speedNeedleMaxAngle = -275f;

    [Header("Upgrades")]
    public UpgradeButton upgradeEngine;

    public UpgradeButton upgradeBoost;
    public UpgradeButton upgradeBonus;

    [Space()]
    public Text upgradeEngineLevel;

    public Text upgradeEngineLevelPrice;
    public Text upgradeBoostLevel;
    public Text upgradeBoostLevelPrice;
    public Text upgradeBonusLevel;
    public Text upgradeBonusLevelPrice;

    [Space()]
    private C_Finisher finisher;

    public bool gameStarted = false;
    public bool gameCompleted = false;
    public bool sceneUnlockedAsDefault = false;

    #region EVENTS

    public delegate void onGameStarted();

    public static event onGameStarted OnGameStarted;

    public delegate void onGameFinished();

    public static event onGameFinished OnGameFinished;

    public delegate void onPlayerLaunched();

    public static event onPlayerLaunched OnPlayerLaunched;

    #endregion

    [SerializeField]
    private TapZone tapZone;

    [SerializeField]
    private UpgradeButton[] upgradeButtons;

    private CanvasGroup canvasGroup;
    private UIController uiController;

    void Awake()
    {
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        //  Locking target frame rate per second (FPS).
        Application.targetFrameRate = C_Settings.Instance.targetFrameRate;

        //	If this scene is unlocked as default, save it as an unlocked scene.
        if (sceneUnlockedAsDefault)
            C_API.UnlockLevel(SceneManager.GetActiveScene().buildIndex);

        upgradeButtons = new[] { upgradeEngine, upgradeBoost, upgradeBonus };
        uiController = canvas.GetComponent<UIController>();
    }

    void Start()
    {
        //	Opening main menu at the beggining.
        OpenMenu(mainMenu);

        //	Updating all cash texts at the beginning.
        foreach (var item in cashText)
            item.text = C_API.GetCurrency().ToString("F0");

        //	Spawns and stores all player vehicles.
        SpawnAllVehicles();

        //	Finds finisher location.
        finisher = GameObject.FindObjectOfType<C_Finisher>();

        //	If found, set maximum distance.
        if (finisher)
        {
            finishPoint = finisher.transform;
            maxDistance = Vector3.Distance(spawnPoint.position, finishPoint.position);
        }

        foreach (var text in maxDistanceText)
            text.text = maxDistance.ToString("F0") + " M";


        if (AdsManager.Instance.WaitForInterstitial)
            AdsManager.Instance.ShowInterstitialAd(null);

        SelectRandomUpgradeForAds();
        CheckAllUpgrades();
    }

    private void SelectRandomUpgradeForAds()
    {
        int value = Random.Range(0, upgradeButtons.Length);

        for (var i = 0; i < upgradeButtons.Length; i++)
            upgradeButtons[i].CanUpgradeByVideo = i == value;
    }

    void OnEnable()
    {
        // Listening an event when player cash is updated.
        C_API.OnPlayerCoinsChanged += C_API_OnPlayerCoinsChanged;
    }

    /// <summary>
    /// When player cash is changed...
    /// </summary>
    /// <param name="changeAmount">Change amount.</param>
    void C_API_OnPlayerCoinsChanged(int changeAmount)
    {
        foreach (var item in cashText)
            item.text = C_API.GetCurrency().ToString("F0");

        CheckAllUpgrades();
    }

    void Update()
    {
        // If there are no any player vehicle, return.
        if (!playerVehicle)
            return;

        //	If game isn't started, return.
        if (!gameStarted)
            return;

        //	Calculating jump distance.
        if (launchPosition != Vector3.zero)
            distance = Vector3.Distance(playerVehicle.transform.position, launchPosition);

        speed = playerVehicle.speed; //	Speed of the player vehicle.
        crashScore = playerVehicle.crashScore; //	Crash score of the player vehicle.
        scoreMultiplier =
            Mathf.Lerp(1f, 10f,
                (float)playerVehicle.currentBonusLevel /
                100f); //  Score multiplier range from 1 to 10 depending on player vehicle's bonus level.
        bonusScore = (int)((crashScore + distance) * scoreMultiplier); //  Calculating total score.
        bonusScore -= (int)(crashScore + distance); //  Calculating only bonus score.
        score = (int)((crashScore + distance) * scoreMultiplier + speed); //	Total score of the player vehicle.
        collectValueText.text = score.ToString();
        
        //	Updating all speed texts.
        foreach (var item in speedText)
            item.text = speed.ToString("F0");

        //	Updating all launch speed texts.
        foreach (var item in launchspeedText)
            item.text = launchSpeed != 0 ? launchSpeed.ToString("F0") : "";

        //	Updating distance texts.
        foreach (var item in distanceText)
            item.text = distance.ToString("F0");

        //	Updating score texts.
        foreach (var item in scoreText)
            item.text = score.ToString("F0");

        //	Updating destruction score texts.
        foreach (var item in destructionText)
            item.text = crashScore.ToString("F0");

        //  Updating bonus score texts.
        foreach (var item in bonusText)
            item.text = bonusScore.ToString("F0");

        // Updating launch speed and distance sliders.
        launchSpeedSlider.value = playerVehicle.boostTime;
        distanceSlider.value = Mathf.Lerp(0f, 1f, distance / maxDistance);

        //	Rotation of the speed needle.
        speedNeedle.transform.localEulerAngles = new Vector3(speedNeedle.transform.localEulerAngles.x,
            speedNeedle.transform.localEulerAngles.y,
            Mathf.Lerp(speedNeedleMinAngle, speedNeedleMaxAngle, speed / 260f));

        //	If game is on and player vehicle is totally crashed, finish the game.
        if (gameStarted && playerVehicle.isCrashed && playerVehicle.speed < 1f)
            FinishGame();
    }

    /// <summary>
    /// Spawns all vehicles.
    /// </summary>
    public void SpawnAllVehicles()
    {
        //	Getting all selectable player vehicles from C_PlayerVehicles scriptable object located in "Resources" folder.
        allPlayerVehicles = new C_CarController[C_PlayerVehicles.Instance.vehicles.Length];

        //	Instantiate all selectable player vehicles
        for (int i = 0; i < C_PlayerVehicles.Instance.vehicles.Length; i++)
        {
            GameObject newCar = GameObject.Instantiate(C_PlayerVehicles.Instance.vehicles[i].carController.gameObject,
                spawnPoint.position, spawnPoint.rotation);
            newCar.gameObject.SetActive(false);
            allPlayerVehicles[i] = newCar.GetComponent<C_CarController>();
        }

        //	Getting lastly selected vehicle index.
        selectedVehicle = PlayerPrefs.GetInt("SelectedVehicle", 0);

        //	Getting lastly selected scene index.
        selectedScene = PlayerPrefs.GetInt("SelectedScene", 0);

        //	Spawns selected vehicle.
        SpawnVehicle();
    }

    /// <summary>
    /// Nexts the vehicle.
    /// </summary>
    public void NextVehicle()
    {
        if (selectedVehicle < C_PlayerVehicles.Instance.vehicles.Length - 1)
            selectedVehicle++;
        else
            selectedVehicle = 0;

        SpawnVehicle();
    }

    /// <summary>
    /// Previouses the vehicle.
    /// </summary>
    public void PreviousVehicle()
    {
        if (selectedVehicle > 0)
            selectedVehicle--;
        else
            selectedVehicle = C_PlayerVehicles.Instance.vehicles.Length - 1;

        SpawnVehicle();
    }

    /// <summary>
    /// Spawns the selected player vehicle.
    /// </summary>
    public void SpawnVehicle()
    {
        // Disabling all vehicles.
        for (int i = 0; i < allPlayerVehicles.Length; i++)
            allPlayerVehicles[i].gameObject.SetActive(false);

        // And only enabling selected vehicle.
        playerVehicle = allPlayerVehicles[selectedVehicle].GetComponent<C_CarController>();
        playerVehicle.isControllable = false;
        playerVehicle.gameObject.SetActive(true);
        playerVehicle.rigid.maxAngularVelocity = 10f;
        playerVehicle.rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY |
                                          RigidbodyConstraints.FreezeRotationZ;

        //	Checks all upgrade stats of the selected vehicle.
        CheckAllUpgrades();

        //  If selected vehicle price is 0, purchase it automatically.
        if (C_PlayerVehicles.Instance.vehicles[selectedVehicle].price <= 0)
            C_PlayerPrefsX.SetBool(C_PlayerVehicles.Instance.vehicles[selectedVehicle].carController.name, true);

        // Enablind/ disabling select/buy buttons.
        bool vehicleUnlocked =
            C_PlayerPrefsX.GetBool(C_PlayerVehicles.Instance.vehicles[selectedVehicle].carController.name, false);

        if (vehicleUnlocked)
        {
            selectVehicleButton.gameObject.SetActive(true);
            buyVehicleButton.gameObject.SetActive(false);
            buyVehicleButton.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            selectVehicleButton.gameObject.SetActive(false);
            buyVehicleButton.gameObject.SetActive(true);
            buyVehicleButton.GetComponentInChildren<Text>().text =
                "Buy Vehicle " + C_PlayerVehicles.Instance.vehicles[selectedVehicle].price.ToString();

            if (C_API.GetCurrency() < C_PlayerVehicles.Instance.vehicles[selectedVehicle].price)
                buyVehicleButton.interactable = false;
            else
                buyVehicleButton.interactable = true;
        }
    }

    /// <summary>
    /// Selects the vehicle.
    /// </summary>
    public void SelectVehicle()
    {
        PlayerPrefs.SetInt("SelectedVehicle", selectedVehicle); //	Saves lastly selected vehicle index.
        playerVehicle = allPlayerVehicles[selectedVehicle]; //	Assings selected vehicle.
    }

    /// <summary>
    /// Selects the scene.
    /// </summary>
    public void SelectScene(int sceneIndex)
    {
        bool sceneUnlocked = C_API.CheckLevel(sceneIndex);

        if (sceneUnlocked)
        {
            PlayerPrefs.SetInt("SelectedScene", sceneIndex);
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            lockedSceneMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Purchases the selected vehicle.
    /// </summary>
    public void BuyVehicle()
    {
        int currentCash = C_API.GetCurrency();

        //	If current cash is enough, purchase it. Otherwise, it fails.
        if (currentCash >= C_PlayerVehicles.Instance.vehicles[selectedVehicle].price)
        {
            C_PlayerPrefsX.SetBool(C_PlayerVehicles.Instance.vehicles[selectedVehicle].carController.name, true);
            C_API.ConsumeCurrency(C_PlayerVehicles.Instance.vehicles[selectedVehicle].price);
            SpawnVehicle();
        }
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Started!");

        //	Opens gameplay menu when game starts.
        OpenMenu(gameMenu);

        gameStarted = true; //	Game started.
        playerVehicle.isControllable = true; //	Player vehicle is now controllable.
        playerCamera.playerCar = playerVehicle; //	Assigns target of the camera.

        //	Creates engine start SFX.
        C_AudioSource.NewAudioSource(playerVehicle.gameObject, playerVehicle.engineStartClip.name, 5f, 50f, 1f,
            playerVehicle.engineStartClip, false, true, true);

        // Firing an event when game starts.
        if (OnGameStarted != null)
            OnGameStarted();
    }

    /// <summary>
    /// Finishs the game.
    /// </summary>
    public void FinishGame()
    {
        Debug.Log("Finished!");
        tapZone.SetReward(score);

        // Opens gameover menu.
        OpenMenu(finishMenu);

        gameStarted = false; //	Game finished.
        C_API.AddCurrency(score); //	Adds and saves new cash.

        //	If player achieves the maximum distance, unlock next level.
        if (gameCompleted)
            C_API.UnlockLevel(SceneManager.GetActiveScene().buildIndex + 1);

        restartSceneButton.gameObject.SetActive(!gameCompleted);
        nextSceneButton.gameObject.SetActive(gameCompleted);

        // Firing an event when game finishes.
        if (OnGameFinished != null)
            OnGameFinished();

//		finishMenu.BroadcastMessage("Animate");
        finishMenu.BroadcastMessage("GetNumber");
    }

    /// <summary>
    /// Launch player vehicle.
    /// </summary>
    public void Launch()
    {
        Debug.Log("Launched!");

        launchPosition = playerVehicle.transform.position; //	Getting launch position.
        launchSpeed = playerVehicle.speed; //	Getting launch speed.
        playerVehicle.rigid.constraints = RigidbodyConstraints.None; //	Player vehicle is now free at air.
        playerVehicle.isControllable = false; //	Player vehicle is now not controllable.	
        playerCamera.distance = 15f; //	Setting distance of the camera when player vehicle launched.
        playerCamera.gameObject.GetComponentInChildren<Animator>().SetTrigger("Launch");

        if (OnPlayerLaunched != null)
            OnPlayerLaunched();
    }

    /// <summary>
    /// Upgrades the engine.
    /// </summary>
    public void UpgradeEngine()
    {
        // Getting and saving upgrade state of the vehicle.
        int currentUpgradeLevel = playerVehicle.currentEngineLevel;

        if (currentUpgradeLevel >= 100)
            return;

        if (upgradeEngine.CanUpgradeByVideo)
        {
            AdsManager.Instance.ShowRewardedAd(TryUpgradeEngine);
        }
        else
        {
            C_API.ConsumeCurrency(playerVehicle.nextPriceForEngineUpgrade);
            UpgradeEngine2();
        }
    }

    private void TryUpgradeEngine(bool isSucceed)
    {
        if (isSucceed)
        {
            upgradeEngine.CanUpgradeByVideo = false;
            UpgradeEngine2();
        }
    }

    private void UpgradeEngine2()
    {
        int currentUpgradeLevel = playerVehicle.currentEngineLevel;
        currentUpgradeLevel += 1;
        PlayerPrefs.SetInt(playerVehicle.transform.name + "_EngineLevel", currentUpgradeLevel);

        // Checks all upgrades after purchasing.
        CheckAllUpgrades();
    }

    /// <summary>
    /// Upgrades the boost.
    /// </summary>
    public void UpgradeBoost()
    {
        // Getting and saving upgrade state of the vehicle.
        int currentUpgradeLevel = playerVehicle.currentBoostLevel;

        if (currentUpgradeLevel >= 100)
            return;

        if (upgradeBoost.CanUpgradeByVideo)
        {
            AdsManager.Instance.ShowRewardedAd(TryUpgradeBoost);
        }
        else
        {
            C_API.ConsumeCurrency(playerVehicle.nextPriceForBoostUpgrade);
            UpgradeBoost2();
        }
    }


    private void TryUpgradeBoost(bool isSucceed)
    {
        if (isSucceed)
        {
            upgradeBoost.CanUpgradeByVideo = false;
            UpgradeBoost2();
        }
    }

    private void UpgradeBoost2()
    {
        int currentUpgradeLevel = playerVehicle.currentBoostLevel;
        currentUpgradeLevel += 1;
        PlayerPrefs.SetInt(playerVehicle.transform.name + "_BoostLevel", currentUpgradeLevel);

        // Checks all upgrades after purchasing.
        CheckAllUpgrades();
    }

    /// <summary>
    /// Upgrades the bonus.
    /// </summary>
    public void UpgradeBonus()
    {
        // Getting and saving upgrade state of the vehicle.
        int currentUpgradeLevel = playerVehicle.currentBonusLevel;

        if (currentUpgradeLevel >= 100)
            return;

        if (upgradeBonus.CanUpgradeByVideo)
        {
            AdsManager.Instance.ShowRewardedAd(TryUpgradeBonus);
        }
        else
        {
            C_API.ConsumeCurrency(playerVehicle.nextPriceForBonusUpgrade);
            UpgradeBonus2();
        }
    }


    private void TryUpgradeBonus(bool isSucceed)
    {
        if (isSucceed)
        {
            upgradeBonus.CanUpgradeByVideo = false;
            UpgradeBonus2();
        }
    }

    private void UpgradeBonus2()
    {
        int currentUpgradeLevel = playerVehicle.currentBonusLevel;

        currentUpgradeLevel += 1;
        PlayerPrefs.SetInt(playerVehicle.transform.name + "_BonusLevel", currentUpgradeLevel);
        // Checks all upgrades after purchasing.
        CheckAllUpgrades();
    }

    /// <summary>
    /// Checks all upgrades.
    /// </summary>
    public void CheckAllUpgrades()
    {
        // If there are no any player vehicle, return.
        if (!playerVehicle)
            return;

        //	Updating all upgrade stats.
        upgradeEngineLevel.text = "Level: " + playerVehicle.currentEngineLevel.ToString("F0") + " / 100";
        upgradeBoostLevel.text = "Level: " + playerVehicle.currentBoostLevel.ToString("F0") + " / 100";
        upgradeBonusLevel.text = "Level: " + playerVehicle.currentBonusLevel.ToString("F0") + " / 100";

        upgradeEngineLevelPrice.text = playerVehicle.nextPriceForEngineUpgrade.ToString("F0");
        upgradeBoostLevelPrice.text = playerVehicle.nextPriceForBoostUpgrade.ToString("F0");
        upgradeBonusLevelPrice.text = playerVehicle.nextPriceForBonusUpgrade.ToString("F0");

        int currentCash = C_API.GetCurrency(); //	Getting current cash.

        //	If current cash is more than target price of the upgrade, make the button interactable.
        if (currentCash >= playerVehicle.nextPriceForEngineUpgrade && playerVehicle.currentEngineLevel < 100 &&
            upgradeEngine.CanUpgradeByVideo == false ||
            upgradeEngine.CanUpgradeByVideo && AdsManager.Instance.IsRewardVideoAvailable)
            upgradeEngine.Unlock();
        else
            upgradeEngine.Lock();

        //	If current cash is more than target price of the upgrade, make the button interactable.
        if (currentCash >= playerVehicle.nextPriceForBoostUpgrade && playerVehicle.currentBoostLevel < 100 &&
            upgradeBoost.CanUpgradeByVideo == false||
            upgradeBoost.CanUpgradeByVideo && AdsManager.Instance.IsRewardVideoAvailable)
            upgradeBoost.Unlock();
        else
            upgradeBoost.Lock();

        //	If current cash is more than target price of the upgrade, make the button interactable.
        if (currentCash >= playerVehicle.nextPriceForBonusUpgrade && playerVehicle.currentBonusLevel < 100 &&
            upgradeBonus.CanUpgradeByVideo == false||
            upgradeBonus.CanUpgradeByVideo && AdsManager.Instance.IsRewardVideoAvailable)
            upgradeBonus.Unlock();
        else
            upgradeBonus.Lock();
    }

    /// <summary>
    /// Opens the target UI menu.
    /// </summary>
    /// <param name="activeMenu">Active menu.</param>
    public void OpenMenu(GameObject activeMenu)
    {
        //	Disabling all UI menus.
        mainMenu.SetActive(false);
        vehicleSelectMenu.SetActive(false);
        sceneSelectMenu.SetActive(false);
        lockedSceneMenu.SetActive(false);
        gameMenu.SetActive(false);
        finishMenu.SetActive(false);

        // And activating only target UI menu.
        if (activeMenu)
            activeMenu.SetActive(true);
    }

    /// <summary>
    /// Restart the scene.
    /// </summary>
    public void Restart()
    {
        AdsManager.Instance.WaitForInterstitial = true;
        ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    #region Multiply

    private int _multiplier;


    public void TryMultiply()
    {
        if (AdsManager.Instance.IsRewardVideoAvailable)
        {
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        uiController.Qq();
        canvasGroup.blocksRaycasts = false;
        _multiplier = tapZone.Use();

        yield return new WaitForSeconds(1);
        
        AdsManager.Instance.ShowRewardedAd(GetMultiplyReward);
    }

    private void Multiply()
    {
        C_API.AddCurrency(score * _multiplier - score);
        ReloadScene();
    }

    private void GetMultiplyReward(bool isSucceed)
    {
        if (isSucceed)
            Multiply();
        else
            Restart();
    }

    #endregion

    /// <summary>
    /// Checks and loads the next scene.
    /// </summary>
    public void CheckAndLoadNextScene()
    {
        C_API.NextLevel(selectedScene + 1);
    }

    void OnDisable()
    {
        C_API.OnPlayerCoinsChanged -= C_API_OnPlayerCoinsChanged;
    }

    // TESTING METHODS

    /// <summary>
    /// Adds the money.
    /// </summary>
    public void AddMoney()
    {
        C_API.AddCurrency(10000);
    }

    /// <summary>
    /// Unlocks all vehicles.
    /// </summary>
    public void UnlockVehicles()
    {
        for (int i = 0; i < C_PlayerVehicles.Instance.vehicles.Length; i++)
        {
            C_PlayerPrefsX.SetBool(C_PlayerVehicles.Instance.vehicles[i].carController.name, true);
        }

        SpawnVehicle();
    }

    /// <summary>
    /// Unlocks all levels.
    /// </summary>
    public void UnlockLevels()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            C_API.UnlockLevel(i);
        }
    }

    /// <summary>
    /// Resets everything and reloads the scene.
    /// </summary>
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}