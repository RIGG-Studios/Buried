using UnityEngine;

public class GrabPlayerState : State
{
    public GrabPlayerState(TentacleController controller) : base("GrabPlayer", controller) => this.controller = controller;

    public override void EnterState()
    {
        GameEvents.OnPlayerGetGrabbed.Invoke(controller);

        controller.GetAgent().speed = 10f;
    }

    public override void ExitState()
    {
        controller.GetAgent().speed = 4.75f;
    }

    public override void UpdateLogic()
    {
        controller.UpdateAgentPosition(controller.GetAnchorPosition());

        float dist = controller.GetDistanceBetweenPlayerAndEndPoint();

        if (dist > 5f)
            controller.stateManager.TransitionStates(TentacleStates.Retreat);

        controller.UpdateSegmentPositions(controller.GetAgentPosition());
    }

    public override void UpdateLateLogic()
    {
     //   controller.UpdateQueuedSegments();
    }
}
