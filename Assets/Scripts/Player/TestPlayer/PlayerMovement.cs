using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform orientation;
    public Transform groundCheck;
    public Rigidbody rb;
    public float groundDrag;
    public float groundDistance = 0.4f;
    public float moveSpeed;
    public float jumpPower;

    public bool readyToJump;
    public bool grounded;

    private float hInput;
    private float vInput;

    private Vector3 moveDir;

    void Start()
    {
        rb.freezeRotation = true;
        rb.drag = groundDrag;

        readyToJump = true;
    }

    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance);

        Debug.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundDistance, groundCheck.position.z), Color.red);
        PlayerInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void PlayerInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            Jump();

            Invoke(nameof(ResetReadyToJump), 0.25f);
        }
    }

    void MovePlayer()
    {
        moveDir = orientation.forward * vInput + orientation.right * hInput;

        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void Jump()
    {
        readyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    void ResetReadyToJump()
    {
        readyToJump = true;
    }
}
