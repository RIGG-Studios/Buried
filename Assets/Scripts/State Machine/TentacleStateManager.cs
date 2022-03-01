using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for handling the tentacle state manager

//create an enum for all possible states a tentacle could be in
public enum TentacleStates
{
    Idle,
    Attack,
    Retreat,
    Scared,
    Stunned,
    GrabPlayer
}

//derive from the state machine class
public class TentacleStateManager : StateMachine
{
    //refernce to the controller
    private TentacleController controller { get { return GetComponent<TentacleController>(); } }

    //reference to all the state classes
    private AttackState attackState;
    private RetreatState retreatState;
    private StunnedState stunnedState;
    private ScaredState scaredState;
    private IdleState idleState;
    private GrabPlayerState grabState;

    //when we awake, assign all the variables to their proper values.
    private void Awake()
    {
        //create new instances of all the states
        attackState = new AttackState(controller);
        idleState = new IdleState(controller);
        retreatState = new RetreatState(controller);
        scaredState = new ScaredState(controller);
        grabState = new GrabPlayerState(controller);

        //set the current state to be the idle state for the tentacle
        currentState = idleState;
    } 

    //method for transtioning states
    public void TransitionStates(TentacleStates state)
    {
        //find the corresponding state using the method below
        State newState = ConvertToState(state);

        //if its found, continue
        if (newState != null) 
        {
            //exit and old state and enter the new one.
            currentState.ExitState();
            currentState = newState;
            newState.EnterState();
        }
    }

    //simple method for converting states from the TentacleStates enum
    public State ConvertToState(TentacleStates state)
    {
        //check all possibilites
        switch (state)
        {
            //if we found any states, return the state instance
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

        //if none are found, return null.
        return null;
    }
}
