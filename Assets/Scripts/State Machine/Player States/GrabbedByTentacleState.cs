using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedByTentacleState : State
{
    public GrabbedByTentacleState(Player player) : base("PlayerGrabbedByTentacle", player) => this.player = player;

    public override void UpdateLogic()
    {
        player.transform.position = Vector3.Lerp(player.transform.position, player.attackingTentacle.GetTentacleEndPoint(), Time.deltaTime * 8f);

        float dist = (player.transform.position - player.attackingTentacle.GetAnchorPosition()).magnitude;

        if(dist <= 1.0f)
        {
            player.stateManager.TransitionStates(PlayerStates.Dead);
        }
    }
}
