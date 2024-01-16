using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardedUpgradeAdsButton : MonoBehaviour
{
    [SerializeField]
    private float checkDelay = .5f;

    [SerializeField]
    private UpgradeButton button;


    private bool _isAvailable;


    private void Start()
    {
        _isAvailable = AdsManager.Instance.IsRewardVideoAvailable;
    }

    private void OnEnable()
    {
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
            if (button.CanUpgradeByVideo)
            {
                _isAvailable = AdsManager.Instance.IsRewardVideoAvailable;

                if (_isAvailable && button.IsLocked)
                    button.Unlock();

                else if (!_isAvailable && !button.IsLocked)
                    button.Lock();
            }

            yield return new WaitForSeconds(checkDelay);
        }
    }
}