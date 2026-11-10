using UnityEngine;

public class Hover : MovementState
{
    public Hover(IMovementContext context) : base(context)
    {
    }

    override public void Enter()
    {
        base.Enter();
        ctx.PrintChildState("Hover");
    }

    public override void Update()
    {
        base.Update();
        if(ctx.getMomentum() > 0f)
        {
            ctx.SwitchAirborneSubState(PlayerStateId.Fly);
        }
    }
}
