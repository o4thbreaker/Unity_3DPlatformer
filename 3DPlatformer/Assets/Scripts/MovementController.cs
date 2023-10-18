using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 7f;
    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private float groundedGravity = -.05f;
    [SerializeField] private float gravity = -9.8f;

    private PlayerInputActions playerInputActions;
    private CharacterController characterController;

    private Vector2 currentMovementInput; // wasd normalized ex W is (0,1)
    private Vector3 currentMovement; // player
    private Vector3 currentCameraRealtiveMovement;
    private bool isMovementPressed;

    private bool isJumpPressed = false;
    private float initialJumpVelocity;
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float maxJumpTime = 0.5f;
    private bool isJumping = false;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        characterController = GetComponent<CharacterController>(); // getcomponent finds for a attached component (in a menu )

        playerInputActions.Player.Move.started += OnMovementInput;
        playerInputActions.Player.Move.canceled += OnMovementInput;
        playerInputActions.Player.Move.performed += OnMovementInput;
        playerInputActions.Player.Jump.started += OnJump;
        playerInputActions.Player.Jump.canceled += OnJump;

        SetupJumpVariables();
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
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

    private void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2; // apex is a highest point of parabola aka highest jump point 

        gravity = (-2 * maxJumpHeight) / (timeToApex * timeToApex);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable(); // enables input system
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable(); // disables input system
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        bool isFalling = currentCameraRealtiveMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        if (characterController.isGrounded)
        {
            currentCameraRealtiveMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentCameraRealtiveMovement.y;
            float newYVelocity = currentCameraRealtiveMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentCameraRealtiveMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentCameraRealtiveMovement.y;
            float newYVelocity = currentCameraRealtiveMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentCameraRealtiveMovement.y = nextYVelocity;
        }
    }

    private void HandleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        { 
            isJumping = true;
            currentCameraRealtiveMovement.y = initialJumpVelocity * .5f;
        }
        else if (isJumping && characterController.isGrounded && !isJumpPressed)
        {
            isJumping = false;
        }
    }

    private void Update()
    {
        HandleRotation();

        characterController.Move(currentCameraRealtiveMovement * Time.deltaTime * playerSpeed);

        HandleGravity();
        HandleJump(); 
    }
}
