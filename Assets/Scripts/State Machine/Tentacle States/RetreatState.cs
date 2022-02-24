using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{
    TentacleProperties properties;
    TentacleStateManager stateManager;

    public RetreatState(TentacleController controller) : base("Retreat", controller) => this.controller = controller;

    public override void EnterState(TentacleController controller)
    {
        base.EnterState(controller);

        this.controller = controller;

        stateManager = controller.stateManager;
        properties = controller.GetTentacleProperties();
    }

    public override void ExitState()
    {
        base.ExitState();

        controller.ResetTentacle();
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        controller.UpdateAgentPosition(controller.GetAnchorPosition());

        float distance = controller.GetDistanceBetweenEndPointAndHole();

    //    if (distance <= 1.5f)
      //      stateManager.TransitionStates(TentacleStates.Idle);
    }

    public override void UpdatePhysics()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions(controller.GetAnchorPosition());
    }
}
