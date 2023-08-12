using UnityEngine;
using UnityEngine.InputSystem;

public class EscapableMenu : MonoBehaviour
{
    [SerializeField] private EscapableBasePanel menu;
    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.ESC.started += RequestESC;
    }

    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.ESC.started -= RequestESC;
    }

    private void RequestESC(InputAction.CallbackContext context)
    {
        menu.RequestESC();
    }

}
