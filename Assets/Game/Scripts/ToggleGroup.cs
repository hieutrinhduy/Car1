using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class ToggleGroup : Singleton<ToggleGroup>
{
    [SerializeField] private List<Image> BorderList;
    [SerializeField] public List<Button> LockButtonList;
    [SerializeField] private GameObject WatchVideoToUnLockMapPanel;
    private int countToUnlockMap = 0;
    [SerializeField] private TextMeshProUGUI CountToUnlockMapTXT;
    [SerializeField] private Button PlayBtn;
    [SerializeField] private Button BuyBtn;
    [SerializeField] private Button TryBtn;

    [SerializeField] private List<Sprite> ImageList;
    [SerializeField] private Image Image_Map_In_WatchToUnlockMapPanel;
    private void Start()
    {
        countToUnlockMap = 0;
        int n = GameController.Ins.level;
        BorderList[n].gameObject.SetActive(true);
        for (int i = 0; i < BorderList.Count; i++)
        {
            if (i != n)
            {
                BorderList[i].gameObject.SetActive(false);
            }
        }
        CountToUnlockMapTXT.SetText(countToUnlockMap+"/3");
        LoadAndSetActiveObjects();
    }
    private void Update()
    {
        CountToUnlockMapTXT.SetText(countToUnlockMap + "/3");
    }
    public int GetActiveToggle()
    {
        Toggle[] toggles = this.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                return i;
            }
        }
        return 0;
    }
    public void SetBorder()
    {
        int n = GameController.Ins.level;
        BorderList[n].gameObject.SetActive(true);
        for (int i = 0; i < BorderList.Count; i++)
        {
            if (i != n)
            {
                BorderList[i].gameObject.SetActive(false);
            }
        }
    }
    int LockButtonIndex;
    public void ActiveWatchVideoToUnLockMapPanel()
    {
        //LockButtonIndex = LockButtonList.IndexOf(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        //Debug.Log(LockButtonIndex+7);
        //Image_Map_In_WatchToUnlockMapPanel.sprite = ImageList[LockButtonIndex+7];
        //WatchVideoToUnLockMapPanel.SetActive(true);
    }
    public void DeActiveWatchVideoToUnLockMapPanel()
    {
        WatchVideoToUnLockMapPanel.SetActive(false);
    }
    public void WatchedAVideo()
    {
        
        if (countToUnlockMap <= 2)
        {
            countToUnlockMap += 1;
            if(countToUnlockMap == 3)
            {
                DeActiveWatchVideoToUnLockMapPanel();
                LockButtonList[LockButtonIndex].gameObject.SetActive(false);
                SaveObjectStates();
                countToUnlockMap = 0;
            }
        }
    }

    void SaveObjectStates()
    {
        foreach (Button btn in LockButtonList)
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
        foreach (Button btn in LockButtonList)
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
    public void TurnOnPlayBtn()
    {
        PlayBtn.gameObject.SetActive(true);
        TryBtn.gameObject.SetActive(false);
        BuyBtn.gameObject.SetActive(false);
    }
    public void TurnOnBuyBtn()
    {
        PlayBtn.gameObject.SetActive(false);
        TryBtn.gameObject.SetActive(true);
        BuyBtn.gameObject.SetActive(true);
    }
    public void ResetLockMap()
    {
        foreach (Button btn in LockButtonList)
        {
            string objectKey = btn.name;
            PlayerPrefs.SetInt(objectKey, 1);
        }
    }
}
