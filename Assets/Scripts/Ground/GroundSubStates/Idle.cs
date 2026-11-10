using UnityEngine;

public class Idle : MovementState
{
    public Idle(IMovementContext context) : base(context)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ctx.PrintChildState("Idle");
    }

    override public void Update()
    {
        if (ctx.getMomentum() > 0.1f)
        {
            ctx.SwitchGroundSubState(PlayerStateId.Run);
            return;
        }
    }
}
