using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class RandomReward : Singleton<RandomReward>
{
    public RectTransform arrow;
    public float time = 1;
    float randomTime;
    public int multiple = 1;
    private float initialXPosition = -280f;
    public Text goldEarned;
    Tween arrowTween;
    public Button ClaimX2BTN;
    private int GoldAmount;
    void Start()
    {
        
    }
    public void SetGoldAmount()
    {
        GoldAmount = UnityEngine.Random.Range(1000, 3000);
        GoldAmount -= GoldAmount % 500;
        Debug.Log(GoldAmount);
        UIManager.Ins.PurchaseGoldWithAmount(GoldAmount);
    }
    public void Randomnize()
    {
        randomTime = Random.Range(2.5f, 4f);
        arrowTween = arrow.DOAnchorPosX(280f, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        StopTween();
        SetArrowXPosition(initialXPosition);
    }
    public void SetArrowXPosition(float xPosition)
    {
        Vector2 newPosition = arrow.anchoredPosition;
        newPosition.x = xPosition;
        arrow.anchoredPosition = newPosition;
        goldEarned.text = "You got " + GoldAmount + "$";
    }

    public void StopTween()
    {
        StartCoroutine(DelayStop());
    }

    IEnumerator DelayStop()
    {
        yield return new WaitForSeconds(randomTime);
        if (arrowTween != null && arrowTween.IsActive())
        {
            arrowTween.Kill();
        }
        if ((-280 <= arrow.anchoredPosition.x && arrow.anchoredPosition.x <= -235) || (230 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= 280f))
        {
            multiple = 2;
            //UIManager.Ins.PurchaseGold2000();
            UIManager.Ins.PurchaseGoldWithAmount(GoldAmount * 2);
            goldEarned.text = "You got " + GoldAmount * 2 + "$";
            GameController.Ins.Save();
        }
        else if ((-235 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= -140f) || (140 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= 230f))
        {
            multiple = 3;
            //UIManager.Ins.PurchaseGold3000();
            UIManager.Ins.PurchaseGoldWithAmount(GoldAmount * 3);
            goldEarned.text = "You got " + GoldAmount * 3 + "$";
            GameController.Ins.Save();
        }
        else if ((-140 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= -46) || (46 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= 140))
        {
            multiple = 4;
            //UIManager.Ins.PurchaseGold4000();
            UIManager.Ins.PurchaseGoldWithAmount(GoldAmount * 4);
            goldEarned.text = "You got " + GoldAmount * 4 + "$";
            GameController.Ins.Save();
        }
        else if (-46 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= 46)
        {
            multiple = 5;
            //UIManager.Ins.PurchaseGold5000();
            UIManager.Ins.PurchaseGoldWithAmount(GoldAmount * 5);
            goldEarned.text = "You got " + GoldAmount * 5 + "$";
            GameController.Ins.Save();
        }
    }
    public void ActiveClaimBTN()
    {
        ClaimX2BTN.interactable = true;
    }
    float tmp;
    public void ClaimX2()
    {
        //reward
        UnityEvent e = new UnityEvent();
        e.AddListener(() =>
        {
            UIManager.Ins.InactiveMoreGoldBtn();
            Randomnize();
            ClaimX2BTN.interactable = false;
            StartCoroutine(ClaimX2AndGoldAnimated());
            UIManager.Ins.ActiveMoreGoldBtn(tmp);
            GameController.Ins.Save();
        });
        SkygoBridge.instance.ShowRewarded(e, null);
        //logevent
        SkygoBridge.instance.LogEvent("claim_more_gold_after_finish");
    }
    IEnumerator ClaimX2AndGoldAnimated(){
        tmp = randomTime + 0.3f;
        yield return new WaitForSeconds(tmp);
        RewardManager.Ins.RewardPileOfCoin(null,6);
    }
}
