using UnityEngine;


public enum ControlSide{Right,Left,None}

public class TouchPanelController : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] GameObject[] rightControls;
    [SerializeField] GameObject[] leftControls;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        SetControlSide(ControlSide.Left);    
    }

    public void SetControlSide(ControlSide side)
    {
        foreach (var control in rightControls)
        {
            control.SetActive(side==ControlSide.Right?true:false);
        }
        foreach (var control in leftControls)
        {
            control.SetActive(side==ControlSide.Left?true:false);
        }
    }

    public void DirectionTouch(int dir)
    {
        Vector2 dirVector = Vector2.right;
        if (dir == 1)
            dirVector = Vector2.up;
        else if(dir == 2)
            dirVector = Vector2.left;
        else if( dir == 3)
            dirVector = Vector2.down;

        playerController.MoveInputAsVector(dirVector);
    }
    public void ActionTouch()
    {
        playerController.PickUpOrPlace();
    }

}
