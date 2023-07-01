using TMPro;
using UnityEngine;


enum TierName{Bronze,Silver,Gold}

public class TierButton : MonoBehaviour
{
    [SerializeField] TierName tier;

    public void RequestStartTier()
    {
        Debug.Log("Request start level " + tier);
    }

}
