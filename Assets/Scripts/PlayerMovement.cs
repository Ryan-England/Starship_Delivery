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
    private bool isSprinting = false;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public int maxJumps = 2;  
    private int jumpsRemaining;  
    bool readyToJump;
    private float jumpBoost = 1.0f;
    public bool IsJumpingCheck = false;
    public float doubleJumpMultiplier = 0.8f;  

    [Header("Climbing")]
    public float climbSpeed = 5f;
    public float climbCheckDistance = 0.5f;
    private bool isClimbing = false;

    [Header("Crouch")]
    public float crouchSpeed = 5f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 10f;
    private bool isCrouching = false;
    private Vector3 originalScale;
    private bool readyToCrouch = true;
    public float crouchCooldown = 0.2f;

    [Header("Slide")]
    public float slideDuration = 0.6f;
    public float slideSpeedMultiplier = 1.5f;
    public float slideForce = 5f;
    private bool isSliding = false;
    private float slideTimer;

    [Header("Ground Check")]
    public float playerHeight;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.C;
    public KeyCode climbKey = KeyCode.F;
    public KeyCode sprintKey = KeyCode.LeftShift;

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

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }


        HandleCrouchAnimation();

     
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }

     
        CheckForClimbableSurface();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (transform.position.y < -100)
        {
            respawn();
        }

        Climb();
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

     
        if (GameController.isMinigameActive)
        {
            horizontalInput = 0; 
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }

        if (IsMovingCheck) Debug.Log(horizontalInput);

       
        isSprinting = Input.GetKey(sprintKey) && grounded && !isCrouching;

     
        if (Input.GetKeyDown(jumpKey) && readyToJump && jumpsRemaining > 0)
        {
            readyToJump = false;

         
            if (!GameController.isMinigameActive)
            {
                Jump();
            }

            Invoke(nameof(ResetJump), jumpCooldown);
        }

      
        if (Input.GetKeyDown(crouchKey) && readyToCrouch && grounded)
        {
            readyToCrouch = false;
            if (isSprinting)
            {
                StartSlide();
            }
            else
            {
                Crouch();
            }
            Invoke(nameof(ResetCrouch), crouchCooldown);
        }

       
        if (Input.GetKeyUp(crouchKey) && grounded)
        {
            StopCrouch();
        }
    }

    private void MovePlayer()
    {
    
        if (GameController.isMinigameActive || isClimbing)
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
        else if (isSliding)
        {
            currentMoveSpeed = moveSpeed * slideSpeedMultiplier;
        }

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);
        }


        if (isSliding)
        {
            rb.AddForce(orientation.forward * slideForce, ForceMode.Impulse);
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        isSprinting = false;
    }

    private void StopCrouch()
    {
       
        if (!Physics.Raycast(transform.position, Vector3.up, standingHeight))
        {
            isCrouching = false;
        }
    }

    private void HandleCrouchAnimation()
    {
        float targetHeight = isCrouching || isSliding ? crouchHeight : standingHeight;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * (targetHeight / standingHeight), originalScale.z);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * crouchTransitionSpeed);
    }

    private void ResetCrouch()
    {
        readyToCrouch = true;
    }

    private void CheckForClimbableSurface()
    {
        RaycastHit hit;
    
        if (Physics.Raycast(transform.position, orientation.forward, out hit, climbCheckDistance))
        {
            if (Input.GetKeyDown(climbKey))
            {
                StartClimbing();
            }
        }
        else if (isClimbing)
        {
            StopClimbing();
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero; 
    }

    private void StopClimbing()
    {
        isClimbing = false;
        rb.useGravity = true;
    }

    private void Climb()
    {
        if (isClimbing)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            Vector3 climbDirection = orientation.up * verticalInput + orientation.right * horizontalInput;
            rb.velocity = climbDirection.normalized * climbSpeed;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        float maxSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
        if (isSliding)
        {
            rb.velocity = new Vector3(rb.velocity.x * 0.95f, rb.velocity.y, rb.velocity.z * 0.95f); 
        }

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        jumpsRemaining--;

 
        if (grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

      
        float currentJumpForce = grounded ? jumpForce : jumpForce * doubleJumpMultiplier;
        
        rb.AddForce(transform.up * currentJumpForce * jumpBoost, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StartSlide()
    {
        isSliding = true;
        isCrouching = true;
        isSprinting = false;
        slideTimer = slideDuration;
    }

    private void StopSlide()
    {
        isSliding = false;
        isCrouching = false;
        rb.velocity = new Vector3(rb.velocity.x * 0.5f, rb.velocity.y, rb.velocity.z * 0.5f); 
    }


   private void OnCollisionEnter(Collision collision)
    {
        if (!grounded && collision.contacts[0].normal.y > 0.7f) 
        {
            grounded = true;
            jumpsRemaining = maxJumps;  
            StopClimbing();
        }

        if (isClimbing)
        {
            rb.useGravity = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        transform.parent = null;
        jumpBoost = 1.0f;
        if (isClimbing)
        {
            StopClimbing();
        }
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