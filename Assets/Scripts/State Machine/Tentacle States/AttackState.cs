using UnityEngine;
using UnityEngine.InputSystem;

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


        if ((currentTentacleDistance < 1 && detachedFromAnchor) || currentTentacleDistance >= properties.tentacleMaxLength || attackTime >= 15f)
        {
            stateManager.TransitionStates(TentacleStates.Retreat);
        }

        float playerDistFromTentacle = controller.GetDistanceBetweenPlayerAndEndPoint();

        if (playerDistFromTentacle <= properties.lightDistance)
        {
            if (Game.instance.player.itemManagement.GetActiveController() != null)
            {
                bool canGetScared = Game.instance.player.itemManagement.GetActiveController().baseItem.item.toolType == ItemProperties.WeaponTypes.Flashlight;

                if (canGetScared)
                {
                    FlashlightController fCon = (FlashlightController)Game.instance.player.itemManagement.GetActiveController();

                    if (fCon.GetState() == FlashlightStates.On)
                    {
                        stateManager.TransitionStates(TentacleStates.Retreat);
                    }
                }
            }
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

        attackTime += Time.deltaTime;

        controller.UpdateAgentPosition(Game.instance.player.GetPosition());


        controller.UpdateSegmentCount();
        controller.UpdateSegmentPositions();
    }
    public override void UpdateLateLogic()
    {
        controller.UpdateQueuedSegments();
    }

    bool IsLookingAtLight(Vector3 targetPos, float FOVAngle)
    {
        // FOVAngle has to be less than 180
        float dot = Vector3.Dot(controller.GetTentacleEndPoint(), (targetPos - controller.GetTentacleEndPoint()).normalized); // credit to fafase for this

        float viewAngle = (1 - dot) * 90; // convert the dot product value into a 180 degree representation (or *180 if you don't divide by 2 earlier)

        if (viewAngle <= 90f)
            return true;
        else
            return false;
    }

}
