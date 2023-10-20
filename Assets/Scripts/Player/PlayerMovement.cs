using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public float crouchYScale;
    public float crouchSpeed;

    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public KeyCode crouchKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    private PlayerInformation playerState;

    public enum MovementState {
        walking,
        sprinting,
        crouching,
        stationary,
        air
    }

    private void MovementStateSoundSetter() {
        int intensity;
        switch (this.state) {
            case MovementState.walking:
                intensity = 2;
                break;
            case MovementState.sprinting:
                intensity = 7;
                break;
            case MovementState.crouching:
                intensity = 1;
                break;
            default:
                intensity = 0;
                break;
        }

        if (intensity != 0)
            playerState.SetDominatingSoundIntensity(intensity);
        else
            playerState.SetSoundIntensity(0);
        
    }

    private void StateHandler() {
        // Mode - Crouching
        if (Input.GetKeyDown(crouchKey)) {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey)) {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        } 
        
        // Mode - Walking
        else if (grounded) {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else if (moveDirection == Vector3.zero)
            state = MovementState.stationary;

        // Mode - Air
        else
            state = MovementState.air;
        
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
    }

    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        MyInput();
        SpeedControl();
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        StateHandler();
        // Debug.Log("Sound Intensity: " + playerState.GetSoundIntensity());

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        } // stop crouch
        else if (Input.GetKeyUp(crouchKey))
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        
    }

    private void MovePlayer() {

        // Debug.Log("Is it zero? " + playerState.GetSoundIntensity());
        if (moveDirection.x > 0 || moveDirection.z > 0) // moving so making noise
            MovementStateSoundSetter();
        else
            playerState.SetSoundIntensity(0);
        
        // on slope
        if (OnSlope() && !exitingSlope) {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force); 

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // airborne
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    
        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl() {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope) {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump() {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}