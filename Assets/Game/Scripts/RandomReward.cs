using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

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
    void Start()
    {
        
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
        goldEarned.text = "You got " + 1000 + "$";
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
            CarController.Ins.GoldInGame = 1000*2;
            GameController.Ins.TotalGold += CarController.Ins.GoldInGame;
            Debug.Log(GameController.Ins.TotalGold);
            goldEarned.text = "You got " + 1000*2 + "$";
            GameController.Ins.Save();
        }
        else if ((-235 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= -140f) || (140 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= 230f))
        {
            multiple = 3;
            CarController.Ins.GoldInGame = 1000 * 3;
            GameController.Ins.TotalGold += CarController.Ins.GoldInGame;
            Debug.Log(GameController.Ins.TotalGold);
            goldEarned.text = "You got " + 1000 * 3 + "$";
            GameController.Ins.Save();
        }
        else if ((-140 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= -46) || (46 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= 140))
        {
            multiple = 4;
            CarController.Ins.GoldInGame = 1000 * 4;
            GameController.Ins.TotalGold += CarController.Ins.GoldInGame;
            Debug.Log(GameController.Ins.TotalGold);
            goldEarned.text = "You got " + 1000 * 4 + "$";
            GameController.Ins.Save();
        }
        else if (-46 < arrow.anchoredPosition.x && arrow.anchoredPosition.x <= 46)
        {
            multiple = 5;
            CarController.Ins.GoldInGame = 1000 * 5;
            GameController.Ins.TotalGold += CarController.Ins.GoldInGame;
            Debug.Log(GameController.Ins.TotalGold);
            goldEarned.text = "You got " + 1000 * 5 + "$";
            GameController.Ins.Save();
        }
    }
    public void ActiveClaimBTN()
    {
        ClaimX2BTN.interactable = true;
    }
    public void ClaimX2()
    {
        Randomnize();
        GameController.Ins.TotalGold += CarController.Ins.GoldInGame -1000;
        ClaimX2BTN.interactable = false;
        StartCoroutine(ClaimX2AndGoldAnimated());
        GameController.Ins.Save();
    }
    IEnumerator ClaimX2AndGoldAnimated(){
        yield return new WaitForSeconds(randomTime+0.3f);
        RewardManager.Ins.RewardPileOfCoin(null,6);
    }
}
