using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ToggleGroup : Singleton<ToggleGroup>
{
    [SerializeField] private List<Image> BorderList;
    [SerializeField] public List<Button> LockButtonList;
    [SerializeField] public List<TextMeshProUGUI> NameLists;
    [SerializeField] private GameObject WatchVideoToUnLockMapPanel;
    [SerializeField] private GameObject UnlockMapPanel;
    private int countToUnlockMap = 0;
    [SerializeField] private TextMeshProUGUI CountToUnlockMapTXT;
    [SerializeField] private Button PlayBtn;
    [SerializeField] private Button BuyBtn;
    [SerializeField] private Button TryBtn;

    [SerializeField] private List<Sprite> ImageList;
    [SerializeField] private Image Image_Map_In_WatchToUnlockMapPanel;
    [SerializeField] private Image Image_Map_In_UnlockMapPanel;
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
        CountToUnlockMapTXT.SetText(countToUnlockMap+"/7");
        LoadAndSetActiveObjects();
    }
    private void Update()
    {
        CountToUnlockMapTXT.SetText(countToUnlockMap + "/7");
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
        LockButtonIndex = LockButtonList.IndexOf(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        //Debug.Log(LockButtonIndex + 5);
        //Image_Map_In_WatchToUnlockMapPanel.sprite = ImageList[LockButtonIndex+7];
        //WatchVideoToUnLockMapPanel.SetActive(true);
        BorderList[LockButtonIndex+5].gameObject.SetActive(true);
        for (int i = 0; i < BorderList.Count; i++)
        {
            if (i != LockButtonIndex+5)
            {
                BorderList[i].gameObject.SetActive(false);
            }
        }
    }
    public void DeActiveWatchVideoToUnLockMapPanel()
    {
        UnlockMapAnimate.Ins.StopUnlockMapAnimate();
        WatchVideoToUnLockMapPanel.SetActive(false);
    }
    public void WatchedAVideo()
    {
        UnityEvent e = new UnityEvent();
        e.AddListener(() =>
        {
            if (countToUnlockMap <= 6)
            {
                countToUnlockMap += 1;
                //Debug.Log(countToUnlockMap);
                CountToUnlockMapTXT.SetText(countToUnlockMap + "/7");
                if (countToUnlockMap == 7)
                {
                    //DeActiveWatchVideoToUnLockMapPanel();
                    LockButtonList[LockButtonIndex].gameObject.SetActive(false);
                    SaveObjectStates();
                    GameController.Ins.level = LockButtonIndex + 5;
                    GameController.Ins.Save();
                    UIManager.Ins.ChangeLevelNotice();
                    TurnOnPlayBtn();
                    countToUnlockMap = 0;
                    CloseUnlockMapPanel();
                }
            }
        });
        SkygoBridge.instance.ShowRewarded(e, null);
        //logevent
        SkygoBridge.instance.LogEvent("watch_a_video_to_unlock_map");
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
    public void OpenUnlockMapPanel()
    {
        Image_Map_In_UnlockMapPanel.sprite = ImageList[LockButtonIndex + 5];
        //WatchVideoToUnLockMapPanel.SetActive(true);
        UnlockMapPanel.SetActive(true);
        UnlockMapAnimate.Ins.StartUnlockMapAnimatePanel();
        UIManager.Ins.CloseSelectMapPanel();
    }
    public void CloseUnlockMapPanel()
    {
        UnlockMapAnimate.Ins.StopUnlockMapAnimate();
        UnlockMapPanel.SetActive(false);
        UIManager.Ins.OpenSelectMapPanelWhileExitUnlockMapPanel();
    }
    public void BuyMap()
    {
        if (GameController.Ins.TotalGold >= 50000)
        {
            GameController.Ins.TotalGold -= 50000;
            GameController.Ins.level = LockButtonIndex + 5;
            GameController.Ins.Save();
            UIManager.Ins.ChangeLevelNotice();
            TurnOnPlayBtn();
            LockButtonList[LockButtonIndex].gameObject.SetActive(false);
            SaveObjectStates();
            CloseUnlockMapPanel();
        }
    }
    public void TryLockedLevel()
    {
        //reward
        UnityEvent e = new UnityEvent();
        e.AddListener(() =>
        {
            SpawnLevel.Ins.SpawnLevelMapWithIndex(LockButtonIndex + 5);
            SpawnLevel.Ins.SpawnPlayer();
            GameController.Ins.IsTryingMap = true;
            UIManager.Ins.TryingLockedMap();
            string tmp = NameLists[LockButtonIndex + 5].text;
            SkygoBridge.instance.LogEvent("try_level_name_"+tmp);
        });
        SkygoBridge.instance.ShowRewarded(e, null);
        //logevent
        //SkygoBridge.instance.LogEvent("try_locked_level");
        //GameController.Ins.level = LockButtonIndex + 7;
    }
}
