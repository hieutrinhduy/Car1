using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShopCarPanelAnimate : Singleton<ShopCarPanelAnimate>
{
    public RectTransform BackBtn;
    public RectTransform Header;
    public RectTransform Board;
    public List<RectTransform> ShopContents;

    public void StartShopCarPanel()
    {
        StartCoroutine(StartShopCarPanelAnimate());
    }
    IEnumerator StartShopCarPanelAnimate()
    {
        //setup
        BackBtn.anchoredPosition = new Vector2(-298f, BackBtn.anchoredPosition.y);
        Header.anchoredPosition = new Vector2(-130.5001f, BackBtn.anchoredPosition.y);
        Board.localScale = Vector2.zero;
        foreach(RectTransform ShopContent in ShopContents)
        {
            DOTween.Kill(ShopContent);
            ShopContent.localScale = Vector2.zero;
        }
        //run
        BackBtn.DOAnchorPosX(131.5f, 0.7f).SetEase(Ease.InOutCubic);
        Header.DOAnchorPosX(299f, 0.7f).SetEase(Ease.InOutCubic);
        Board.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        foreach (RectTransform ShopContent in ShopContents)
        {
            ShopContent.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
