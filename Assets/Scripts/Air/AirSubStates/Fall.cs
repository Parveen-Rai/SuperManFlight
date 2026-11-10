using UnityEngine;

public class Fall : MovementState
{
    public Fall(IMovementContext context) : base(context)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ctx.PrintChildState("Fall");
    }

    public override void Update()
    {
        base.Update();
        
        if (ctx.IsGrounded())
        {
            ctx.SwitchToGrounded();
            return;
        }
        if(ctx.getVerticalVelocity() >= 0)
        {
            ctx.SwitchAirborneSubState(PlayerStateId.Hover);
        }
    }
}
