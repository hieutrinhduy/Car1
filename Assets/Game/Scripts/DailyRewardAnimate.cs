using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DailyRewardAnimate : Singleton<DailyRewardAnimate>
{
    public RectTransform BackBtn;
    public RectTransform Header;
    public RectTransform Board;
    public List<RectTransform> ShopContents;
    public RectTransform Claim;
    public RectTransform Claimx2;
    public RectTransform DailyRewardDay4;
    public void StartDailyReward()
    {
        StartCoroutine(StartDailyRewardAnimate());
    }
    IEnumerator StartDailyRewardAnimate()
    {
        //setup
        float x = DailyRewardDay4.localPosition.x;
        BackBtn.anchoredPosition = new Vector2(-489f, BackBtn.anchoredPosition.y);
        Header.anchoredPosition = new Vector2(-226, BackBtn.anchoredPosition.y);
        Claim.localScale = Vector2.zero;
        Claimx2.localScale = Vector2.zero;
        Board.localScale = Vector2.zero;
        foreach (RectTransform ShopContent in ShopContents)
        {
            DOTween.Kill(ShopContent);
            ShopContent.localScale = Vector2.zero;
        }
        //run
        BackBtn.DOAnchorPosX(112f, 0.7f).SetEase(Ease.InOutCubic);
        Header.DOAnchorPosX(375f, 0.7f).SetEase(Ease.InOutCubic);
        Board.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        foreach (RectTransform ShopContent in ShopContents)
        {
            ShopContent.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.15f);
        }
        Claim.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        Claimx2.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);

        foreach (RectTransform ShopContent in ShopContents)
        {
            ShopContent.DOScale(Vector3.one*1.25f, 0.3f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.2f);
            ShopContent.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad);
        }
    }
}
