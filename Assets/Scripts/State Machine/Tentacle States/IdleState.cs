using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(TentacleController controller, Player player) : base("Idle", controller)
    {
        this.player = player;
        this.controller = controller;
    }

    public override void EnterState()
    {
        controller.InitializeTentacles();
    }

    public override void UpdateLogic()
    {
        controller.SetAgentPosition(Vector3.zero);
    }

}
