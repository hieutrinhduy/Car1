using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject blackout;

    [SerializeField]
    private GameObject buttonBlackout;

    
    public void Qq()
    {
        blackout.SetActive(true);
        buttonBlackout.SetActive(true);
    }
}
