using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : State
{
    public PlayerDeadState(Player player) : base("PlayerDead", player) => this.player = player;

    public override void EnterState()
    {
        Debug.Log("Player Dead");
    }
}
