using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TentacleStates
{
    Idle,
    Attack,
    Retreat,
    Scared,
    Stunned
}

public class TentacleStateManager : MonoBehaviour
{
    TentacleController controller;
    public State currentState { get; private set; }

    public AttackState attackState { get; private set; }
    public RetreatState retreatState { get; private set; }
    public StunnedState stunnedState { get; private set; }
    public ScaredState scaredState { get; private set; }
    public IdleState idleState { get; private set; }

    private void Awake()
    {
        controller = GetComponent<TentacleController>();

        attackState = new AttackState(controller);
        idleState = new IdleState(controller);
        retreatState = new RetreatState(controller);

        currentState = idleState;
    }

    public void Start()
    {
        if (currentState == null)
            return;

        currentState.EnterState(controller);
    }

    public void Update()
    {
        if (currentState == null)
            return;

        currentState.UpdateLogic();
    }

    public void LateUpdate()
    {
        if (currentState == null)
            return;

        currentState.UpdateLateLogic();
    }

    public void FixedUpdate()
    {
        if (currentState == null)
            return;

        currentState.UpdatePhysics();
    }

    public void TransitionStates(TentacleStates state)
    {
        State newState = ConvertToState(state);

        if (newState != null) 
        {
            currentState.ExitState();
            currentState = newState;
            newState.EnterState(controller);
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
        }

        return null;
    }
}
