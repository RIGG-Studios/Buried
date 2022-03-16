using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : State
{
    private TentacleProperties properties = null;
    private TentacleStateManager stateManager = null;

    private bool detachedFromAnchor;
    private float attackTime;

    public AttackState(TentacleController controller) : base("Attack", controller) => this.controller = controller;

    public override void EnterState()
    {
        stateManager = controller.stateManager;
        properties = controller.GetTentacleProperties();

        controller.SetAgentPosition(controller.GetAnchorPosition());
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
        Vector3 playerPos = Game.instance.player.GetPosition();
        float currentTentacleDistance = controller.GetDistanceBetweenEndPointAndAnchor();

        if (currentTentacleDistance >= 4 && !detachedFromAnchor)
            detachedFromAnchor = true;


        if ((currentTentacleDistance < 1 && detachedFromAnchor) || currentTentacleDistance >= properties.tentacleMaxLength || attackTime >= 15f)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        float playerDistFromTentacle = controller.GetDistanceBetweenPlayerAndEndPoint();

        if (playerDistFromTentacle <= properties.lightDistance)
        {
        }

        if (playerDistFromTentacle <= properties.detectionRange)
        {
            stateManager.TransitionStates(TentacleStates.GrabPlayer);
        }

        attackTime += Time.deltaTime;

        controller.UpdateAgentPosition(playerPos);
        controller.UpdateSegmentPositions(playerPos);
    }

    public override void UpdatePhysics()
    {

    }
}
