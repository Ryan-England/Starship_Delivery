using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    private bool is2D;
    public bool IsMovingCheck = false;

    [Header("Sprint")]
    public float sprintSpeedMultiplier = 1.5f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    private bool isSprinting = false;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public int maxJumps = 2;  // Maximum number of jumps allowed (2 for double jump)
    private int jumpsRemaining;  // Current jumps remaining
    bool readyToJump;
    private float jumpBoost = 1.0f;
    public bool IsJumpingCheck = false;
    public float doubleJumpMultiplier = 0.8f;  // How strong the second jump is compared to the first

    [Header("Crouch")]
    public float crouchSpeed = 5f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 10f;
    private bool isCrouching = false;
    private Vector3 originalScale;
    private bool readyToCrouch = true;
    public float crouchCooldown = 0.2f;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Respawn")]
    public GameObject SpawnPoint;
    private Vector3 respawnLocation;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    Transform tf;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        rb.freezeRotation = true;
        originalScale = transform.localScale;

        if (SpawnPoint != null)
        {
            respawnLocation = SpawnPoint.transform.position;
        }
        else 
        { 
            respawnLocation = transform.position;
        }

        ResetJump();
        ResetCrouch();
        grounded = true;
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        // Handle crouch transition
        HandleCrouchAnimation();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (transform.position.y < -100)
        {
            respawn();
        }
    }

    private void MyInput()
    {
        if (is2D)
        {
            verticalInput = 0;
        }
        else
        {
            verticalInput = Input.GetAxisRaw("Vertical");
            if (IsJumpingCheck) Debug.Log(verticalInput);
        }

        // Disable horizontal movement when minigame is active
        if (GameController.isMinigameActive)
        {
            horizontalInput = 0; // Prevent A/D movement
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }

        if (IsMovingCheck) Debug.Log(horizontalInput);

        // Sprint input
        isSprinting = Input.GetKey(sprintKey) && grounded && !isCrouching;

        // Jump input
        if (Input.GetKeyDown(jumpKey) && readyToJump && jumpsRemaining > 0)
        {
            readyToJump = false;

            Jump();
            if (!GameController.isMinigameActive)
            {
                Jump();
            }

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Crouch input
        if (Input.GetKeyDown(crouchKey) && readyToCrouch && grounded)
        {
            readyToCrouch = false;
            Crouch();
            Invoke(nameof(ResetCrouch), crouchCooldown);
        }

        // Stand up when key is released
        if (Input.GetKeyUp(crouchKey) && grounded)
        {
            StopCrouch();
        }
    }

    private void MovePlayer()
    {
        // Skip all movement when the minigame is active
        if (GameController.isMinigameActive)
        {
            return;
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentMoveSpeed = moveSpeed;
        if (isCrouching)
        {
            currentMoveSpeed = crouchSpeed;
        }
        else if (isSprinting)
        {
            currentMoveSpeed = moveSpeed * sprintSpeedMultiplier;
        }

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        isSprinting = false;
    }

    private void StopCrouch()
    {
        // Check if there's enough space to stand up
        if (!Physics.Raycast(transform.position, Vector3.up, standingHeight))
        {
            isCrouching = false;
        }
    }

    private void HandleCrouchAnimation()
    {
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * (targetHeight / standingHeight), originalScale.z);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * crouchTransitionSpeed);
    }

    private void ResetCrouch()
    {
        readyToCrouch = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        float maxSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
        if(flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        jumpsRemaining--;

        // reset y velocity (only on first jump)
        if (grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

        // Apply different force for double jump
        float currentJumpForce = grounded ? jumpForce : jumpForce * doubleJumpMultiplier;
        
        rb.AddForce(transform.up * currentJumpForce * jumpBoost, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!grounded && collision.contacts[0].normal.y > 0.7f)  // Check if we're landing on top of something
        {
            grounded = true;
            jumpsRemaining = maxJumps;  // Reset available jumps when landing
        }

        if (collision.transform.tag == "Platform")
        {
            transform.parent = collision.transform.parent;
            PlatformManager P = collision.gameObject.GetComponentInParent<PlatformManager>();

            if (P.bounciness > 0)
            {
                jumpBoost = 1.0f + P.bounciness;
                float bv = 0;
                if (collision.relativeVelocity.y > 2)
                {
                    bv += P.bounciness * collision.relativeVelocity.y;
                }
                rb.AddForce(transform.up * bv, ForceMode.Impulse);
            }

            if (P.lethal)
            {
                respawn();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        transform.parent = null;
        jumpBoost = 1.0f;
    }

    public void changeControls(bool value)
    {
        is2D = value;
    }

    public void resetOrientation()
    {
        orientation.eulerAngles = new Vector3(0f, 0f, 0f);
        tf.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    public void respawn()
    {
        transform.position = respawnLocation;
        resetOrientation();
        rb.velocity = Vector3.zero;
        isCrouching = false;
        isSprinting = false;
        jumpsRemaining = maxJumps;
        transform.localScale = originalScale;
    }
}