using UnityEngine;

public class PlayerAdapter : MonoBehaviour
{
    PlayerInput controls;

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool JumpPressed { get; private set; }

    public bool BoostHeld { get; private set; }

    public bool FlyPressed { get; private set; }

    public float VerticalMove { get; private set; }


    private void Awake()
    {
        controls = new PlayerInput();
        controls.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => Move = Vector2.zero;

        controls.Player.Look.performed += ctx => Look = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => Look = Vector2.zero;

        controls.Player.Jump.performed += ctx => JumpPressed = true;
        controls.Player.Jump.canceled +=ctx=> JumpPressed = false;

        controls.Player.Sprint.performed += ctx => BoostHeld = true;
        controls.Player.Sprint.canceled += ctx => BoostHeld = false;

        controls.Player.TakeOff.performed += ctx => FlyPressed = true;
        controls.Player.TakeOff.canceled += ctx => FlyPressed = false;

        controls.Player.VerticalMove.performed += ctx => VerticalMove = ctx.ReadValue<float>();
        controls.Player.VerticalMove.canceled += ctx => VerticalMove = 0;

    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void LateUpdate()
    {
        JumpPressed = false;
        FlyPressed = false;
    }
}
