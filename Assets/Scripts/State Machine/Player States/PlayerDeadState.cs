using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadState : State
{
    public PlayerDeadState(Player player) : base("PlayerDead", player) => this.player = player;

    public override void EnterState()
    {
        SceneManager.LoadScene(1);
    }
}
