using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerHolder;

    public void ShowPlayer() => playerHolder.SetActive(true);
    public void HidePlayer() => playerHolder.SetActive(false);


    [HideInInspector] public PlayerControls playerControls;
    [HideInInspector] public LevelCreator levelCreator;
    private const int StepSize = 1;

    private MovementAction current;
    private Vector2? stored;

    private float stepTime = 0.15f;
    private float stepTimer = 0;
    private bool moving = false;
    MovementAction newMovement;

    Vector2Int initPosition = new Vector2Int(5,4);
    //Vector2Int position = new Vector2Int(5,4);
    Vector2Int target = new Vector2Int(5,5);

    public Section holdingSection;
    [SerializeField] GameObject redBox;

    public Vector2Int Position { get; private set; }

    private void Awake()
    {
        levelCreator = FindObjectOfType<LevelCreator>();
        InitPosition();
        HidePlayer();
    }

    public void SetInitPosition(Vector2Int pos)
    {
        initPosition = pos;
        InitPosition();
    }
    
    public void InitPosition()
    {
        Position = initPosition;
        target = initPosition + Vector2Int.up;
        transform.localPosition = new Vector3(Position.x,Position.y,0);
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
        if (GameSettings.IsPaused) return;

        //Debug.Log("PICK UP");
        //Place red box where looking to pick up
        //GameObject newRedBox = Instantiate(redBox);
        //newRedBox.transform.position = transform.position + current.movement + Vector3.forward;

        if (levelCreator.heldSection == null)
        {
            // check if there is a pickable in front of player
            levelCreator.PickupSectionAt(Position, target, current.rotationIndex);
        }
        else
        {

            //Debug.Log("Request placing at " + target+ "with rotation "+rotation);
            levelCreator.PlaceHeldSectionAt(target, current.rotationIndex);
            // Placing
            // Check if all boxes fit in game in front of player
            // Place
        }
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        if (GameSettings.IsPaused) return;

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
            if(WalkableTile(Position + Vector2Int.right))
                newMovement = new MovementAction(transform.position,transform.rotation,Vector3.right, Vector3.down,0);
            else return;
        }
        if(direction.y > 0){
            if (WalkableTile(Position + Vector2Int.up))
                newMovement = new MovementAction(transform.position, transform.rotation, Vector3.up, Vector3.right,1);
            else return;
        }
        if(direction.x < 0){
            if (WalkableTile(Position + Vector2Int.left))
                newMovement = new MovementAction(transform.position, transform.rotation, Vector3.left, Vector3.up,2);
            else return;
        }
        if(direction.y < 0){
            if (WalkableTile(Position + Vector2Int.down))
                newMovement = new MovementAction(transform.position, transform.rotation, Vector3.down, Vector3.left,3);
            else return;
        }

        current = newMovement;
        moving = true;
        DoMove(); // Adding this prevents player from picking up piece the same frame moving starts
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
        // PLace actual player at the target position directly so picking up tiles act from this position even during the move animation
        if (stepTimer == 0)
        {
            PlacePlayerAtIndex();
            levelCreator.UpdateHeld(target, current.rotationIndex);
            GameSettings.StepsCounter++;
        }

        stepTimer += Time.deltaTime;
        transform.position += current.movement* Time.deltaTime/stepTime;
        transform.Rotate(current.rotation, 90f * Time.deltaTime / stepTime, Space.World);
        if(stepTimer >= stepTime)
        {
            transform.position = current.TargetPosition;
            transform.rotation = current.TargetRotation;

            

            stepTimer = 0;
            moving = false;
            CheckForStored();
        }
    }

    private void PlacePlayerAtIndex()
    {
        Position = new Vector2Int(Mathf.RoundToInt(current.TargetPosition.x), Mathf.RoundToInt(current.TargetPosition.y));
        //Debug.Log("Player placed at: "+position);
        Vector2Int forwardVector = new Vector2Int(Mathf.RoundToInt(current.movement.x), Mathf.RoundToInt(current.movement.y));
        target = Position + forwardVector;

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
        public int rotationIndex;

        public MovementAction(Vector3 currentPos, Quaternion currentRot, Vector3 move, Vector3 rot, int rotIndex)
        {
            movement = move;
            rotation = rot;

            //Calulate Target
            TargetPosition = currentPos + move;
            TargetRotation = Quaternion.Euler(rot * 90f) * currentRot;
            rotationIndex = rotIndex;
        }
    }

}
