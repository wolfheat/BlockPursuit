using UnityEngine;


public enum ControlSide{Right,Left,None}

public class TouchPanelController : MonoBehaviour
{
    PlayerController playerController;

    private int activeControls = 0;

    // NEW MAIN INPUT METHOD
    [SerializeField] GameObject swipe;

    [SerializeField] GameObject middle;
    [SerializeField] GameObject right;
    [SerializeField] GameObject left;
    [SerializeField] GameObject none;
    private GameObject[] controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = new GameObject[] { swipe,middle, right,left,none};

        playerController = FindObjectOfType<PlayerController>();

    }

    public void ChangeTouch()
    {
        controls[activeControls].SetActive(false);
        activeControls = (activeControls+1)%controls.Length;
        controls[activeControls].SetActive(true);
    }

    public void DirectionTouch(int dir)
    {
        Debug.Log("TOUCH: "+dir);
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
