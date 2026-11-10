using UnityEngine;

public abstract class MovementState
{
    protected readonly IMovementContext ctx;

    protected MovementState(IMovementContext context)
    {
        ctx = context;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}
