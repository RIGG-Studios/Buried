using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Movement,
    GrabbedByTentacle,
    Dead
}

public class PlayerStateManager : StateMachine
{
    private Player player { get { return GetComponent<Player>(); } }

    private MovementState movementState = null;
    private GrabbedByTentacleState grabbedState = null;
    private PlayerDeadState deadState = null;
    private void Awake()
    {
        movementState = new MovementState(player);
        grabbedState = new GrabbedByTentacleState(player);
        deadState = new PlayerDeadState(player);

        currentState = movementState;
    }

    public void TransitionStates(PlayerStates state)
    {
        State newState = ConvertToState(state);

        if (newState != null)
        {
            Debug.Log(newState);
            currentState.ExitState();
            currentState = newState;
            newState.EnterState();
        }
    }

    public State ConvertToState(PlayerStates state)
    {
        switch (state)
        {
            case PlayerStates.Movement:
                return movementState;

            case PlayerStates.GrabbedByTentacle:
                return grabbedState;

            case PlayerStates.Dead:
                return deadState;
        }

        return null;
    }
}
