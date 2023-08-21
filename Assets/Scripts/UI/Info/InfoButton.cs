using UnityEngine;

public class InfoButton : MonoBehaviour
{
    [SerializeField] InfoData infoData;
    private InfoPopup infoPopup;

    private void Start()
    {
        infoPopup = FindObjectOfType<InfoPopup>();
    }

    public void ShowInfo()
    {
        if(infoData == null)
        {
            infoPopup.ShowInfo(new InfoData() {informationText = " NO INFO SET" });
            return;
        }
        infoPopup.ShowInfo(infoData);
    }

}
