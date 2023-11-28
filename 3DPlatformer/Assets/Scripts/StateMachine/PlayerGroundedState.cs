using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory)
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
        Debug.Log("Entered Grounded state");

        Ctx.StepOffset = 0.1f;

        InitializeSubState();
        HandleGravity();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        // if player is grounded and jump is pressed => switch to jump state
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress && !Ctx.IsDashing)
        {
            SwitchState(Factory.Jump());
        }
        else if (!Ctx.CharacterController.isGrounded && !Ctx.IsDashing)
        {
            SwitchState(Factory.Fall());
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Grounded state");
    }
    
    public void HandleGravity()
    {
        Ctx.CurrentCameraRealtiveMovementY = Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY = Ctx.Gravity * Time.deltaTime;
    }
}
