using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
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
        Debug.Log("Entered Fall state");
        InitializeSubState();
    }

    public override void UpdateState()
    {
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

    }

    public void HandleGravity()
    {
        /*float previousYVelocity = Ctx.CurrentCameraRealtiveMovementY;
        Ctx.CurrentCameraRealtiveMovementY = Ctx.CurrentCameraRealtiveMovementY + Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentCameraRealtiveMovementY) * .5f, -20f);*/

        Ctx.CurrentCameraRealtiveMovementY += Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY += Ctx.Gravity * Time.deltaTime;
    }
}
