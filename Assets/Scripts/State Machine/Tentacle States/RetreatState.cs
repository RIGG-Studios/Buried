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
        controller.ResetTentacle();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        controller.UpdateAgentPosition(controller.GetAnchorPosition());

        float distance = controller.GetDistanceBetweenEndPointAndHole();

        if (distance <= 1f)
        {
            stateManager.TransitionStates(TentacleStates.Idle);
        }

        controller.UpdateTentacleRotation(3f, 4f);
    }

    public override void UpdateLateLogic()
    {
        base.UpdateLateLogic();

        Vector3 dir = controller.GetAnchorPosition();

        controller.UpdateSegments(dir);
    }
}
