using UnityEngine;

public class State 
{
    public string name;

    protected TentacleController controller;
    protected Player player;

    protected State(string name, TentacleController controller)
    {
        this.name = name;
        this.controller = controller;
    }
    
    protected State(string name, Player player)
    {
        this.name = name;
        this.player = player;
    }

    public virtual void UpdateInput() { }
    public virtual void EnterState() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void UpdateLateLogic() { }
    public virtual void ExitState() { }
}
