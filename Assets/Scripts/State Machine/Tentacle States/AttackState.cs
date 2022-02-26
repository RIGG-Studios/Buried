using UnityEngine;

public class AttackState : State
{
    private TentacleProperties properties = null;
    private TentacleStateManager stateManager = null;

    private bool detachedFromAnchor;
    private float attackTime;
    private float wrapTime;

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
        wrapTime = 0.0f;
    }

    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(Game.instance.player.GetPosition());

        float currentTentacleDistance = controller.GetDistanceBetweenEndPointAndAnchor();
        attackTime += Time.deltaTime;

        if (currentTentacleDistance >= 4 && !detachedFromAnchor)
            detachedFromAnchor = true;
        else if (currentTentacleDistance < 1 && detachedFromAnchor || currentTentacleDistance >= properties.tentacleMaxLength)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        float playerDistFromTentacle = controller.GetDistanceBetweenPlayerAndEndPoint();

        if (playerDistFromTentacle <= properties.lightDistance)
        {
            //    stateManager.TransitionStates(TentacleStates.Scared);
        }

        if (playerDistFromTentacle <= 1f)
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

        controller.UpdateQueuedSegments();
    }

    public override void UpdateLateLogic()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions();
        controller.UpdateAgentTrackedPositions();
    }

}
