using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    // Rigidbody for 2D Physics
    Rigidbody2D rb2D;
    // Audio Source
    AudioSource audioSource;
    // Is the player on the ground?
    bool isOnGround;
    // Horizontal input check
    float horizontalInput;
    // Check jumping
    bool shouldJump;
    // Jump force
    Vector2 jumpForce;
    // Can the player grab?
    bool canGrab;
    // Is the player grabbing?
    bool isGrabbing;
    // Grab joint
    HingeJoint2D grab;
    // Object to grab
    Rigidbody2D ropeRB;
    // Ungrab Cooldown
    float grabTime;
    // Movement speed
    [SerializeField]
    float moveSpeed = 5.0f;
    // Jump strength (force added when jumping)
    [SerializeField]
    float jumpStrength;
    // Center point for ground detection circle
    [SerializeField]
    Transform groundDetectPoint;
    // Radius for ground detection circle
    [SerializeField]
    float groundDetectRadius = 0.25f;
    // Ground layer
    [SerializeField]
    LayerMask whatCountsAsGround;
    // Center point for rope detection circle
    [SerializeField]
    Transform ropeCheck;
    // Rope detection radius
    [SerializeField]
    float ropeRad = 0.55f;
    // Rope layer
    [SerializeField]
    LayerMask countsAsRope;

    // Use this for initialization
    void Start()
    {
        // this code teleports the game object
        //transform.position = new Vector3(0, 0, 0);
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpForce = new Vector2(0, jumpStrength);
        GetMovementInput();
        GetJumpInput();
        CanGrab();
        Grab();
        UnGrab();
        UpdateIsOnGround();
    }

    private void GetJumpInput()
    {
        if(Input.GetButtonDown("Jump") && isOnGround)
        {
            shouldJump = true;
        }
        else
        {
            shouldJump = false;
        }
    }

    private void GetMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void CanGrab()
    {
        Collider2D[] ropeObjects = Physics2D.OverlapCircleAll(ropeCheck.position, ropeRad, countsAsRope);
        if(ropeObjects.Length > 0 && !isGrabbing)
        {
            canGrab = true;
            ropeRB = ropeObjects[0].attachedRigidbody;
        }
        else
        {
            canGrab = false;
            ropeRB = null;
        }
    }

    private void Grab()
    {
        if(Input.GetButtonDown("Jump") && canGrab)
        {
            grab = gameObject.AddComponent<HingeJoint2D>();
            grabTime = Time.time;
            grab.connectedBody = ropeRB;
            isGrabbing = true;
            canGrab = false;
        }
    }

    private void UnGrab()
    {
        if(Input.GetButtonDown("Jump") && isGrabbing && Time.time - grabTime > .5f)
        {
            Destroy(grab);
            ropeRB = null;
            isGrabbing = false;
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Rope")
        {
            if(!isGrabbing)
            {
                grab = gameObject.AddComponent<HingeJoint2D>();
                grab.connectedBody = collision.rigidbody;
                isGrabbing = true;
            }
        }
    }*/

    // Checks for ground below and updates the isOnGround variable accordingly
    private void UpdateIsOnGround()
    {
        Collider2D[] groundObjects = Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatCountsAsGround);
        isOnGround = groundObjects.Length > 0;
        if (isGrabbing)
            isOnGround = true;
    }

    // Player movement logic
    private void Move()
    {
        if (!isGrabbing)
            rb2D.velocity = new Vector2(horizontalInput * moveSpeed, rb2D.velocity.y);
        else
            rb2D.AddForce(new Vector2(horizontalInput * moveSpeed * 2, 0));
        // Translate() doesn't use physics
        //transform.Translate(new Vector3(0.01f, 0, 0));
        // GetKey is difficult if things are going to change even a little bit
        //else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.Translate(new Vector3(-0.1f, 0, 0));
        //}
    }
    
    // Player jump logic
    private void Jump()
    {
        if (shouldJump)
        {
            // Don't use different/amalgamate systems for movement (it's like mixing battery types)
            //transform.translate(0, 1.0f, 0);
            //rb2D.velocity = new Vector2(rb2D.velocity.x, jumpStrength);
            audioSource.Play();
            rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            shouldJump = false;
        }
    }
}
