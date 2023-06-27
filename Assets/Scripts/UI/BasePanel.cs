using UnityEngine;

public class BasePanel : MonoBehaviour
{

    [SerializeField] private GameObject panel;
    public void ShowPanel() => panel.SetActive(true);
    public void HidePanel() => panel.SetActive(false);
    public void TogglePanel() => panel.gameObject.SetActive(!panel.gameObject.activeSelf);
}
