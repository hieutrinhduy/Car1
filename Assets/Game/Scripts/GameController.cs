using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : Singleton<GameController>
{
    [SerializeField] private List<Text> TotalGoldText;
    public bool IsTryingMap;

    public int carSelectionIndex = 0;

    public int level = 1;
    public int TotalGold = 0;

    public override void Awake()
    {
        base.Awake();
        //Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Start()
    {
        IsTryingMap = false;
        carSelectionIndex = PlayerPrefs.GetInt("CurrentCar");
        Time.timeScale = 1;
        Load();
    }
    public void AddGold()
    {
        TotalGold += 1;
    }
    public void FinishMap()
    {
        int currentLevel = level;
        Debug.Log(currentLevel);
        while(level == currentLevel)
        {
            level = UnityEngine.Random.Range(0, SpawnLevel.Ins.Levels.Count);
            Debug.Log(level);
        }
    }
    public void Update() 
    {
        
    }
    public void Save()
    {
        foreach (Text goldtext in TotalGoldText)
        {
            goldtext.text = ""+ TotalGold;
        }
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Gold", TotalGold);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        foreach (Text goldtext in TotalGoldText)
        {
            goldtext.text = "" + TotalGold;
        }
        level = PlayerPrefs.GetInt("Level");
        TotalGold = PlayerPrefs.GetInt("Gold");
    }
    public void Break()
    {

    }
    public void UnBreak()
    {

    }
}
