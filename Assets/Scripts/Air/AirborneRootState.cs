using UnityEngine;

public class AirborneRootState : MovementState
{
    public AirborneRootState(IMovementContext context) : base(context) { }

    public override void Enter()
    {
        base.Enter();
        ctx.PrintRootState("Airborne");
    }

    public override void Update()
    {
        base.Update();

        if (ctx.IsGrounded())
        {
            ctx.SwitchToGrounded();
            return;
        }

        if (ctx.getMomentum() <= 0f && ctx.getVerticalVelocity() < 0)
        {
            ctx.SwitchAirborneSubState(PlayerStateId.Fall);
        }

        ctx.FlyMove();

        if (ctx.IsGrounded())
        {
            ctx.SwitchToGrounded();
        }
    }
}
