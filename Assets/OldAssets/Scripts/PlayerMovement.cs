// using System;
// using Unity.VisualScripting;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Movement")]
//     public float moveSpeed;
//     public float groundDrag;
//     private bool is2D;
//     public bool IsMovingCheck = false;

//     [Header("Jump")]
//     public float jumpForce;
//     public float jumpCooldown;
//     public float airMultiplier;
//     bool readyToJump;
//     private float jumpBoost = 1.0f;
//     public bool IsJumpingCheck = false;

//     [Header("Crouch")]
//     public float crouchSpeed = 5f; // Speed while crouching
//     public float crouchHeight = 0.5f; // Height when crouched
//     public float standingHeight = 2f; // Normal height when standing
//     public float crouchTransitionSpeed = 10f; // How fast to transition to crouch
//     private bool isCrouching = false;
//     private Vector3 originalScale;
//     private bool readyToCrouch = true;
//     public float crouchCooldown = 0.2f;

//     [Header("Ground Check")]
//     public float playerHeight;
//     public LayerMask whatIsGround;
//     bool grounded;

//     [Header("Keybinds")]
//     public KeyCode jumpKey = KeyCode.Space;
//     public KeyCode crouchKey = KeyCode.LeftControl;

//     [Header("Respawn")]
//     public GameObject SpawnPoint;
//     private Vector3 respawnLocation;

//     public Transform orientation;

//     float horizontalInput;
//     float verticalInput;

//     Vector3 moveDirection;

//     Rigidbody rb;
//     Transform tf;

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         tf = GetComponent<Transform>();
//         rb.freezeRotation = true;
//         originalScale = transform.localScale;

//         if (SpawnPoint != null)
//         {
//             respawnLocation = SpawnPoint.transform.position;
//         }
//         else 
//         { 
//             respawnLocation = transform.position;
//         }

//         ResetJump();
//         ResetCrouch();
//         grounded = true;
//     }

//     private void Update()
//     {
//         MyInput();
//         SpeedControl();

//         // handle drag
//         if (grounded)
//         {
//             rb.drag = groundDrag;
//         }
//         else
//         {
//             rb.drag = 0;
//         }

//         // Handle crouch transition
//         HandleCrouchAnimation();
//     }

//     private void FixedUpdate()
//     {
//         MovePlayer();

//         if (transform.position.y < -100)
//         {
//             respawn();
//         }
//     }

//     private void MyInput()
//     {
//         if (is2D)
//         {
//             verticalInput = 0;
//         }
//         else
//         {
//             verticalInput = Input.GetAxisRaw("Vertical");
//             if (IsJumpingCheck) Debug.Log(verticalInput);
//         }

//         horizontalInput = Input.GetAxisRaw("Horizontal");
//         if (IsMovingCheck) Debug.Log(horizontalInput);

//         // Jump input
//         if (Input.GetKey(jumpKey) && readyToJump && grounded)
//         {
//             readyToJump = false;
//             Jump();
//             Invoke(nameof(ResetJump), jumpCooldown);
//         }

//         // Crouch input
//         if (Input.GetKeyDown(crouchKey) && readyToCrouch && grounded)
//         {
//             readyToCrouch = false;
//             Crouch();
//             Invoke(nameof(ResetCrouch), crouchCooldown);
//         }

//         // Stand up when key is released
//         if (Input.GetKeyUp(crouchKey) && grounded)
//         {
//             StopCrouch();
//         }
//     }

//     private void MovePlayer()
//     {
//         moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

//         float currentMoveSpeed = isCrouching ? crouchSpeed : moveSpeed;

//         if (grounded)
//         {
//             rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
//         }
//         else
//         {
//             rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);
//         }
//     }

//     private void Crouch()
//     {
//         isCrouching = true;
//     }

//     private void StopCrouch()
//     {
//         // Check if there's enough space to stand up
//         if (!Physics.Raycast(transform.position, Vector3.up, standingHeight))
//         {
//             isCrouching = false;
//         }
//     }

//     private void HandleCrouchAnimation()
//     {
//         float targetHeight = isCrouching ? crouchHeight : standingHeight;
//         Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * (targetHeight / standingHeight), originalScale.z);
//         transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * crouchTransitionSpeed);
//     }

//     private void ResetCrouch()
//     {
//         readyToCrouch = true;
//     }

//     private void SpeedControl()
//     {
//         Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

//         // limit velocity if needed
//         if(flatVel.magnitude > moveSpeed)
//         {
//             Vector3 limitedVel = flatVel.normalized * moveSpeed;
//             rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
//         }
//     }

//     private void Jump()
//     {
//         // reset y velocity
//         rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

//         rb.AddForce(transform.up * jumpForce * jumpBoost, ForceMode.Impulse);
//     }

//     private void ResetJump()
//     {
//         readyToJump = true;
//     }

//     //On collision, ground the player and check for properties of the platform
//     private void OnCollisionEnter(Collision collision)
//     {
//         grounded = true;

//         //PlatformManager P = collision.gameObject.GetComponent<PlatformManager>();
//         if (collision.transform.tag == "Platform")
//         {
//             transform.parent = collision.transform.parent; //sets player as child of the platform in order to support moving platforms
//             PlatformManager P = collision.gameObject.GetComponentInParent<PlatformManager>();

//             if (P.bounciness > 0) //bounce handling
//             {
//                 jumpBoost = 1.0f + P.bounciness;
//                 float bv = 0;
//                 if (collision.relativeVelocity.y > 2)
//                 {
//                     bv += P.bounciness * collision.relativeVelocity.y;
//                 }
//                 rb.AddForce(transform.up * bv, ForceMode.Impulse);
//             }

//             if (P.lethal)
//             {
//                 respawn();
//             }
//         }
//     }

//     //Upon leaving a surface, set player to ungrounded and unparent
//     private void OnCollisionExit(Collision collision)
//     {
//         grounded = false;

//         transform.parent = null;

//         jumpBoost = 1.0f;
//     }

//     public void changeControls(bool value)
//     {
//         is2D = value;
//     }

//     public void resetOrientation()
//     {
//         orientation.eulerAngles = new Vector3(0f, 0f, 0f);
//         tf.eulerAngles = new Vector3(0f, 0f, 0f);
//     }

//      public void respawn()
//     {
//         transform.position = respawnLocation;
//         resetOrientation();
//         rb.velocity = Vector3.zero;
//         // Reset crouch state on respawn
//         isCrouching = false;
//         transform.localScale = originalScale;
//     }
// }
