using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerControls playerControls;
    private const int StepSize = 1;

    private MovementAction current;
    private Vector2? stored;

    private Vector3 rotation = Vector3.up;
    private Vector3 movement = Vector3.up;
    private float stepTime = 0.15f;
    private float stepTimer = 0;
    private bool moving = false;
    MovementAction newMovement;

    Vector2Int position = Vector2Int.one;

    private Section holdingSection;

    [SerializeField] GameObject redBox;

    private void Awake()
    {
        //playerControls = new PlayerControls();
        InitPosition();    
    }

    private void InitPosition()
    {
        transform.localPosition = new Vector3(position.x,position.y,-2);
    }

    private void OnEnable()
    {
        // Subscribe to input
        Inputs.Instance.Controls.Main.Move.performed += MoveInput;
        Inputs.Instance.Controls.Main.Interact.performed += PickUpOrPlace;
    }
    
    private void OnDisable()
    {
        Inputs.Instance.Controls.Main.Move.performed -= MoveInput;
        Inputs.Instance.Controls.Main.Interact.performed -= PickUpOrPlace;
    }

    private void PickUpOrPlace(InputAction.CallbackContext context)
    {
        Debug.Log("PICK UP");
        //Place red box where looking to pick up
        GameObject newRedBox = Instantiate(redBox);
        newRedBox.transform.position = transform.position + current.movement + Vector3.forward;

        // check if there is a pickable in front of player
        // Check if player is not on that pickable
        // Pick Up

        // Placing
        // Check if all boxes fit in game in front of player
        // Place

    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        MoveInputAsVector(direction);
    }
    private void MoveInputAsVector(Vector2 direction)
    {
        if (moving)
        {
            stored = direction;
            return;
        }


        if (direction.x > 0)
        {
            if(WalkableTile(position + Vector2Int.right))
            {
                newMovement = new MovementAction(transform.position,transform.rotation,Vector3.right, Vector3.down);
                Debug.Log("Right");
            }
            else return;
        }
        if(direction.x < 0){
            if (WalkableTile(position + Vector2Int.left))
            {
                newMovement = new MovementAction(transform.position, transform.rotation, Vector3.left, Vector3.up);
                Debug.Log("Left");
            }
            else return;
        }
        if(direction.y > 0){
            if (WalkableTile(position + Vector2Int.up))
            {
                newMovement = new MovementAction(transform.position, transform.rotation, Vector3.up, Vector3.right);
                Debug.Log("Up");
            }
            else return;
        }
        if(direction.y < 0){
            if (WalkableTile(position + Vector2Int.down))
            {
                newMovement = new MovementAction(transform.position, transform.rotation, Vector3.down, Vector3.left);
                Debug.Log("Down");
            }
            else return;
        }

        current = newMovement;
        moving = true;
        
    }

    private bool WalkableTile(Vector2Int pos)
    {
        return LevelCreator.IsWalkable(pos);
    }

    private void Update()
    {
        if (moving) DoMove();
    }

    private void DoMove()
    {
        stepTimer += Time.deltaTime;
        transform.position += current.movement* Time.deltaTime/stepTime;
        transform.Rotate(current.rotation, 90f * Time.deltaTime / stepTime, Space.World);
        if(stepTimer >= stepTime)
        {
            transform.position = current.TargetPosition;
            transform.rotation = current.TargetRotation;

            stepTimer = 0;
            moving = false;
            PlacePlayerAtIndex();
            CheckForStored(); 
        }
    }

    private void PlacePlayerAtIndex()
    {
        position = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.y));
        //Debug.Log("Player placed at: "+position);
    }

    private void CheckForStored()
    {
        if(stored != null)
        {
            //Debug.Log("Loading Stored movement: "+stored);
            MoveInputAsVector((Vector2)stored);
            stored = null;
        }
        else
        {
            //Debug.Log("Movement Held: "+ Inputs.Instance.Controls.Main.Move.ReadValue<Vector2>());
            if (Inputs.Instance.Controls.Main.Move.IsPressed()) MoveInputAsVector(Inputs.Instance.Controls.Main.Move.ReadValue<Vector2>()); 
            else moving = false;
        }
    }

    public class MovementAction
    {
        public Vector3 movement;
        public Vector3 rotation;
        public Vector3 TargetPosition;
        public Quaternion TargetRotation;

        public MovementAction(Vector3 currentPos, Quaternion currentRot, Vector3 move, Vector3 rot)
        {
            movement = move;
            rotation = rot;

            //Calulate Target
            TargetPosition = currentPos + move;
            TargetRotation = Quaternion.Euler(rot*90f)* currentRot;
        }
    }

}
