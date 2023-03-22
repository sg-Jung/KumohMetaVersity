using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float climbSpeed;
    public float wallrunSpeed;
    public float slideSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public bool crouched;
    [HideInInspector]
    public float startYScale;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    //[Header("References")]
    //public Climbing climbingScript;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [HideInInspector]
    public Rigidbody rb;

    public bool climbing;
    public bool WallRunning;
    public bool sliding;
    public bool freeze;

    private FSM fsm;

    public MovementState state;

    public enum MovementState
    {
        freeze,
        walking,
        sprinting,
        slliding,
        crouching,
        air
    }

    private void Awake()
    {
        fsm = new FSM();

        fsm.AddState("Freeze", new FreezeState(this));
        fsm.AddState("Walk", new WalkState(this));
        fsm.AddState("Sprint", new SprintState(this));
        fsm.AddState("Jump", new JumpState(this));
        fsm.AddState("Crouch", new CrouchState(this));
        fsm.AddState("Air", new AirState(this));
        fsm.AddState("Silde", new SlideState(this));

        fsm.SetState("Walk");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);
        // grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        //MyInput();
        SpeedControl();
        StateHandler();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void MyInput()
    {
       

        /*// start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            *//*crouched = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);*//*

        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            crouched = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }*/
    }

    void StateHandler()
    {
        // Input - WASD
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
        }

        /*// Mode - Sliding     // 아직 사용할지 몰라 미구현
        if (sliding)
        {
            state = MovementState.slliding;

            // increase speed by one every second
            if (OnSlope() && rb.velocity.y < 0.1f)
                moveSpeed = slideSpeed;

            fsm.SetState("Slide");
        }*/

        // Mode - Jumping
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Mode - Crouching
        if (Input.GetKeyDown(crouchKey) && !crouched)
        {
            fsm.SetState("Crouch");
        }
        else if(Input.GetKeyUp(crouchKey) && crouched)
        {
            fsm.SetState("Walk");
        }

        // Mode - Sprinting
        if (Input.GetKey(sprintKey) && grounded && !crouched)
        {
            fsm.SetState("Sprint");
        }

        // Mode - Walking
        else if (grounded && !crouched)
        {
            fsm.SetState("Walk");
        }

        // Mode - Air
        else if (!readyToJump)
        {
            fsm.SetState("Air");
        }

    }

    void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump()
    {
        readyToJump = false;
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

}