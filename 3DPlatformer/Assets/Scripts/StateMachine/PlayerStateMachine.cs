using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float playerSpeed = 7f;
    [SerializeField] private float rotationSpeed = 1f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.8f;

    [Header("States")]
    private const string is_walking = "isWalking";
    private const string is_jumping = "isJumping";

    [Header("Components")]
    private PlayerInputActions playerInputActions;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement")]
    private Vector2 currentMovementInput; // wasd normalized ex W is (0,1)
    private Vector3 currentMovement; // player
    private Vector3 currentCameraRealtiveMovement;
    private Vector3 appliedMovement; // movement after calculating vervlet velocity to jump
    private bool isMovementPressed;

    [Header("Jumping")]
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float maxJumpTime = 0.5f;
    private bool isJumpPressed = false;
    private bool isDoubleJumpPressed = false;
    private bool isJumping = false;
    private bool isDoubleJumping;
    private bool requireNewJumpPress = false;
    private float initialJumpVelocity;

    [Header("Dashing")]
    [SerializeField] private float dashingSpeed = 20f;
    [SerializeField] private float dashDuration = .25f;
    [SerializeField] private float dashCooldown = .25f;
    private bool isDashPressed = false;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    [Header("StateFactory")]
    private PlayerBaseState currentState;
    private PlayerStateFactory states;

    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Animator Animator { get { return animator; } }

    public Vector2 CurrentMovementInput { get { return currentMovementInput; } }
    public Vector3 CurrentCameraRealtiveMovement { get { return currentCameraRealtiveMovement; } }
    public float CurrentCameraRealtiveMovementY { get { return currentCameraRealtiveMovement.y; } set { currentCameraRealtiveMovement.y = value; } }

    public bool IsMovementPressed { get { return isMovementPressed; } }
    public bool IsJumpPressed { get { return isJumpPressed; } set { isJumpPressed = value; } }
    public bool IsDoubleJumpPressed { get { return isDoubleJumpPressed; } }
    public bool IsDashPressed { get { return isDashPressed; }  set { isDashPressed = value; } }

    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public bool IsDoubleJumping { get { return isDoubleJumping; } set { isDoubleJumping = value; } }
    public bool RequireNewJumpPress { get { return requireNewJumpPress; } set { requireNewJumpPress = value; } }
    public float InitialJumpVelocity { get { return initialJumpVelocity; } }

    public string IS_JUMPING { get { return is_jumping; } }
    public string IS_WALKING { get { return is_walking; } }
   
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; } }
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; } }
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; } }

    public float PlayerSpeed { get { return playerSpeed;} }
    public float Gravity { get { return gravity; } }

    public float DashingSpeed { get { return dashingSpeed; } set { dashingSpeed = value; } }
    public bool IsDashing { get { return isDashing; } set { isDashing = value; } }
    public float DashDuration { get { return dashDuration; } set { dashDuration = value; } }
    public float DashCooldown { get { return dashCooldown; } set { dashCooldown = value; } }
    public float DashCooldownTimer { get { return dashCooldownTimer; } set { dashCooldownTimer = value; } }

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
        playerInputActions.Player.Dash.performed += OnDash;
        
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

    private void OnDash(InputAction.CallbackContext context)
    {
        isDashPressed = context.ReadValueAsButton();
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

        if (dashCooldownTimer > 0f) // may not be the best solution but i couldnt find any better
        {
            dashCooldownTimer -= Time.deltaTime;
        }
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
