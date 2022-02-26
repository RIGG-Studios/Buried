using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TentacleStates
{
    Idle,
    Attack,
    Retreat,
    Scared,
    Stunned,
    GrabPlayer
}

public class TentacleStateManager : StateMachine
{
    private TentacleController controller = null;

    private AttackState attackState;
    private RetreatState retreatState;
    private StunnedState stunnedState;
    private ScaredState scaredState;
    private IdleState idleState;
    private GrabPlayerState grabState;

    private void Awake()
    {
        controller = GetComponent<TentacleController>();

        attackState = new AttackState(controller);
        idleState = new IdleState(controller);
        retreatState = new RetreatState(controller);
        scaredState = new ScaredState(controller);
        grabState = new GrabPlayerState(controller);

        currentState = idleState;
    } 
    public void TransitionStates(TentacleStates state)
    {
        State newState = ConvertToState(state);

        if (newState != null) 
        {
            currentState.ExitState();
            currentState = newState;
            newState.EnterState();
        }
    }

    public State ConvertToState(TentacleStates state)
    {
        switch (state)
        {
            case TentacleStates.Attack:
                return attackState;

            case TentacleStates.Idle:
                return idleState;

            case TentacleStates.Retreat:
                return retreatState;

            case TentacleStates.Scared:
                return scaredState;

            case TentacleStates.GrabPlayer:
                return grabState;
        }

        return null;
    }
}
