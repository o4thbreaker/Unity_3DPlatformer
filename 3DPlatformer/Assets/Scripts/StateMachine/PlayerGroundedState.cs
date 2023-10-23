using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Entered Grounded state");

        HandleGravity();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {

    }
    public override void CheckSwitchStates()
    {
        // if player is grounded and jump is pressed => switch to jump state
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress) 
        {
            SwitchState(Factory.Jump());
        }
        else if (!Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
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

    public void HandleGravity()
    {
        Ctx.CurrentCameraRealtiveMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }
}
