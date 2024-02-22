using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OnlineGiftPanelAnimate : Singleton<OnlineGiftPanelAnimate>
{
    public RectTransform Claim;
    public RectTransform Claimx2;
    public RectTransform Header;
    public RectTransform MainBoard;
    public RectTransform Tag;
    public RectTransform BackBtn;
    public void StartOnlineGift()
    {
        DOTween.Kill(Header);
        DOTween.Kill(Claim);
        DOTween.Kill(Claimx2);
        //Set up
        Claim.localScale = Vector2.one;
        Claimx2.localScale = Vector2.one;
        Header.localScale = Vector2.one;
        MainBoard.localScale = Vector3.zero;
        Tag.localScale = Vector3.zero;
        BackBtn.anchoredPosition = new Vector2(-75, BackBtn.anchoredPosition.y);
        //Run
        Claim.DOScale(1.02f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        Claimx2.DOScale(1.02f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        Header.DOScale(1.09f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        MainBoard.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        Tag.DOScale(Vector3.one, 0.7f).SetEase(Ease.InOutQuad);
        BackBtn.DOAnchorPosX(136.9f, 0.7f).SetEase(Ease.InOutCubic);
    }
}
