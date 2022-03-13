using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{
    private TentacleStateManager stateManager = null;
    private float retreatTimer = 0.0f;

    public RetreatState(TentacleController controller) : base("Retreat", controller) => this.controller = controller;

    public override void EnterState()
    {
        stateManager = controller.stateManager;
        controller.GetAgent().speed = 10f;

        GameEvents.OnTentacleRetreat.Invoke(controller);
        GameEvents.OnPlayerDie += OnPlayerDead;
    }

    public override void ExitState()
    {
        controller.GetAgent().speed = 4.75f;
        controller.ResetTentacle();
        retreatTimer = 0.0f;
        GameEvents.OnPlayerDie -= OnPlayerDead;
    }

    public override void UpdateLogic()
    {
        retreatTimer += Time.deltaTime;
        controller.UpdateAgentPosition(controller.GetAnchorPosition());

        float distance = controller.GetDistanceBetweenEndPointAndAnchor();

        if (distance <= 1f || retreatTimer >= 10f)
           stateManager.TransitionStates(TentacleStates.Idle);
    }

    public override void UpdateLateLogic()
    {
        controller.UpdateSegmentPositions();
        controller.UpdateAgentTrackedPositions();
    }

    public void OnPlayerDead()
    {
        stateManager.TransitionStates(TentacleStates.Idle);
    }
}
