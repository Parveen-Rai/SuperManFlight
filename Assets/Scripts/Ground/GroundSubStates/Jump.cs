using UnityEngine;

public class Jump : MovementState
{
    public Jump(IMovementContext context) : base(context)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ctx.PrintChildState("Jump");
    }

    override public void Update()
    {
        if (ctx.IsGrounded())
        {
            ctx.SwitchGroundSubState(PlayerStateId.Idle);
            return;
        }
    }
}
