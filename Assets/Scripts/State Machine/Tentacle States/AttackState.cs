using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    TentacleProperties properties;
    TentacleStateManager stateManager;

    public AttackState(TentacleController controller) : base("Attack", controller) => this.controller = controller;

    public override void EnterState(TentacleController controller)
    {
        this.controller = controller;

        stateManager = controller.stateManager;
        properties = controller.GetTentacleProperties();

        controller.SetAgentPosition(controller.GetAnchorPosition());
        controller.occupied = true;
    }
    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(Game.instance.player.GetPosition());

        float distance = controller.GetDistanceBetweenEndPointAndHole();

        if (distance >= properties.tentacleMaxLength)
        {
       //     stateManager.TransitionStates(TentacleStates.Retreat);
        }

        controller.UpdateQueuedSegments();
    }

    public override void UpdateLateLogic()
    {
        Vector3 targetDir = controller.GetAgentPosition();

        controller.UpdateSegments(targetDir);
        controller.UpdateSegmentCount();
        controller.UpdateAgentTrackedPositions();
    }
}
