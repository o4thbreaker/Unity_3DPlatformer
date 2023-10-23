using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
       : base(currentContext, playerStateFactory)
    {

    }

    public override void EnterState()
    {
        Debug.Log("Entered Walk state");
        Ctx.Animator.SetBool(Ctx.IS_WALKING, true);
    }
    public override void UpdateState()
    {
        Ctx.AppliedMovementX = Ctx.CurrentCameraRealtiveMovement.x;
        Ctx.AppliedMovementZ = Ctx.CurrentCameraRealtiveMovement.z;
        CheckSwitchStates();
    }
    public override void ExitState()
    {

    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        /*if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
        {
            SwitchState(Factory.Jump());
        }*/
    }
    public override void InitializeSubState()
    {

    }
}
