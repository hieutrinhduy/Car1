using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SelectMapAnimate : Singleton<SelectMapAnimate>
{
    public RectTransform BackBtn;
    public RectTransform Header;
    public RectTransform Board;
    public List<RectTransform> BoardContents;
    public RectTransform PlayButton;

    public void StartSelectMapAnimate(){
         StartCoroutine(StartSelectMapAnimateUI());
    }
    IEnumerator StartSelectMapAnimateUI(){
        //SET UP
            DOTween.Kill(PlayButton);
            BackBtn.anchoredPosition = new Vector2(-568.7f, BackBtn.anchoredPosition.y);
            Header.anchoredPosition = new Vector2(-243f, Header.anchoredPosition.y);
            Board.localScale = Vector2.zero;
            PlayButton.localScale = Vector2.one;
            foreach(RectTransform BoardContent in BoardContents)
            {
                DOTween.Kill(BoardContent);
                BoardContent.localScale = Vector2.zero;
            }
        //RUN
            yield return new WaitForSeconds(1.5f);
            BackBtn.DOAnchorPosX(156.3f, 0.7f).SetEase(Ease.InOutCubic);
            Header.DOAnchorPosX(482f, 0.7f).SetEase(Ease.InOutCubic);
            Board.DOScale(Vector3.one* 0.9063308f, 0.4f).SetEase(Ease.InOutQuad);
            PlayButton.DOScale(1.05f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            foreach (RectTransform BoardContent in BoardContents)
            {
                BoardContent.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutQuad);
                yield return new WaitForSeconds(0.1f);
            }
    }
}
