using UnityEngine;

public class StateMachine
{
    public MovementState currentState {  get; private set; }

    public void Initialize(MovementState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }
    
    public void ChangeState(MovementState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
