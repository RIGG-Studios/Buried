using UnityEngine;

public class StateMachine : MonoBehaviour
{ 
    public State currentState { get; set; }

    public void Start()
    {
        if (currentState == null)
            return;

        currentState.EnterState();
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
}
