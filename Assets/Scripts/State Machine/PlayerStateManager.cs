using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make an enum for all the possible states the player can be in
public enum PlayerStates
{
    Movement,
    GrabbedByTentacle,
    Dead
}

//this class handles the state transitoning/updating of the player
public class PlayerStateManager : StateMachine
{
    //create a reference to the player 
    private Player player { get { return GetComponent<Player>(); } }

    //create variables for all the player state classes
    private MovementState movementState = null;
    private GrabbedByTentacleState grabbedState = null;
    private PlayerDeadState deadState = null;

    private void Awake()
    {
        //create new instances of the states
        movementState = new MovementState(player);
        grabbedState = new GrabbedByTentacleState(player);
        deadState = new PlayerDeadState(player);

        //assign the current state to the movement state, because we want to start being able to move.
        currentState = movementState;
    }

    //method for transitioning states
    public void TransitionStates(PlayerStates state)
    {
        //find the corresponding state using the method below
        State newState = ConvertToState(state);

        if (newState != null)
        {
            //if everything is good, exit the old state and enter the new state.
            currentState.ExitState();
            currentState = newState;
            newState.EnterState();
        }
    }

    //method used for converting PlayerStates to a state class
    public State ConvertToState(PlayerStates state)
    {
        //check all possibilites
        switch (state)
        {
            //if we found any, return the corresponding state class.
            case PlayerStates.Movement:
                return movementState;

            case PlayerStates.GrabbedByTentacle:
                return grabbedState;

            case PlayerStates.Dead:
                return deadState;
        }

        //if nothing is found, return null
        return null;
    }
}
