using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 7f;
    [SerializeField] private float rotationSpeed = 1f;

    //[SerializeField] private float groundedGravity = -.05f;
    [SerializeField] private float gravity = -9.8f;

    private const string is_walking = "isWalking";
    private const string is_jumping = "isJumping";

    private PlayerInputActions playerInputActions;
    private CharacterController characterController;
    private Animator animator;

    private Vector2 currentMovementInput; // wasd normalized ex W is (0,1)
    private Vector3 currentMovement; // player
    private Vector3 currentCameraRealtiveMovement;
    private Vector3 appliedMovement; // movement after calculating vervlet velocity to jump
    private bool isMovementPressed;

    private bool isJumpPressed = false;
    private bool isDoubleJumpPressed = false;
    private bool isJumping = false;
    private bool isDoubleJumping;
    private bool requireNewJumpPress = false;
    private float initialJumpVelocity;
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float maxJumpTime = 0.5f;

    private PlayerBaseState currentState;
    private PlayerStateFactory states;


    /*private Vector3 cameraY = Camera.main.transform.up;
    public Vector3 CameraY { get { return cameraY; } set { cameraY = value; } }*/

    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Vector2 CurrentMovementInput { get { return currentMovementInput; } }
    public Vector3 CurrentCameraRealtiveMovement { get { return currentCameraRealtiveMovement; } }
    public Animator Animator { get { return animator; } }
    public bool IsJumpPressed { get { return isJumpPressed; } }
    public bool IsDoubleJumpPressed { get { return isDoubleJumpPressed; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public bool IsDoubleJumping { get { return isDoubleJumping; } set { isDoubleJumping = value; } }
    public string IS_JUMPING { get { return is_jumping; } }
    public string IS_WALKING { get { return is_walking; } }
    public bool RequireNewJumpPress { get { return requireNewJumpPress; } set { requireNewJumpPress = value; } }
    public float InitialJumpVelocity { get { return initialJumpVelocity; } }
    public bool IsMovementPressed {  get { return isMovementPressed; } }
    public float CurrentCameraRealtiveMovementY { get { return currentCameraRealtiveMovement.y; } set { currentCameraRealtiveMovement.y = value; } }
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; } }
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; } }
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; } }
    public float PlayerSpeed { get { return playerSpeed;} }
    public float Gravity { get { return gravity; } }



    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        characterController = GetComponent<CharacterController>(); // getcomponent finds for a attached component (in a menu )
        animator = GetComponent<Animator>();

        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState(); // in the PlayerGroundedState

        playerInputActions.Player.Move.started += OnMovementInput;
        playerInputActions.Player.Move.canceled += OnMovementInput;
        playerInputActions.Player.Move.performed += OnMovementInput;
        playerInputActions.Player.Jump.performed += OnJump;
        
        SetupJumpVariables();
    }

    private void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2; // apex is a highest point of parabola aka highest jump point 

        gravity = (-2 * maxJumpHeight) / (timeToApex * timeToApex); // ????????
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex; // = 10.(6)
    }

    void Start()
    {
        characterController.Move(appliedMovement * Time.deltaTime);
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * playerSpeed;
        currentMovement.z = currentMovementInput.y * playerSpeed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

        float playerVerticalInput = currentMovement.z;
        float playerHorizontalInput = currentMovement.x;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 forwardRelativeVerticalInput = playerVerticalInput * cameraForward;
        Vector3 rightRelativeHorizontalInput = playerHorizontalInput * cameraRight;

        currentCameraRealtiveMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        float delay = Time.time * 0.3f;

        isJumpPressed = context.ReadValueAsButton(); // true once jump is pressed
        isDoubleJumpPressed = isJumpPressed && Time.time > delay && !characterController.isGrounded; // true once jump is pressed mid air

        requireNewJumpPress = false;
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentCameraRealtiveMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentCameraRealtiveMovement.z;

        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        HandleRotation();
        characterController.Move(appliedMovement * Time.deltaTime);
        currentState.UpdateStates();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable(); // enables input system
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable(); // disables input system
    }
}
