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

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    private float jumpBoost = 1.0f;
    public bool IsJumpingCheck = false;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    //public KeyCode jumpKey = KeyCode.Space;

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

        if (SpawnPoint != null)
        {
            respawnLocation = SpawnPoint.transform.position;
        }
        else { 
            respawnLocation = transform.position;
            
        }

        ResetJump();
        grounded = true;
    }

    private void Update()
    {
        // ground check [DEPRECATED: grounded is now updated on collision enter/exit]
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

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
    }

    private void FixedUpdate()
    {
     
        MovePlayer();


        //Check respawn
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
            if (IsJumpingCheck)
            {
                Debug.Log(verticalInput);
            }
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

        if (IsMovingCheck)
        {
            Debug.Log(horizontalInput);
        }

        // Jump input (already controlled with isMinigameActive)
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            if (!GameController.isMinigameActive)
            {
                Jump();
            }

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }


    private void MovePlayer()
    {
        // Skip all movement when the minigame is active
        if (GameController.isMinigameActive)
        {
            return;
        }

        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * jumpBoost, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    //On collision, ground the player and check for properties of the platform
    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;

        //PlatformManager P = collision.gameObject.GetComponent<PlatformManager>();
        if (collision.transform.tag == "Platform")
        {
            transform.parent = collision.transform.parent; //sets player as child of the platform in order to support moving platforms
            PlatformManager P = collision.gameObject.GetComponentInParent<PlatformManager>();

            if (P.bounciness > 0) //bounce handling
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

    //Upon leaving a surface, set player to ungrounded and unparent
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
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
