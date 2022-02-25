using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private TentacleProperties properties = null;
    private TentacleStateManager stateManager = null;

    bool detachedFromAnchor;

    public AttackState(TentacleController controller) : base("Attack", controller) => this.controller = controller;

    public override void EnterState(TentacleController controller)
    {
        this.controller = controller;

        stateManager = controller.stateManager;
        properties = controller.GetTentacleProperties();

        controller.SetAgentPosition(controller.GetAnchorPosition());
        controller.occupied = true;

        GameEvents.OnTentacleAttackPlayer.Invoke(controller);
    }
    public override void ExitState()
    {
        detachedFromAnchor = false;
    }

    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(Game.instance.player.GetPosition());

        float currentTentacleDistance = controller.GetDistanceBetweenEndPointAndHole();

        if (currentTentacleDistance >= 4 && !detachedFromAnchor)
            detachedFromAnchor = true;
        else if(currentTentacleDistance < 1 && detachedFromAnchor)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        if(currentTentacleDistance > properties.tentacleMaxLength)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        float playerDistFromTentacle = controller.GetDistanceBetweenPlayerAndEndPoint();

        if(playerDistFromTentacle <= properties.lightDistance && Game.instance.player.flashLightEnabled)
        {
            stateManager.TransitionStates(TentacleStates.Scared);
        }


        controller.UpdateQueuedSegments();

    }

    public override void UpdateLateLogic()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions(Vector3.zero);
        controller.UpdateAgentTrackedPositions();
    }

}
