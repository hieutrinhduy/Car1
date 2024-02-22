using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GamePlayPanelAnimate : Singleton<GamePlayPanelAnimate>
{
    public RectTransform ArrowHorizontal;
    public RectTransform SteeringWheel;
    public RectTransform PauseBtn;
    public RectTransform ReplayBtn;
    public RectTransform NitroBtn;
    public RectTransform ArrowVertical;

    public void StartGamePlayPanel()
    {
        StartCoroutine(StartGamePlayPanelAnimate());
    }
    IEnumerator StartGamePlayPanelAnimate()
    {
        yield return new WaitForSeconds(1.5f);
        //set up
        ArrowHorizontal.anchoredPosition = new Vector2(ArrowHorizontal.anchoredPosition.x, -115f);
        SteeringWheel.anchoredPosition = new Vector2(SteeringWheel.anchoredPosition.x, -209f);
        ArrowVertical.anchoredPosition = new Vector2(129, ArrowVertical.anchoredPosition.y);
        NitroBtn.anchoredPosition = new Vector2(108f, NitroBtn.anchoredPosition.y);
        PauseBtn.anchoredPosition = new Vector2(PauseBtn.anchoredPosition.x, 59f);
        ReplayBtn.anchoredPosition = new Vector2(ReplayBtn.anchoredPosition.x, 59f);
        //run
        ArrowHorizontal.DOAnchorPosY(159f, 0.7f).SetEase(Ease.InOutCubic);
        SteeringWheel.DOAnchorPosY(239.9f, 0.7f).SetEase(Ease.InOutCubic);
        ArrowVertical.DOAnchorPosX(-136f, 0.7f).SetEase(Ease.InOutCubic);
        NitroBtn.DOAnchorPosX(-353f, 0.7f).SetEase(Ease.InOutCubic);
        PauseBtn.DOAnchorPosY(-90.19995f, 0.7f).SetEase(Ease.InOutCubic);
        ReplayBtn.DOAnchorPosY(-90.19995f, 0.7f).SetEase(Ease.InOutCubic);
    }
    Tween Nitro;
    public void NitroAnimate()
    {
        Nitro = NitroBtn.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    public void StopNitroAnimate()
    {
        Nitro.Kill();
        NitroBtn.localScale = Vector3.one * 1.3f;
    }
}
