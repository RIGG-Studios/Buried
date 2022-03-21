using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : State
{
    private TentacleProperties properties = null;
    private TentacleStateManager stateManager = null;

    private bool detachedFromAnchor;
    private float attackTime;

    public AttackState(TentacleController controller, Player player) : base("Attack", controller)
    {
        this.player = player;
        this.controller = controller;
    }

    public override void EnterState()
    {
        stateManager = controller.stateManager;
        properties = controller.GetTentacleProperties();
        controller.SetAgentPosition(controller.GetAnchorPosition());
        controller.targetSpeed = properties.tentacleMoveSpeed;
        controller.occupied = true;

        GameEvents.OnTentacleAttackPlayer.Invoke(controller);
    }
    public override void ExitState()
    {
        detachedFromAnchor = false;
        attackTime = 0.0f;
    }

    public override void UpdateLogic()
    {
        Vector3 playerPos = player.GetPosition();
        float currentTentacleDistance = controller.GetTentacleDistance();
        float playerDistFromTentacle = controller.GetDistanceBetweenPlayerAndEndPoint();

        if (currentTentacleDistance >= 4 && !detachedFromAnchor)
            detachedFromAnchor = true;

        if ((currentTentacleDistance < 1 && detachedFromAnchor) || currentTentacleDistance >= properties.tentacleMaxLength || attackTime >= 15f)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        if (controller.IsTentacleScared(playerDistFromTentacle))
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        if (playerDistFromTentacle <= properties.detectionRange)
        {
            stateManager.TransitionStates(TentacleStates.GrabPlayer);
        }

        attackTime += Time.deltaTime;

        controller.UpdateAgentPosition(playerPos);
        controller.UpdateSegmentPositions(playerPos);
        controller.CheckForLights();
    }
}
