using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FinishStageAnimate : Singleton<FinishStageAnimate>
{
    public RectTransform Board;
    public RectTransform Header;
    public RectTransform RandomReward;
    public RectTransform GoldEarnAmount;
    public RectTransform ClaimMoreBtn;
    public RectTransform MainMenuBtn;
    public RectTransform NextLevelBtn;

    public void StartFinishStagePanel()
    {
        StartCoroutine(StartFinishStagePanelAnimate());
    }
    IEnumerator StartFinishStagePanelAnimate()
    {
        //Set up
        DOTween.Kill(ClaimMoreBtn);
        DOTween.Kill(MainMenuBtn);
        DOTween.Kill(NextLevelBtn);
        DOTween.Kill(Header);
        DOTween.Kill(GoldEarnAmount);
        Board.anchoredPosition = new Vector2(Board.anchoredPosition.x, 1021f);
        Header.anchoredPosition = new Vector2(Board.anchoredPosition.x, 184);
        RandomReward.anchoredPosition = new Vector2(RandomReward.anchoredPosition.x, 645);
        RandomReward.localScale = Vector3.zero;
        ClaimMoreBtn.localScale = Vector3.zero;
        MainMenuBtn.localScale = Vector3.zero;
        NextLevelBtn.localScale = Vector3.zero;
        GoldEarnAmount.localScale = Vector3.zero;

        MainMenuBtn.anchoredPosition = new Vector2(MainMenuBtn.anchoredPosition.x, -228);
        NextLevelBtn.anchoredPosition = new Vector2(NextLevelBtn.anchoredPosition.x, -228);

        //Run
        Board.DOAnchorPosY(0, 0.7f).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(0.2f);

        Header.DOAnchorPosY(-91f, 0.7f).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(0.15f);

        RandomReward.DOAnchorPosY(198f, 0.7f).SetEase(Ease.InOutCubic);
        RandomReward.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);

        GoldEarnAmount.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        GoldEarnAmount.localScale = Vector3.one;
        GoldEarnAmount.DOScale(1.07f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.15f);

        MainMenuBtn.DOAnchorPosY(132.3f, 0.7f).SetEase(Ease.InOutCubic);
        NextLevelBtn.DOAnchorPosY(132.3f, 0.7f).SetEase(Ease.InOutCubic);
        MainMenuBtn.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        NextLevelBtn.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);

        ClaimMoreBtn.localScale = Vector3.one;
        ClaimMoreBtn.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
