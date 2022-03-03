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

        controller.UpdateQueuedSegments();
    }

    public override void UpdateLateLogic()
    {
        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions();
    }
}
