using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerHolder;
    [SerializeField] GameObject body;

    [SerializeField] GameObject character;
    [SerializeField] Animator animator;

    [SerializeField] GameObject hands;
    [SerializeField] PlayerLevelDataList playerLevelsDefinition;

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
    Vector2Int target = new Vector2Int(5,5);

    public Section holdingSection;


    public Vector2Int Position { get; private set; }

    private void Awake()
    {
        levelCreator = FindObjectOfType<LevelCreator>();
        
        HidePlayer();
    }

    public void SetInitPosition(Vector2Int pos)
    {
        Position = pos;
        target = pos + Vector2Int.up;
        transform.localPosition = new Vector3(Position.x,Position.y,0);
        //current = new MovementAction(body.transform.localPosition, Quaternion.LookRotation(Vector3.back, Vector3.up), Vector3.up, Vector3.back,1);
        current = new MovementAction(transform.position, character.transform.forward, Vector3.up, 1);
    }
    
    public void InitPosition()
    {
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
        PickUpOrPlace();
    }
    public void PickUpOrPlace()
    {
        if (GameSettings.IsPaused) return;

        if (levelCreator.heldSection == null)
            levelCreator.PickupSectionAt(Position, target, current.rotationIndex);
        else
            levelCreator.PlaceHeldSectionAt(target, current.rotationIndex);
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        MoveInputAsVector(direction);
    }

    public void MoveInputAsVector(Vector2 direction)
    {
        if (GameSettings.IsPaused) return;

        if (moving)
        {
            stored = direction;
            return;
        }

        bool canMove = true;

        if (direction.x > 0)
        {
            if (WalkableTile(Position + Vector2Int.right))
                newMovement = new MovementAction(transform.position, character.transform.forward, Vector3.right, 0);
            else canMove = false;
        }
        if(direction.y > 0){
            if (WalkableTile(Position + Vector2Int.up))
                newMovement = new MovementAction(transform.position, character.transform.forward, Vector3.up , 1);
            else canMove = false;
        }
        if(direction.x < 0){
            if (WalkableTile(Position + Vector2Int.left))
                newMovement = new MovementAction(transform.position, character.transform.forward, Vector3.left, 2);
            else canMove = false;
        }
        if(direction.y < 0){
            if (WalkableTile(Position + Vector2Int.down))
                newMovement = new MovementAction(transform.position, character.transform.forward, Vector3.down, 3);
            else canMove = false;
        }

        if (!canMove)
        {
            SoundController.Instance.PlaySFX(SFX.NoStep);
            return;
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
        character.transform.Rotate(Vector3.back, current.angle * Time.deltaTime / stepTime, Space.World); 
        //body.transform.Rotate(current.rotation, 90f * Time.deltaTime / stepTime, Space.World);


        if(stepTimer >= stepTime)
        {
            transform.position = current.TargetPosition;
            //body.transform.rotation = current.TargetRotation;
            Debug.Log("Hands pointing: "+current.movement+","+Vector3.back);
            hands.transform.rotation = Quaternion.LookRotation(Vector3.forward, current.movement);
            character.transform.rotation = Quaternion.LookRotation(current.movement,Vector3.back);

            stepTimer = 0;
            moving = false;
            CheckForStored();
            SoundController.Instance.PlaySFX(SFX.TakeStep);
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
            //Debug.Log("Movement     Held: "+ Inputs.Instance.Controls.Main.Move.ReadValue<Vector2>());
            if (Inputs.Instance.Controls.Main.Move.IsPressed()) MoveInputAsVector(Inputs.Instance.Controls.Main.Move.ReadValue<Vector2>()); 
            else moving = false;
        }
    }

    public class MovementAction
    {
        public Vector3 movement;
        public float angle;
        public Vector3 startLookDirection;
        public Vector3 TargetPosition;
        public Vector3 TargetLookDirection;
        public int rotationIndex;

        public MovementAction(Vector3 currentPos, Vector3 startLookDir, Vector3 move, int rotIndex)
        {
            movement = move;
            startLookDirection = startLookDir;
            //Calulate Target
            TargetPosition = currentPos + move;
            TargetLookDirection = move;
            rotationIndex = rotIndex;
            angle = Vector3.SignedAngle(startLookDir, TargetLookDirection,Vector3.back);
        }
    }
    
    public class MovementActionOLD
    {
        public Vector3 movement;
        public Vector3 rotation;
        public Vector3 TargetPosition;
        public Quaternion TargetRotation;
        public int rotationIndex;

        public MovementActionOLD(Vector3 currentPos, Quaternion currentRot, Vector3 move, Vector3 rot, int rotIndex)
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
