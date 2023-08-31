using UnityEngine;

public class InfoButton : MonoBehaviour
{
    [SerializeField] InfoData infoData;
    [SerializeField] BasePanel sourcePanel;
    private InfoPopup infoPopup;

    private void Start()
    {
        infoPopup = FindObjectOfType<InfoPopup>();
    }

    public void ShowInfo()
    {
        if(infoData == null)
        {
            infoPopup.ShowInfo(new InfoData() {informationText = " NO INFO SET" }, sourcePanel);
            return;
        }
        infoPopup.ShowInfo(infoData, sourcePanel);
    }

}
