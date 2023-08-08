using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CreditsController : BasePanel
{
    [SerializeField] GameObject mainSelected;
    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(mainSelected.gameObject);
    }
    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.performed += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.performed -= RequestESC;
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        if (!Enabled()) return;
        Debug.Log("Credits ESC");
        CloseCredits();
    }

    public void CloseCredits()
    {
        TransitionScreen.Instance.StartTransition(GameAction.HideCredits);
    }
}
