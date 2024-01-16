using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    private bool canUpgradeByVideo;

    public bool CanUpgradeByVideo
    {
        get => canUpgradeByVideo;
        set
        {
            canUpgradeByVideo = value;
            UpdateContainer();
        }
    }


    [SerializeField]
    private GameObject lockedBackground;

    [SerializeField]
    private GameObject unlockedBackground;

    [Space]
    [SerializeField]
    private GameObject adsContainer;

    [SerializeField]
    private GameObject priceContainer;

    [Space]
    [SerializeField]
    private Button button;

    [Space]
    [SerializeField]
    private UnityEvent click;

    public bool IsLocked { get; private set; }


    public void Unlock()
    {
        unlockedBackground.SetActive(true);
        lockedBackground.SetActive(false);

        button.interactable = true;
        IsLocked = false;
    }

    public void Lock()
    {
        unlockedBackground.SetActive(false);
        lockedBackground.SetActive(true);

        button.interactable = false;
        IsLocked = true;
    }

    public void Click()
    {
        click?.Invoke();
    }

    public void UpdateContainer()
    {
        adsContainer.SetActive(canUpgradeByVideo);
        priceContainer.SetActive(!canUpgradeByVideo);
    }
}