using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardedAdsButton : MonoBehaviour
{
    [SerializeField]
    private float checkDelay = .5f;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Animator animator;

    private bool _isAvailable;
    private readonly int pulseHash = Animator.StringToHash("Pulse");


    private void Reset()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _isAvailable = AdsManager.Instance.IsRewardVideoAvailable;
    }

    private void OnEnable()
    {
        animator.Play(pulseHash, 0, 0);
        animator.speed = 0;
            
        StartCoroutine(UpdateButtonAvailability());
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private IEnumerator UpdateButtonAvailability()
    {
        while (true)
        {
            button.interactable = AdsManager.Instance.IsRewardVideoAvailable;

            if (_isAvailable != AdsManager.Instance.IsRewardVideoAvailable)
            {
                _isAvailable = AdsManager.Instance.IsRewardVideoAvailable;
                if (_isAvailable)
                {
                    animator.speed = 1;
                }
                else
                {
                    animator.Play(pulseHash, 0, 0);
                    animator.speed = 0;
                }
            }


            yield return new WaitForSeconds(checkDelay);
        }
    }
}