using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UnlockMapAnimate : Singleton<UnlockMapAnimate>
{
    public RectTransform Board;
    public RectTransform Header;
    public RectTransform GoldBtn;
    public RectTransform WatchAdBtn;
    public RectTransform LevelImage;

    public void StartUnlockMapAnimatePanel()
    {
        StartCoroutine(StartUnlockMapAnimate());
    }
    IEnumerator StartUnlockMapAnimate()
    {
        //set up
        Board.anchoredPosition = new Vector3 (0.0f, 943f, 0.0f);
        Header.localScale = Vector3.one;
        LevelImage.localScale = Vector3.zero;
        GoldBtn.localScale = Vector3.one;
        WatchAdBtn.localScale = Vector3.one;
        //run
        Board.DOAnchorPosY(-6.200012f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        Header.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        LevelImage.DOScale(Vector2.one, 0.3f);
        yield return new WaitForSeconds(0.3f);
        GoldBtn.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        WatchAdBtn.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
