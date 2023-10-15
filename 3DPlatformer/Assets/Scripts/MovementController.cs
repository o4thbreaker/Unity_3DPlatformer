using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;

    private PlayerInputActions playerInputActions;
    private CharacterController characterController;

    private Vector2 currentMovementInput; // wasd normalized ex W is (0,1)
    private Vector3 currentMovement; // player
    private bool isMovementPressed;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        characterController = GetComponent<CharacterController>(); // getcomponent finds for a attached component (in a menu )

        playerInputActions.Player.Move.started += OnMovementInput;
        playerInputActions.Player.Move.canceled += OnMovementInput;
        playerInputActions.Player.Move.performed += OnMovementInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
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

    private void HandleMovement()
    {
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

        Vector3 cameraRealtiveMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;

        characterController.Move(cameraRealtiveMovement * Time.deltaTime * playerSpeed);
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
    }
}
