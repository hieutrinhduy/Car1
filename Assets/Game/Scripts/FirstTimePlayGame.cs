using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FirstTimePlayGame : MonoBehaviour
{
    private bool hasActivated;
    public int Gold;
    public int level;
    void Start()
    {
        hasActivated = (PlayerPrefs.GetInt("hasActivated") == 1);
        if (!hasActivated)
        {
            GameController.Ins.TotalGold = Gold;
            GameController.Ins.level = level;
            GameController.Ins.Save();
            PlayerPrefs.SetInt("CurrentCar", 0);
            foreach (Button priceBTN in UIManager.Ins.PriceBTNList)
            {
                string objectKey = priceBTN.name;
                PlayerPrefs.SetInt(objectKey, 1);
            }
            UIManager.Ins.LoadAndSetActiveObjects();
            Daily.Ins.ResetDailyReward();
            hasActivated = true;
            Destroy(gameObject);
            PlayerPrefs.SetInt("hasActivated", 1);
            PlayerPrefs.Save();
        }
        else
        {
            hasActivated = true;
            Destroy(gameObject);
        }
    }
}
