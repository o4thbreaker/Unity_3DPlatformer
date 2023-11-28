using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float initialWalkingSpeed; // desiredMoveSpeed
    private float initialDashingSpeed; // moveSpeed
    private float speedDifference;
    private float endingDashTimer;
    private bool isEndingDash;

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
        Ctx.Animator.SetBool(Ctx.IS_DASHING, true);

        initialWalkingSpeed = Ctx.AppliedMovementZ;
        initialDashingSpeed = Ctx.AppliedMovementZ * Ctx.DashingSpeed;
        speedDifference = Mathf.Abs(initialWalkingSpeed - initialDashingSpeed);
        endingDashTimer = 0f;
        Ctx.IsDashPressed = false;
        isEndingDash = false;
        Ctx.IsDashing = true;
    }

    public override void UpdateState()
    {
        Dash();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.DashDuration <= 0f && !isEndingDash && Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Walk());
        }
        else if (Ctx.DashDuration <= 0f && !isEndingDash && !Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Dash state");
        Ctx.Animator.SetBool(Ctx.IS_DASHING, false);
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

        if (Ctx.DashDuration <= 0f)
        {
            if (endingDashTimer >= speedDifference)
            {
                isEndingDash = false;
            }
            else
                isEndingDash = true;

            if (isEndingDash)
            {
                Ctx.AppliedMovementZ = Mathf.Lerp(initialDashingSpeed, initialWalkingSpeed, endingDashTimer / speedDifference);
                endingDashTimer += Time.deltaTime * Ctx.EndingDashDurationBoost;
            }
        }
    }
}
