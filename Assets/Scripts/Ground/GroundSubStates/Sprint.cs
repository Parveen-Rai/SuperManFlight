using UnityEngine;

public class Sprint : MovementState
{

    public Sprint(IMovementContext context) : base(context)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ctx.PrintChildState("Sprint");
    }

    override public void Update()
    {
        switch (ctx.getMomentum())
        {
            case float n when (n <  6f):
                ctx.SwitchGroundSubState(PlayerStateId.Run);
                return;
        }
    }
}
