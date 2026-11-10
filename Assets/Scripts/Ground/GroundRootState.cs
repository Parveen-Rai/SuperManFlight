using UnityEngine;

public class GroundRootState : MovementState
{
    public GroundRootState(IMovementContext context) : base(context) { }


    public override void Enter()
    {
        base.Enter();
        ctx.PrintRootState("Ground");
    }

    public override void Update()
    {
        if (!ctx.IsGrounded() && ctx.FlyPressed)
        {
            ctx.SwitchToAirborne();
            return;
        }
        else if (!ctx.IsGrounded())
        {
            ctx.SwitchGroundSubState(PlayerStateId.Jump);
        }
        ctx.GroundMove();
    }
}
