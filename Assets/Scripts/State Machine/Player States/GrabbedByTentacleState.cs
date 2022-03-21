using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedByTentacleState : State
{
    public GrabbedByTentacleState(Player player) : base("PlayerGrabbedByTentacle", player) => this.player = player;

    public override void EnterState()
    {
        GameEvents.OnDropPlayer += DropPlayer;
    }

    public override void ExitState()
    {
        GameEvents.OnDropPlayer -= DropPlayer;
    }

    private void DropPlayer()
    {
        player.stateManager.TransitionStates(PlayerStates.Movement);
    }

    public override void UpdateLogic()
    {
        player.transform.position = player.attackingTentacle.GetTentacleEndPoint();

        float dist = (player.transform.position - player.attackingTentacle.GetAnchorPosition()).magnitude;

        if(dist <= 1.0f)
        {
            player.stateManager.TransitionStates(PlayerStates.Dead);
        }
    }
}
