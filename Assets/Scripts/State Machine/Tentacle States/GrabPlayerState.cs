using UnityEngine;

public class GrabPlayerState : State
{
    public GrabPlayerState(TentacleController controller) : base("GrabPlayer", controller) => this.controller = controller;

    public override void EnterState()
    {
        GameEvents.OnPlayerGetGrabbed.Invoke(controller);
    }

    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(controller.GetAnchorPosition());

        float dist = controller.GetDistanceBetweenPlayerAndEndPoint();

        if (dist > 5f)
            controller.stateManager.TransitionStates(TentacleStates.Retreat);

        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions();
        controller.UpdateQueuedSegments();
    }

    public override void UpdateLateLogic()
    {
        controller.UpdateQueuedSegments();
    }
}
