using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{
    private TentacleStateManager stateManager = null;

    public RetreatState(TentacleController controller) : base("Retreat", controller) => this.controller = controller;

    public override void EnterState()
    {
        stateManager = controller.stateManager;
        GameEvents.OnTentacleRetreat.Invoke(controller);

        GameEvents.OnPlayerDie += OnPlayerDead;
    }

    public override void ExitState()
    {
        controller.ResetTentacle();

        GameEvents.OnPlayerDie -= OnPlayerDead;
    }

    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(controller.GetAnchorPosition());

        float distance = controller.GetDistanceBetweenEndPointAndAnchor();

        if (distance <= 0.5f)
           stateManager.TransitionStates(TentacleStates.Idle);

        controller.UpdateQueuedSegments();
    }

    public override void UpdateLateLogic()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions();
        controller.UpdateAgentTrackedPositions();
    }

    public void OnPlayerDead()
    {
        stateManager.TransitionStates(TentacleStates.Idle);
    }
}
