using UnityEngine;

public class State
{
    public string name;
    protected TentacleController controller;

    protected State(string name, TentacleController controller)
    {
        this.name = name;
        this.controller = controller;
    }

    public virtual void EnterState(TentacleController manager) { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void UpdateLateLogic() { }
    public virtual void ExitState() { }
}
