using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{
    private TentacleStateManager stateManager = null;

    public RetreatState(TentacleController controller) : base("Retreat", controller) => this.controller = controller;

    public override void EnterState(TentacleController controller)
    {
        this.controller = controller;
        stateManager = controller.stateManager;

        GameEvents.OnTentacleRetreat.Invoke(controller);
    }

    public override void ExitState()
    {
        controller.ResetTentacle();
    }

    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(controller.GetAnchorPosition());

        float distance = controller.GetDistanceBetweenEndPointAndHole();

        if (distance <= 1.5f)
           stateManager.TransitionStates(TentacleStates.Idle);

        controller.UpdateQueuedSegments();
    }

    public override void UpdateLateLogic()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions(controller.GetAnchorPosition());
    }
}
