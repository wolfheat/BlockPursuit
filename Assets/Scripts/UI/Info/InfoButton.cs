using UnityEngine;

public class InfoButton : MonoBehaviour
{
    [SerializeField] InfoData infoData;
    [SerializeField] GameObject button;
    private InfoPopup infoPopup;

    private void Start()
    {
        infoPopup = FindObjectOfType<InfoPopup>();
    }

    public void ShowInfo()
    {
        if(infoData == null)
        {
            infoPopup.ShowInfo(new InfoData() {informationText = " NO INFO SET" },gameObject);
            return;
        }
        infoPopup.ShowInfo(infoData, gameObject);
    }

}
