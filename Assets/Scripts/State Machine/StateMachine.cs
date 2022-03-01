using UnityEngine;

//class for handling state machine functionality, its in a seperate class so state machines can be made for different objects. e.x (Player, Tentacles)
public class StateMachine : MonoBehaviour
{ 
    //every state machine will have an active state, so we create a variable here
    public State currentState { get; set; }

    public void Start()
    {   
        //when we start, enter the current state
        if (currentState == null)
            return;

        currentState.EnterState();
    }

    public void Update()
    {
        //every frame, we want to update the current state update logic
        if (currentState == null)
            return;

        currentState.UpdateInput();

        currentState.UpdateLogic();
    }

    public void LateUpdate()
    {
        //update the current state late update logic
        if (currentState == null)
            return;

        currentState.UpdateLateLogic();
    }

    public void FixedUpdate()
    {
        //update the current state physics logic
        if (currentState == null)
            return;

        currentState.UpdatePhysics();
    }
}
