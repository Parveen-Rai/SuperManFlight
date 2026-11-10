using UnityEngine;

public class Run : MovementState
{

    public Run(IMovementContext context) : base(context)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ctx.PrintChildState("Run");
    }

    override public void Update()
    {
        switch (ctx.getMomentum())
        {
            case float n when (n <= 0.1f):
                ctx.SwitchGroundSubState(PlayerStateId.Idle);
                return;
            case float n when (n > 6f):
                ctx.SwitchGroundSubState(PlayerStateId.Sprint);
                return;
        }
    }
}
