using UnityEngine;
using UnityEngine.InputSystem;

public class TabToggleGameObject : MonoBehaviour
{
    [SerializeField] GameObject go;

    private void OnEnable()
    {
        Inputs.Instance.Controls.Main.TAB.performed += Toggle;
    }
    
    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.TAB.performed += Toggle;
    }

    // Update is called once per frame
    private void Toggle(InputAction.CallbackContext context)
    {
        go.SetActive(!go.activeSelf);
    }
}
