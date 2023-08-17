using UnityEngine;
using UnityEngine.UI;

public class BannerNavigationRemover : MonoBehaviour
{
    private void OnEnable()
    {
        AdController.InitializedComplete += RemoveNavigationForBanner;
    }

    private void OnDisable()
    {
        AdController.InitializedComplete -= RemoveNavigationForBanner;
    }

    void RemoveNavigationForBanner()
    {
        Debug.Log("Remove Navigation For Banner");

        //Disable 
        Button button = GameObject.Find("BANNER(Clone)")?.GetComponentInChildren<Button>();        
        if(button != null) button.navigation = new Navigation() { mode = Navigation.Mode.None };
        
    }
}
