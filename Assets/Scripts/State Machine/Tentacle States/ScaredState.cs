using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaredState : State
{
    private TentacleStateManager stateManager = null;
    private float timer;

    public ScaredState(TentacleController controller, Player player) : base("Scared", controller)
    {
        this.player = player;
        this.controller = controller;
    }

    public override void EnterState()
    {
        stateManager = controller.stateManager;
    }

    public override void UpdateLogic()
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            stateManager.TransitionStates(TentacleStates.GrabPlayer);
            timer = 0.0f;
        }
    }

    public override void UpdatePhysics()
    {
        controller.UpdateSegmentPositions(controller.GetAnchorPosition());
    }
}
