using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LosePanelAnimate : Singleton<LosePanelAnimate>
{
    public RectTransform MenuBtn;
    //public RectTransform Header;
    public RectTransform Board;
    public RectTransform ReplayBtn;
    public RectTransform ReplayBtnWhenTryLockedMap;

    public void StartLosePanelAnimate(){
        //set up
        DOTween.Kill(MenuBtn);
        DOTween.Kill(ReplayBtn);
        DOTween.Kill(ReplayBtnWhenTryLockedMap);
        MenuBtn.localScale = Vector2.one;
        //Header.localScale = Vector2.one;
        Board.localScale = Vector2.zero;
        ReplayBtn.localScale = Vector2.one;
        ReplayBtnWhenTryLockedMap.localScale = Vector2.one;
        //run
        MenuBtn.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        Board.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
        //Header.DOScale(1.05f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        ReplayBtn.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        ReplayBtnWhenTryLockedMap.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    public void StopLosePanel()
    {
        DOTween.Kill(MenuBtn);
        DOTween.Kill(ReplayBtn);
        DOTween.Kill(ReplayBtnWhenTryLockedMap);
    }
}
