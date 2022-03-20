using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    public PlayerIdleState(Player player) : base("PlayerIdle", player)
    {
        this.player = player;
    }
}
