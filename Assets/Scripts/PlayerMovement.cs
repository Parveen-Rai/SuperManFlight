using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour , IMovementContext
{

    public CharacterController playerController;



    [Header("Ground")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 5f;
    public float gravity = 9.8f;
    private float turnSpeed = 10f;


    [Header("Air")]
    public float flyAcceleration = 60f;
    public float flyMaxSpeed = 70f;
    public float boostMultiplier = 2f;
    public float fallGravity = 0.5f;
    public float raiseSpeed = 10f;



    [Header("CustomPhysics")]
    public float accelaration = 25f;
    public float decelaration = 30f;
    public float airControlMultiplier = 0.5f;


    [Header("Input Settings")]
    public PlayerAdapter input;



    [Header("Player State Report")]
    public TextMeshProUGUI rootStateText;
    public TextMeshProUGUI childStateText;
    public TextMeshProUGUI MomentumText;



    public Vector2 MoveInput => input.Move;

    public Vector2 LookInput => input.Look;
    public bool JumpPressed => input.JumpPressed;
    public bool BoostHeld => input.BoostHeld;

    public bool FlyPressed => input.FlyPressed;

    public CharacterController Controller => playerController;

    private Vector3 horizontalVelocity;
    private float verticalVelocity = 0;


    // FSMs
    StateMachine rootSM = new StateMachine();
    StateMachine groundSM = new StateMachine();
    StateMachine airborneSM = new StateMachine();

    GroundRootState groundRoot;
    AirborneRootState airborneRoot;

    Dictionary<string, MovementState> groundStates = new Dictionary<string, MovementState>();
    Dictionary<string, MovementState> airborneStates = new Dictionary<string, MovementState>();

    private void Awake()
    {
        input = GetComponent<PlayerAdapter>();

        // root states
        groundRoot = new GroundRootState(this);
        airborneRoot = new AirborneRootState(this);

        // ground substates
        groundStates[PlayerStateId.Idle] = new Idle(this);
        groundStates[PlayerStateId.Run] = new Run(this);
        groundStates[PlayerStateId.Sprint] = new Sprint(this);
        groundStates[PlayerStateId.Jump] = new Jump(this);

        // airborne substates
        airborneStates[PlayerStateId.Fall] = new Fall(this);
        airborneStates[PlayerStateId.Fly] = new Fly(this);
        airborneStates[PlayerStateId.Boost] = new Boost(this);
        airborneStates[PlayerStateId.Hover] = new Hover(this);
    }

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        rootSM.Initialize(groundRoot);
        groundSM.Initialize(groundStates[PlayerStateId.Idle]);
    }

    public bool IsGrounded()
    {
         return playerController.isGrounded;
    }

    void Update()
    {
        rootSM.currentState?.Update();

        if (rootSM.currentState == groundRoot)
        {
            groundSM.currentState?.Update();
        }
        else if (rootSM.currentState == airborneRoot)
        {
            airborneSM.currentState?.Update();
        }
    }

    private void FixedUpdate()
    {
        rootSM.currentState?.FixedUpdate();
    }

    public void GroundMove()
    {
        float x = MoveInput.x;
        float z = MoveInput.y;

        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 inputDir = forward * z + right * x;
        inputDir.Normalize();

        float currentSpeed = Mathf.Lerp(walkSpeed, runSpeed, BoostHeld ? 1f : 0f);

        Vector3 targetHorizontalVelocity = inputDir * currentSpeed;

        bool hasInput = inputDir.sqrMagnitude > 0.001f;

        float accel = hasInput ? accelaration : decelaration;

        // Smooth acceleration/deceleration
        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetHorizontalVelocity,
            accel * Time.deltaTime
        );

        Vector3 finalVelocity = horizontalVelocity;
        finalVelocity.y = GetVerticalVelocity();
        // Debug.Log("Final Velocity : " + finalVelocity);
        playerController.Move(finalVelocity * Time.deltaTime);

        // Rotate toward movement direction
        if (horizontalVelocity.sqrMagnitude > 0.001f)
        {
            Vector3 viewVector = new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(viewVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        MomentumText.text = "Momentum : " + horizontalVelocity.magnitude.ToString("F2");
    }


    public void FlyMove()
    {
        SetVerticalVelocity();
        float x = MoveInput.x;
        float z = MoveInput.y;
        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 inputDir = forward * z + right * x;
        inputDir.Normalize();

        float currentSpeed = Mathf.Lerp(walkSpeed, runSpeed, BoostHeld ? 1f : 0f);

        Vector3 targetHorizontalVelocity = inputDir * currentSpeed;

        bool hasInput = inputDir.sqrMagnitude > 0.001f;

        float accel = hasInput ? accelaration : decelaration;

        // Smooth acceleration/deceleration
        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetHorizontalVelocity,
            accel * Time.deltaTime
        );

        Vector3 finalVelocity = horizontalVelocity;
        finalVelocity.y = verticalVelocity;

        playerController.Move(finalVelocity * Time.deltaTime);
        // Rotate toward movement direction
        if (horizontalVelocity.sqrMagnitude > 0.001f)
        {
            Vector3 viewVector = new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(viewVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        MomentumText.text = "Momentum : " + horizontalVelocity.magnitude.ToString("F2");
    }

    public void Jump()
    {
       verticalVelocity = Mathf.Sqrt(jumpHeight*gravity*2);
    }

    public void ApplyCustomGravityGround()
    {
       
    }

    public void ApplyCustiomGravityAirborne()
    {
        //verticalVelocity.y -= fallGravity * Time.deltaTime;
    }

    float GetVerticalVelocity()
    {
        if (playerController.isGrounded)
        {
           verticalVelocity = -1f;
            if (JumpPressed)
            {
                Jump();
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    public void PrintRootState(string State)
    {
        rootStateText.text = "Root State : "+State;
    }

    public void SwitchToGrounded()
    {
        rootSM.ChangeState(groundRoot);
        SwitchGroundSubState(PlayerStateId.Idle);
    }

    public void SwitchToAirborne()
    {
        rootSM.ChangeState(airborneRoot);
        SwitchAirborneSubState(PlayerStateId.Hover);
    }

    public void SwitchGroundSubState(string id)
    {
        groundSM.ChangeState(groundStates[id]);
    }

    public void SwitchAirborneSubState(string id)
    {
        airborneSM.ChangeState(airborneStates[id]);
    }

    public void PrintChildState(string stateName)
    {
       childStateText.text = "Sub State : " + stateName;
    }

    public float getMomentum()
    {
        return horizontalVelocity.magnitude;
    }

    public void SetVerticalVelocity()
    {
        float axis = input.VerticalMove;
        float _gravity =  axis < 0 ? fallGravity : 1;
        verticalVelocity = axis * raiseSpeed * _gravity;
    }

    public float getVerticalVelocity()
    {
        return verticalVelocity;
    }
}
