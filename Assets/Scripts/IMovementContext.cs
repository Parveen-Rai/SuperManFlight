using UnityEngine;
public interface IMovementContext
{

    CharacterController Controller { get; }

    // Input
    Vector2 MoveInput { get; }

    Vector2 LookInput { get; }
    bool JumpPressed { get; }
    bool BoostHeld { get; }

    bool FlyPressed { get; }

    // Sensors
    bool IsGrounded();


    float getMomentum();

    float getVerticalVelocity();

    //Movement Commands 
    void GroundMove();
    void FlyMove();
    void Jump();

    void ApplyCustomGravityGround();

    void ApplyCustiomGravityAirborne();


    // States switching
    void SwitchToGrounded();
    void SwitchToAirborne();

    void SwitchGroundSubState(string id);

    void SwitchAirborneSubState(string id);

    void PrintRootState(string stateName);

    void PrintChildState(string stateName);

}
