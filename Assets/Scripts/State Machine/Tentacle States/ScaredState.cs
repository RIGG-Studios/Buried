using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaredState : State
{
    private TentacleStateManager stateManager = null;
    private float timer;

    public ScaredState(TentacleController controller) : base("Scared", controller) => this.controller = controller;

    public override void EnterState()
    {
        stateManager = controller.stateManager;
    }

    public override void UpdateLogic()
    {
        controller.targetRotation = Random.Range(5f, 9f);

        timer += Time.deltaTime;

        Debug.Log(timer);
        if (timer >= 1)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
            timer = 0.0f;
        }
    }

    public override void UpdatePhysics()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions(Vector3.zero);
        controller.UpdateAgentTrackedPositions();
    }
}
