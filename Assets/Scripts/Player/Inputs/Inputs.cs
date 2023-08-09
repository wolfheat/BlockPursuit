using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    //Singelton
    public static Inputs Instance { get; private set; }
    public PlayerControls Controls { get; private set; }
    public bool PointerOverUI { get; private set; }

    private void Update()
    {
        PointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    private void Awake()
    {
        //Singelton
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        Controls = new PlayerControls();
        
    }
    private void OnEnable()
    {
        Controls.Enable();
        Controls.Main.LeftClickB.performed += PrintPosition;

    }

    private void PrintPosition(InputAction.CallbackContext obj)
    {
        Debug.Log("Mouse: " + Mouse.current.position.ReadValue());
    }
    private void OnDisable()
    {
        Controls.Disable();
    }
}
