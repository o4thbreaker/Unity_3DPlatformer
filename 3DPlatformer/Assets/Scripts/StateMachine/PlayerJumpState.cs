using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory factory) 
        : base(currentContext, factory)
    {
        IsRootState = true;
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Walk());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Entered Jump state");

        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState()
    {
        HandleDoubleJump();
        HandleGravity();

        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IS_JUMPING, false);
        Ctx.IsJumping = false;
        if (Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }   
    }

    private void HandleJump()
    {
        Ctx.Animator.SetBool(Ctx.IS_JUMPING, true);

        Ctx.IsJumping = true;
        Ctx.IsDoubleJumping = false;

        Ctx.CurrentCameraRealtiveMovementY = Ctx.InitialJumpVelocity;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocity;
    }

    private void HandleDoubleJump()
    { 
        if (Ctx.IsDoubleJumpPressed && !Ctx.IsDoubleJumping)
        {
            Ctx.IsDoubleJumping = true;

            Ctx.CurrentCameraRealtiveMovementY = Ctx.InitialJumpVelocity;
            Ctx.AppliedMovementY = Ctx.InitialJumpVelocity;
        }
    }

    public void HandleGravity()
    {
        bool isFalling = Ctx.CurrentCameraRealtiveMovementY <= 0.0f || !Ctx.IsJumpPressed;

        float fallMultiplier = 2.0f;
        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentCameraRealtiveMovementY;
            Ctx.CurrentCameraRealtiveMovementY = Ctx.CurrentCameraRealtiveMovementY + (Ctx.Gravity * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentCameraRealtiveMovementY) * .5f, -20f);
        }
        else
        {
            float previousYVelocity = Ctx.CurrentCameraRealtiveMovementY;
            Ctx.CurrentCameraRealtiveMovementY = Ctx.CurrentCameraRealtiveMovementY + (Ctx.Gravity * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentCameraRealtiveMovementY) * .5f;
        }
    }
}
