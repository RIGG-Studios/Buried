using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{
    private TentacleStateManager stateManager = null;
    private TentacleProperties properties;
    private float retreatTimer = 0.0f;

    public RetreatState(TentacleController controller, Player player) : base("Retreat", controller)
    {
        this.player = player;
        this.controller = controller;
        properties = controller.GetTentacleProperties();
    }

    public override void EnterState()
    {
        stateManager = controller.stateManager;
        controller.targetSpeed = 0.05f;

        GameEvents.OnTentacleRetreat?.Invoke(controller);
        GameEvents.OnPlayerDie += OnPlayerDead;

        float dist = controller.GetDistanceBetweenPlayerAndEndPoint();

        if(dist >= 2.5f)
        {
            controller.SetAgentPosition(controller.GetTentacleEndPoint());
        }

        controller.targetSpeed = 0.1f;
    }

    public override void ExitState()
    {
        controller.ResetTentacle();
        retreatTimer = 0.0f;
        GameEvents.OnPlayerDie -= OnPlayerDead;
    }

    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(controller.GetAnchorPosition());
        retreatTimer += Time.deltaTime;

        float distance = controller.GetTentacleDistance();

        if (distance <= 1f || retreatTimer >= 10f)
           stateManager.TransitionStates(TentacleStates.Idle);

        controller.UpdateSegmentPositions(controller.GetAgentPosition());
        controller.CheckForLights();
    }

    public void OnPlayerDead()
    {
        stateManager.TransitionStates(TentacleStates.Idle);
    }
}
