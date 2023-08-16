using UnityEngine;

public class LoadingGame : MonoBehaviour
{   
    private void OnEnable()
    {
        SavingUtility.LoadingComplete += SetActiveControlFromSave;

    }
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= SetActiveControlFromSave;
    }

    private void SetActiveControlFromSave()
    {
        gameObject.SetActive(false);
    }

}
