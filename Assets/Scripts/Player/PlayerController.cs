using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerHolder;
    [SerializeField] GameObject character;
    [SerializeField] Animator animator;

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
    private float idleTimer = 0;
    private const float IdleRelaxTime = 13f;

    private bool moving = false;
    MovementAction newMovement;

    Vector2Int initPosition = new Vector2Int(5,4);
    Vector2Int target = new Vector2Int(5,5);

    public Section holdingSection;
    private bool placedDuringWalkCycle;
    [SerializeField] private PlayerAvatarController playerAvatarController;

    public Vector2Int Position { get; private set; }

    public void SetNewCharacter(GameObject newCharacter)
    {
        character = newCharacter;
        animator = newCharacter.GetComponent<Animator>();
    }
    
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
        {
            bool didPickup = levelCreator.PickupSectionAt(Position, target, current.rotationIndex);
            if (didPickup)
            {
                animator.CrossFade("LiftAnimation", 0.1f);
                playerAvatarController.Hold(true);
            }
        }
        else
        {
            bool didPlace = levelCreator.PlaceHeldSectionAt(target, current.rotationIndex);
            if (didPlace)
            {
                placedDuringWalkCycle = true;
                animator.CrossFade("LiftAnimation", 0.1f);
                playerAvatarController.Hold(false);
            }

        }
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
        else Idle();
    }

    private void Idle()
    {
        idleTimer += Time.deltaTime;
        if(idleTimer > IdleRelaxTime)
        {
            float random = Random.Range(0,1f);
            if(random<0.15f)
                animator.CrossFade("Cast Spell", 0.1f);
            else if (random < 0.3f)
                animator.CrossFade("Roar", 0.1f);
            else
                animator.CrossFade("Idle Relax",0.1f);

            idleTimer = 0;
        }
    }

    private void DoMove()
    {
        // Place actual player at the target position directly so picking up tiles act from this position even during the move animation
        if (stepTimer == 0)
        {
            if(levelCreator.heldSection == null && playerAvatarController.Grabbing)
            {
                playerAvatarController.PretendToGrab(false);
            }
            PlacePlayerAtIndex();
            levelCreator.UpdateHeld(target, current.rotationIndex);
            GameSettings.StepsCounter++;
            animator.CrossFade("Run", 0.1f);
        }

        stepTimer += Time.deltaTime;
        
        transform.position += current.movement* Time.deltaTime/stepTime;
        character.transform.Rotate(Vector3.back, current.angle * Time.deltaTime / stepTime, Space.World); 


        if(stepTimer >= stepTime)
        {
            transform.position = current.TargetPosition;
            character.transform.rotation = Quaternion.LookRotation(current.movement,Vector3.back);

            stepTimer = 0;
            moving = false;
            CheckForStored();
            SoundController.Instance.PlaySFX(SFX.FootStep);
            animator.CrossFade("Idle", 0.1f);
            idleTimer = 0;

            //Check here if pickable tile is in front of player and do sheke if so
            GameTile tile = levelCreator.GetSectionAt(Position, target);

            if (tile != null && levelCreator.heldSection == null)
            {
                if(!placedDuringWalkCycle)
                    tile.section.ShakeTile(current.rotationIndex);
                playerAvatarController.PretendToGrab(true);
            }
            placedDuringWalkCycle = false;
        }
    }

    private void PlacePlayerAtIndex()
    {
        Position = new Vector2Int(Mathf.RoundToInt(current.TargetPosition.x), Mathf.RoundToInt(current.TargetPosition.y));
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
