using UnityEngine;
using UnityEngine.InputSystem;

public class State
{
    public string name;

    protected TentacleController controller;
    protected Player player;

    public InputAction moveAction;
    public InputAction lookAction;

    protected State(string name, TentacleController controller)
    {
        this.name = name;
        this.controller = controller;
    }
    
    protected State(string name, Player player)
    {
        this.name = name;
        this.player = player;

        moveAction = player.GetComponent<PlayerInput>().actions["Move"];
        lookAction = player.GetComponent<PlayerInput>().actions["Look"];
    }

    public virtual void EnterState() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void UpdateLateLogic() { }
    public virtual void ExitState() { }
}
