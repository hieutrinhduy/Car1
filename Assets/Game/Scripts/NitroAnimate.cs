using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NitroAnimate : Singleton<NitroAnimate>
{
    public RectTransform NitroBtn;

    public void StartNitroAnimate()
    {
        NitroBtn.localScale = Vector3.one * 1.3f;
        NitroBtn.DOScale(1.8f, 0.4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
