using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class BasePanel : MonoBehaviour
{

    [SerializeField] private GameObject panel;
    [SerializeField] protected Button mainSelectedButton;

    private Canvas canvas;
    public void ShowPanel()
    {
        if(canvas == null) canvas = GetComponent<Canvas>();
        panel.SetActive(true);
        canvas.enabled = true;
        SetSelected();
    }
    public virtual void SetSelected()
    {
        if (mainSelectedButton != null)
            EventSystem.current.SetSelectedGameObject(mainSelectedButton.gameObject);
    }

    public void HidePanel()
    {
        if (canvas == null)
        {
            Debug.Log("No available canvas");
            canvas = GetComponent<Canvas>();
            if(canvas == null)
            {
                Debug.Log("Could not get canvas");
                return;
            }
        }
        canvas.enabled = false;
        panel.SetActive(false);
    }

    public bool Enabled() => canvas.enabled;
    public void TogglePanel()
    {
        bool current = canvas.enabled;
        panel.SetActive(!current);
        canvas.enabled = !current;
    }

    private void OnEnable()
    {
        canvas = GetComponent<Canvas>();
    }
}
