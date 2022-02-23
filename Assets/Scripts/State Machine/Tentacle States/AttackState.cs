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

        controller.SetAgentPosition(controller.GetAnchorPosition());
        controller.occupied = true;

        stateManager = controller.stateManager;
        properties = controller.GetTentacleProperties();
    }

    public override void UpdateLogic()
    {
        Vector3 playerPos = Game.instance.player.GetPosition();
        controller.UpdateAgentPosition(playerPos);

        float distance = controller.GetDistanceBetweenEndPointAndHole();

        if (distance >= properties.tentacleMaxLength)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        float playerDistance = controller.GetDistanceBetweenPlayerAndEndPoint();

        if (playerDistance <= 3f)
        {
            controller.UpdateTentacleRotation(2f, 4f);
        }
        else
        {
            controller.UpdateTentacleRotation(0f, 4f);
        }

        Vector3 targetDir = controller.GetAgentPosition();
        controller.UpdateSegments(targetDir);
    }
}
