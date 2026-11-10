using UnityEngine;

public class Fly : MovementState
{
    public Fly(IMovementContext context) : base(context)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ctx.PrintChildState("Fly");
    }

    public override void Update()
    {
        base.Update();
        if (ctx.getMomentum() <= 0f)
        {
            ctx.SwitchAirborneSubState(PlayerStateId.Hover);
            return;
        }
    }
}
