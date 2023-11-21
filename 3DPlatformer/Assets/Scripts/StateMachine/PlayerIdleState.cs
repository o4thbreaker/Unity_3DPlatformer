using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
       : base(currentContext, playerStateFactory)
    {

    }

    public override void InitializeSubState()
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Entered Idle state");
        Ctx.Animator.SetBool(Ctx.IS_WALKING, false);
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
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
        if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Walk());
        }
        else if (Ctx.IsDashPressed)
        {
            SwitchState(Factory.Dash());
        }
    }
}
