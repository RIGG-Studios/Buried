using UnityEngine;

public class AttackState : State
{
    private TentacleProperties properties = null;
    private TentacleStateManager stateManager = null;

    private bool detachedFromAnchor;
    private float attackTime;
    private float wrapTime;
    private float targetRotation;

    public AttackState(TentacleController controller) : base("Attack", controller) => this.controller = controller;

    public override void EnterState()
    {
        stateManager = controller.stateManager;
        properties = controller.GetTentacleProperties();
        player = Game.instance.player;

        controller.SetAgentPosition(controller.GetAnchorPosition());
        controller.occupied = true;

        GameEvents.OnTentacleAttackPlayer.Invoke(controller);
    }
    public override void ExitState()
    {
        detachedFromAnchor = false;
        attackTime = 0.0f;
        wrapTime = 0.0f;
    }

    public override void UpdateLogic()
    {
        float currentTentacleDistance = controller.GetDistanceBetweenEndPointAndAnchor();

        if (currentTentacleDistance >= 4 && !detachedFromAnchor)
            detachedFromAnchor = true;


        if ((currentTentacleDistance < 1 && detachedFromAnchor) || currentTentacleDistance >= properties.tentacleMaxLength || attackTime >= 5)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        float playerDistFromTentacle = controller.GetDistanceBetweenPlayerAndEndPoint();

        targetRotation = playerDistFromTentacle <= 2 ? 2f : 0f;

        controller.targetRotation = Mathf.Lerp(controller.targetRotation, targetRotation, Time.deltaTime * 5f);

        if (playerDistFromTentacle <= 0.5f)
        {
            wrapTime += Time.deltaTime;

            if (wrapTime > 0.75f)
            {
                stateManager.TransitionStates(TentacleStates.GrabPlayer);
            }
        }
        else
        {
            wrapTime = 0.0f;
        }

        attackTime += Time.deltaTime;

        controller.UpdateAgentPosition(Game.instance.player.GetPosition());
        controller.UpdateQueuedSegments();

    }

    public override void UpdateLateLogic()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions();
        controller.UpdateAgentTrackedPositions();
    }
}
