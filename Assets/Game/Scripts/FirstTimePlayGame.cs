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
            foreach (Button lockBTN in ToggleGroup.Ins.LockButtonList)
            {
                string objectKey = lockBTN.name;
                PlayerPrefs.SetInt(objectKey, 1);
            }
            UIManager.Ins.LoadAndSetActiveObjects();
            Daily.Ins.ResetDailyReward();
            hasActivated = true;
            //PlayerPrefs.SetInt("CountToUnlockMap",0);
            for (int i = 0; i < 15; i++)
            {
                string tmp = "CountToLockMap" + i;
                PlayerPrefs.SetInt(tmp,0);
            }
            Destroy(gameObject);
            PlayerPrefs.SetInt("SpecialDrive",0);
            PlayerPrefs.SetInt("Sound",1);
            PlayerPrefs.SetInt("Music",1);
            PlayerPrefs.SetInt("hasActivated", 1);
            PlayerPrefs.SetInt("BoughtOffer", 0);
            PlayerPrefs.Save();
        }

        else
        {
            hasActivated = true;
            Destroy(gameObject);
        }
    }
}
