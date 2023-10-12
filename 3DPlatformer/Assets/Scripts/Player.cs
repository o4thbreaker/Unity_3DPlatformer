using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7f;
    private bool isWalking;
    
    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //cannot move towards this dir
            //attempt only x (left/right) movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) 
            {
                //can move only x
                moveDir = moveDirX;
            }
            else
            {
                //cannot move only on the x
                // attempt only z (forward/backward)
                Vector3 moveDirZ = new Vector3(moveDir.z,0, 0).normalized;
                canMove = !Physics.CapsuleCast(transform.position, Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                
                if (canMove)
                {
                    //can move only z
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move any dir
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
    }

}
