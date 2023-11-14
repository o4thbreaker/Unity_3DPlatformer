using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
       : base(currentContext, playerStateFactory)
    {

    }

    public override void InitializeSubState()
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Entered Walk state");

        InitializeSubState();
        Ctx.Animator.SetBool(Ctx.IS_WALKING, true);
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementX = Ctx.CurrentCameraRealtiveMovement.x;
        Ctx.AppliedMovementZ = Ctx.CurrentCameraRealtiveMovement.z;
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsDashPressed)
        {
            SwitchState(Factory.Dash());
        }
    }

    public override void ExitState()
    {

    }
}
