using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Daily : Singleton<Daily>
{
    
    private int tmp;
    public Button ClaimBTN;
    public Button Claim2xBTN;
    public int CanCollectFirstDay;
    [SerializeField] private List<GameObject> nonActiveList;
    [SerializeField] private List<GameObject> checkMarkList;
    public DateTime FirstClaimDate;
    public DateTime CurrentDate;
    public DateTime LastClaimDate;
    private void Update()
    {
        
    }
    private void Start()
    {
        //Lay Ngay choi hien tai
        CurrentDate = DateTime.Now;
        //Lay Ngay claim dau tien
        if (DateTime.TryParse(PlayerPrefs.GetString("FirstClaimDate"), out DateTime FirstClaimDate))
        {
            Debug.Log($"First claim date is set to: {FirstClaimDate}");
        }
        else
        {
            Debug.LogWarning("Failed to parse the stored date string.");
        }
        //Lay ngay claim cuoi cung
        if (DateTime.TryParse(PlayerPrefs.GetString("LastClaimDate"), out DateTime LastClaimDate))
        {
            Debug.Log($"First claim date is set to: {LastClaimDate}");
        }
        else
        {
            Debug.LogWarning("Failed to parse the stored date string.");
        }
        if (CurrentDate.Day == LastClaimDate.Day)
        {
            ClaimBTN.interactable = false;
            Claim2xBTN.interactable = false;
            tmp = PlayerPrefs.GetInt("SoNgayChoi");
            Debug.Log(tmp);
            for (int i = 0; i < tmp; i++)
            {
                nonActiveList[i].SetActive(false);
                checkMarkList[i].SetActive(true);
            }
        }
        else if(CurrentDate > LastClaimDate)
        {
            tmp = PlayerPrefs.GetInt("SoNgayChoi");
            Debug.Log(tmp);
            nonActiveList[tmp].SetActive(false);
            for (int i = 0; i < tmp; i++)
            {
                nonActiveList[i].SetActive(false);
                checkMarkList[i].SetActive(true);
            }
        }
        else
        {
            ClaimBTN.interactable = false;
            Claim2xBTN.interactable = false;
        }
    }
    
    public void CollectedDailyReward()
    {
        if (tmp == 0)
        {
            checkMarkList[tmp].SetActive(true);
            UIManager.Ins.PurchaseGold1000();
            PlayerPrefs.SetInt("Gold", GameController.Ins.TotalGold);
            tmp++;
            PlayerPrefs.SetInt("SoNgayChoi", tmp);
            PlayerPrefs.SetString("LastClaimDate", DateTime.Now.ToString());
            
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("FirstClaimDate")))
            {
                PlayerPrefs.SetString("FirstClaimDate", DateTime.Now.ToString());
            }
            else
            {
                if (DateTime.TryParse(PlayerPrefs.GetString("FirstClaimDate"), out DateTime FirstClaimDate))
                {
                    Debug.Log($"First claim date is set to: {FirstClaimDate}");
                }
                else
                {
                    Debug.LogWarning("Failed to parse the stored date string.");
                }
            }
            GameController.Ins.Save();
        }
        else if(tmp == 4)
        {
            checkMarkList[tmp].SetActive(true);
            UIManager.Ins.PurchaseGold10000();
            PlayerPrefs.SetInt("Gold", GameController.Ins.TotalGold);
            tmp++;
            PlayerPrefs.SetInt("SoNgayChoi", tmp);
            PlayerPrefs.SetString("LastClaimDate", DateTime.Now.ToString());
            UIManager.Ins.PriceBTNList[9].gameObject.SetActive(false);
            UIManager.Ins.SaveObjectStates();
            GameController.Ins.Save();
        }
        else
        {
            checkMarkList[tmp].SetActive(true);
            UIManager.Ins.PurchaseGold5000();
            PlayerPrefs.SetInt("Gold", GameController.Ins.TotalGold);
            tmp++;
            PlayerPrefs.SetInt("SoNgayChoi", tmp);
            PlayerPrefs.SetString("LastClaimDate", DateTime.Now.ToString());
            GameController.Ins.Save();
        }
    }

    public void CollectedX2DailyReward()
    {
        if (tmp == 0)
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("FirstClaimDate")))
            {
                PlayerPrefs.SetString("FirstClaimDate", DateTime.Now.ToString());
            }
            else
            {
                if (DateTime.TryParse(PlayerPrefs.GetString("FirstClaimDate"), out DateTime FirstClaimDate))
                {
                    Debug.Log($"First claim date is set to: {FirstClaimDate}");
                }
                else
                {
                    Debug.LogWarning("Failed to parse the stored date string.");
                }
            }
            checkMarkList[tmp].SetActive(true);
            UIManager.Ins.PurchaseGold2000();
            PlayerPrefs.SetInt("Gold", GameController.Ins.TotalGold);
            Claim2xBTN.interactable = false;
            ClaimBTN.interactable = false;
            tmp++;
            PlayerPrefs.SetInt("SoNgayChoi", tmp);
            PlayerPrefs.SetString("LastClaimDate", DateTime.Now.ToString());
            GameController.Ins.Save();
        }
        else if (tmp == 4)
        {
            checkMarkList[tmp].SetActive(true);
            UIManager.Ins.PurchaseGold20000();
            PlayerPrefs.SetInt("Gold", GameController.Ins.TotalGold);
            Claim2xBTN.interactable = false;
            ClaimBTN.interactable = false;
            tmp++;
            UIManager.Ins.PriceBTNList[9].gameObject.SetActive(false);
            UIManager.Ins.SaveObjectStates();
            PlayerPrefs.SetInt("SoNgayChoi", tmp);
            PlayerPrefs.SetString("LastClaimDate", DateTime.Now.ToString());
            GameController.Ins.Save();
        }
        else
        {
            checkMarkList[tmp].SetActive(true);
            UIManager.Ins.PurchaseGold10000();
            PlayerPrefs.SetInt("Gold", GameController.Ins.TotalGold);
            Claim2xBTN.interactable = false;
            ClaimBTN.interactable = false;
            tmp++;
            PlayerPrefs.SetInt("SoNgayChoi", tmp);
            PlayerPrefs.SetString("LastClaimDate", DateTime.Now.ToString());
            GameController.Ins.Save();
        }
    }
    public void DisableClaimBtn()
    {
        ClaimBTN.interactable = false;
        Claim2xBTN.interactable = false;
    }
    public void AbleClaimButton()
    {
        ClaimBTN.interactable = true;
        Claim2xBTN.interactable = true;
    }
    public void ResetDailyReward()
    {
        PlayerPrefs.SetInt("SoNgayChoi", 0);
        PlayerPrefs.SetString("FirstClaimDate", "");
        PlayerPrefs.SetString("LastClaimDate", "");
    }
}
