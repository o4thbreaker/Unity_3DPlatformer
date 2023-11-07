using System.Collections.Generic;

enum PlayerStates
{
    Idle,
    Walk,
    Jump,
    Grounded,
    Fall,
    Dash
}

public class PlayerStateFactory 
{
    private PlayerStateMachine context;
    private Dictionary<PlayerStates, PlayerBaseState> states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
        states[PlayerStates.Idle] = new PlayerIdleState(context, this);
        states[PlayerStates.Walk] = new PlayerWalkState(context, this);
        states[PlayerStates.Jump] = new PlayerJumpState(context, this);
        states[PlayerStates.Grounded] = new PlayerGroundedState(context, this);
        states[PlayerStates.Fall] = new PlayerFallState(context, this);
        states[PlayerStates.Dash] = new PlayerDashState(context, this);
    }

    public PlayerBaseState Idle()
    {
        return states[PlayerStates.Idle];
    }

    public PlayerBaseState Walk()
    {
        return states[PlayerStates.Walk];
    }

    public PlayerBaseState Jump()
    {
        return states[PlayerStates.Jump];
    }

    public PlayerBaseState Grounded()
    {
        return states[PlayerStates.Grounded];
    }

    public PlayerBaseState Fall()
    {
        return states[PlayerStates.Fall];
    }

    public PlayerBaseState Dash()
    {
        return states[PlayerStates.Dash];
    }
}
