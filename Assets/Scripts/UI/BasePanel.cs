using UnityEngine;

public class BasePanel : MonoBehaviour
{

    [SerializeField] private GameObject panel;
    public void ShowPanel() => panel.SetActive(true);
    public void HidePanel() => panel.SetActive(false);
    public bool Enabled() => panel.activeSelf;
    public void TogglePanel() => panel.gameObject.SetActive(!panel.gameObject.activeSelf);
}
