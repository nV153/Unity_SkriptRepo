using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player movement controller handling walking, sprinting, crouching, jumping, dashing, and knockback.
/// Uses Rigidbody-based physics movement with speed control and state management.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    public bool grounded;

    [Header("Sprint Variables")]
    public float maxSprint;
    public float sprintCooldown;
    public Image sprintbar;
    private float sprintRemain;
    private float timeSinceLastSprint;

    [Header("Dashing")]
    public KeyCode dashKey = KeyCode.E;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool readyToDash = true;
    private Vector3 dashDirection;
    private float dashTimer;
    private bool isDashing = false;

    public Transform orientation;

    private float verticalInput;
    private float horizontalInput;

    private Vector3 knockbackVelocity;
    private float knockbackTimer;

    private Vector3 moveDirection;
    private Rigidbody rb;

    public MovementState state;

    /// <summary>
    /// Enumeration for the player’s movement state.
    /// </summary>
    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Prevent rigidbody from rotating due to physics

        ResetJump();

        sprintRemain = maxSprint;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // Check if player is grounded using a raycast
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, playerHeight * 0.6f, WhatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // Set drag depending on grounded state
        rb.drag = grounded ? groundDrag : 0;

        // Sprint stamina regeneration and consumption
        if (state != MovementState.sprinting)
        {
            if (timeSinceLastSprint > sprintCooldown)
            {
                sprintRemain += Time.deltaTime;
            }
            else
            {
                timeSinceLastSprint += Time.deltaTime;
            }
        }
        else
        {
            sprintRemain -= Time.deltaTime;
            timeSinceLastSprint = 0f;
        }
        sprintbar.fillAmount = sprintRemain / maxSprint;

        // Start dash when key pressed and player has input and dash is ready
        if (Input.GetKeyDown(dashKey) && readyToDash && (horizontalInput != 0 || verticalInput != 0))
        {
            readyToDash = false;
            isDashing = true;
            dashTimer = dashDuration;
            dashDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
            Invoke(nameof(ResetDash), dashCooldown);
        }
    }

    private void FixedUpdate()
    {
        // Handle dash movement
        if (isDashing)
        {
            rb.velocity = dashDirection * dashForce;
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
            return;
        }
        // Handle knockback movement
        else if (knockbackTimer > 0f)
        {
            rb.velocity = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        MovePlayer();
    }

    /// <summary>
    /// Handles player input for movement, jump, and crouch.
    /// </summary>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump if conditions met
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 3, ForceMode.Impulse);
        }
        // Stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    /// <summary>
    /// Determines and updates current movement state and speed.
    /// </summary>
    private void StateHandler()
    {
        if (grounded && Input.GetKey(sprintKey) && sprintRemain > 0)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded && Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            moveSpeed = walkSpeed;

            // Decrease sprint stamina in air if sprinting
            if (Input.GetKey(sprintKey) && sprintRemain > 0)
            {
                sprintRemain -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Applies movement forces to the player based on inputs and state.
    /// </summary>
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    /// <summary>
    /// Limits the player's velocity to the current movement speed.
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Makes the player jump by applying upward impulse.
    /// </summary>
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Resets jump readiness after cooldown.
    /// </summary>
    private void ResetJump()
    {
        readyToJump = true;
    }

    /// <summary>
    /// Resets dash readiness after cooldown.
    /// </summary>
    private void ResetDash()
    {
        readyToDash = true;
    }

    /// <summary>
    /// Applies knockback force and duration to the player.
    /// </summary>
    /// <param name="force">Knockback force vector.</param>
    /// <param name="duration">Duration for knockback movement.</param>
    public void ApplyKnockback(Vector3 force, float duration)
    {
        knockbackVelocity = force;
        knockbackTimer = duration;
    }
}
