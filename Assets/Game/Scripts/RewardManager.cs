using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardManager : Singleton<RewardManager>
{
    [SerializeField] private RectTransform PileOfCpoinParent;
    [SerializeField] Text coinUIText;
    public Vector3[] InitialPos;
    public Quaternion[] InitialRotation;
    [SerializeField] private RectTransform target;
    private Vector2 imagePosition;
    public int CoinNo;

    private void Start()
    {
        InitialPos = new Vector3[CoinNo];
        InitialRotation = new Quaternion[CoinNo];
        for (int i = 0; i < PileOfCpoinParent.childCount; i++)
        {
            InitialPos[i] = PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().position;
            InitialRotation[i] = PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().rotation;
        }
        Debug.Log(imagePosition);
    }
    private void ResetPos()
    {
        for (int i = 0; i < PileOfCpoinParent.childCount; i++)
        {
            PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().rotation = InitialRotation[i];
            Debug.Log("Reset vi tri");
        }
    }
    public void RewardPileOfCoinWhenBuy(RectTransform SpawnPoint, int No_coin)
    {
        Vector3 position = SpawnPoint.position;
        position.y +=300; 
        PileOfCpoinParent.position = position;

        var delay = 0f;
        PileOfCpoinParent.gameObject.SetActive(true);
        for (int i = 0; i < PileOfCpoinParent.childCount; i++)
        {
            PileOfCpoinParent.GetChild(i).DOScale(1.5f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().DOMove(target.position, 1f).SetDelay(delay+0.2f).SetEase(Ease.OutBack);
            PileOfCpoinParent.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay+1f).SetEase(Ease.OutBack);
            PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(Vector3.zero, 0f).SetDelay(0f).SetEase(Ease.OutBack);
            delay += 0.2f;
        }
    }
    public void RewardPileOfCoin(RectTransform SpawnPoint, int No_coin)
    {
        Vector3 position = Vector3.zero;
        position.y +=70;
        PileOfCpoinParent.anchoredPosition = position;

        var delay = 0f;
        PileOfCpoinParent.gameObject.SetActive(true);
        for (int i = 0; i < PileOfCpoinParent.childCount; i++)
        {
            PileOfCpoinParent.GetChild(i).DOScale(2f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().DOMove(target.position, 1f).SetDelay(delay+0.2f).SetEase(Ease.OutBack);
            PileOfCpoinParent.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay+1f).SetEase(Ease.OutBack);
            PileOfCpoinParent.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(Vector3.zero, 0f).SetDelay(0f).SetEase(Ease.OutBack);
            delay += 0.2f;
        }
    }
}
