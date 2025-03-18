using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Member Variables
    [Header("Movement")]
    [Tooltip("Speed at which the player moves.")]
    public float moveSpeed = 4f;
    [Tooltip("Amount of drag applied when on the ground.")]
    public float groundDrag = 6f;
    private bool is2D; // If you want 2D movement
    private Vector3 moveDirection; // Direction of movement

    [Header("Sprint")]
    [Tooltip("Multiplier applied to movement speed while sprinting.")]
    public float sprintSpeedMultiplier = 1.5f;
    private bool isSprinting = false; // Checks if you are sprinting for sliding methods

    [Header("Jump")]
    [Tooltip("Force applied when jumping.")]
    public float jumpForce = 7f;
    private float jumpMultiplier = 1f; // multiplier for the jump force
    [Tooltip("Cooldown time between jumps.")]
    public float jumpCooldown = 0f;
    [Tooltip("Multiplier applied to movement speed while in the air.")]
    public float airMultiplier = 5f;
    [Tooltip("Maximum number of jumps the player can perform before touching the ground.")]
    public int maxJumps = 2;  
    [Tooltip("Multiplier of force applied to the second jump.")]
    public float doubleJumpMultiplier = 0.8f;  
    private int jumpsRemaining; // Counter for remaining jumps
    private bool readyToJump; // Checks if the player is able to jump
    
    [Header("Jetpack")]
    [Tooltip("Checks if the player has a jetpack active.")]
    public bool jetpackActive = false;
    [Tooltip("Total amount of jetfuel that the player is allowed to have.")]
    public float jetfuel = 15f;
    private float jetMaximum;
    [Tooltip("The change in fuel burn rate and fuel recharge rate combined.")]
    public float fuelRate = 5f;
    [Tooltip("The boost in vertical velocity over time when using the jetpack.")]
    public float jetBoost = 0.15f;
    private float minRotation = -120f; // minimum rotation of gauge dial
    private float maxRotation = 120f; // maximum rotation of gauge dial


    [Header("Climbing")]
    [Tooltip("Speed at which the player climbs surfaces.")]
    public float climbSpeed = 5f;
    [Tooltip("Distance to check for a climbable surface.")]
    public float climbCheckDistance = 0.5f;
    private bool isClimbing = false; // Checks if you are climbing

    [Header("Crouch")]
    [Tooltip("Speed of movement while crouching.")]
    public float crouchSpeed = 5f;
    [Tooltip("Height of the player while crouching.")]
    public float crouchHeight = 0.5f;
    [Tooltip("Height of the player while standing.")]
    public float standingHeight = 2f;
    [Tooltip("Speed of transition between crouching and standing.")]
    public float crouchTransitionSpeed = 10f;
    [Tooltip("Cooldown before the player can crouch again.")]
    public float crouchCooldown = 0.2f;
    private Vector3 originalScale; // Transform scale of the original player
    private bool readyToCrouch = true; // Checks if the player is able to crouch
    private bool isCrouching = false; // Checks if you are crouch

    [Header("Slide")]
    [Tooltip("Duration of a slide.")]
    public float slideDuration = 0.6f;
    [Tooltip("Multiplier applied to movement speed while sliding.")]
    public float slideSpeedMultiplier = 1.5f;
    [Tooltip("Force applied to initiate a slide.")]
    public float slideForce = 5f;
    private bool isSliding = false; // Checks if you are sliding
    private float slideTimer;
    

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.C;
    public KeyCode climbKey = KeyCode.F;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Respawn")]
    [Tooltip("Set a spawn point for the level, otherwise use current position if nothing is set.")]
    public GameObject SpawnPoint;
    private Vector3 respawnLocation;

    [Header("External References")]
    [Tooltip("Transform used for player orientation.")]
    public Transform orientation;
    private GameObject circleGauge; // gauge for jetpack
    private GameObject imageNeedle; // gauge needle for jetpack

    // Script references
    private float horizontalInput;
    private float verticalInput;
    private bool grounded; // Checks if you are grounded
    private Rigidbody rb;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        circleGauge = GameObject.Find("GaugeCircle");
        imageNeedle = GameObject.Find("GaugeNeedle");
        circleGauge.SetActive(false);
        imageNeedle.SetActive(false);
        jetMaximum = jetfuel;
        UpdateFuelGauge();
    }

    private void Update()
    {
        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
            if (jetpackActive) {
                RefillJet();
            }
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

    #region Player Input
    private void MyInput()
    {
        if (is2D)
        {
            verticalInput = 0;
        }
        else
        {
            verticalInput = Input.GetAxisRaw("Vertical");
        }

     
        if (GameController.isMinigameActive)
        {
            horizontalInput = 0; 
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }

       
        isSprinting = Input.GetKey(sprintKey) && grounded && !isCrouching;

        if(jetpackActive){
            circleGauge.SetActive(true);
            imageNeedle.SetActive(true);
            if (Input.GetKey(jumpKey) && jetfuel > 0)
            {
                readyToJump = false;

            
                if (!GameController.isMinigameActive)
                {
                    Jet();
                }

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
        else if(!jetpackActive){
            if (Input.GetKeyDown(jumpKey) && readyToJump && jumpsRemaining > 0)
            {
                readyToJump = false;

            
                if (!GameController.isMinigameActive)
                {
                    Jump();
                }

                Invoke(nameof(ResetJump), jumpCooldown);
            }            
        }

      
        if (Input.GetKeyDown(KeyCode.C) && readyToCrouch && grounded)
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

       
        if (Input.GetKeyUp(KeyCode.C) && grounded)
        {
            StopCrouch();
        }
    }
    #endregion

    #region Player Movement
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
    #endregion

    #region Crouching Functionality
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

    #endregion

    #region Climbing Functionality
    private void CheckForClimbableSurface()
    {
        RaycastHit hit;
        bool AttachedtoWall = Physics.Raycast(transform.position, orientation.forward, out hit, climbCheckDistance);
    
        if (AttachedtoWall)
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
    #endregion

    #region Sprinting Functionality
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
    #endregion

    #region Jump Logic
    private void Jump()
    {
        jumpsRemaining--;

 
        if (grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

      
        float currentJumpForce = grounded ? jumpForce : jumpForce * doubleJumpMultiplier;
        
        rb.AddForce(transform.up * currentJumpForce * jumpMultiplier, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    #endregion

    #region Jetpack Logic
    private void Jet()
    {
        if (jetfuel > 0)
        {
            rb.AddForce(transform.up * jumpForce / 10f * jetBoost, ForceMode.Impulse);
            jetfuel -= Time.deltaTime * fuelRate;
            UpdateFuelGauge();
        }
    }

    private void RefillJet() {
        if (jetfuel < jetMaximum)
        {
            jetfuel += fuelRate * Time.deltaTime;
            jetfuel = Mathf.Min(jetfuel, jetMaximum);
            Debug.Log(jetfuel);
            UpdateFuelGauge();
        }
    }

    private void UpdateFuelGauge() {
        float fuelPercentage = jetfuel / jetMaximum;
        float rotationAngle = Mathf.Lerp(minRotation, maxRotation, fuelPercentage);
        imageNeedle.transform.rotation = Quaternion.Euler(0, 0, -rotationAngle);
    }
    #endregion

    #region Sliding Functionality
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
    #endregion

    #region Collision Logic
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
        GameObject Other = collision.gameObject;
        if(Other.tag == "Platform")
        {
            PlatformManager Manager = Other.GetComponentInParent<PlatformManager>();
            if( Manager != null && Manager.lethal)
            {
                respawn();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        transform.parent = null;
        jumpMultiplier = 1.0f;
        if (isClimbing)
        {
            StopClimbing();
        }
    }
    #endregion

    #region Debug Functionality
        public void changeControls(bool value)
        {
            is2D = value;
        }

        public void resetOrientation()
        {
            orientation.eulerAngles = new Vector3(0f, 0f, 0f);
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    #endregion

    #region Respawn Functionality
    public void respawn()
    {
        transform.position = respawnLocation;
        transform.localScale = originalScale;
        rb.velocity = Vector3.zero;
        isCrouching = false;
        isSprinting = false;
        jumpsRemaining = maxJumps;
    }
    #endregion
}