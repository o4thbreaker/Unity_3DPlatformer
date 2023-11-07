using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
       : base(currentContext, playerStateFactory)
    {
        
    }

    public override void InitializeSubState()
    {
       
    }

    public override void EnterState()
    {
        Debug.Log("Entered Dash state");

        Ctx.IsDashing = true;
    }

    public override void UpdateState()
    {
        Dash();

        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.DashDuration <= 0f)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void ExitState()
    {
        Ctx.IsDashPressed = false;
        Ctx.IsDashing = false;
        Ctx.DashDuration = 0.25f;
        Ctx.DashCooldownTimer = Ctx.DashCooldown;
    }

    private void Dash()
    {
        if (Ctx.DashCooldownTimer > 0f)
        {
            return;
        }
        else
        {
            Ctx.DashDuration -= Time.deltaTime;

            Ctx.AppliedMovementX = Ctx.CurrentCameraRealtiveMovement.x * Ctx.DashingSpeed;
            Ctx.AppliedMovementZ = Ctx.CurrentCameraRealtiveMovement.z * Ctx.DashingSpeed;
        }
    }
}
