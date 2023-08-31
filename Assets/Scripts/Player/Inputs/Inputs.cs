using UnityEngine;
using UnityEngine.EventSystems;

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
    }
    private void OnEnable()
    {
        if(Controls == null) Controls = new PlayerControls();
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
