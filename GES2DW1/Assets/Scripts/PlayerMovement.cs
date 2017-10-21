using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    // Rigidbody for 2D Physics
    Rigidbody2D rb2D;
    // Is the player on the ground?
    bool isOnGround;
    // Horizontal input check
    float horizontalInput;
    // Check jumping
    bool shouldJump;
    // Jump force
    Vector2 jumpForce;
    // Is the player grabbing?
    bool isGrabbing;
    // Grab joint
    HingeJoint2D grab;
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

    // Use this for initialization
    void Start()
    {
        // this code teleports the game object
        //transform.position = new Vector3(0, 0, 0);
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpForce = new Vector2(0, jumpStrength);
        GetMovementInput();
        GetJumpInput();
        Ungrab();
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

    private void Ungrab()
    {
        if(Input.GetButtonDown("Jump") && isGrabbing)
        {
            Destroy(grab);
            isGrabbing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Rope")
        {
            Debug.Log("Rope");
            if(!isGrabbing)
            {
                grab = gameObject.AddComponent<HingeJoint2D>();
                grab.connectedBody = collision.rigidbody;
                isGrabbing = true;
            }
        }
    }

    // Checks for ground below and updates the isOnGround variable accordingly
    private void UpdateIsOnGround()
    {
        Collider2D[] groundObjects = Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatCountsAsGround);
        isOnGround = groundObjects.Length > 0;
    }

    // Player movement logic
    private void Move()
    {
        rb2D.velocity = new Vector2(horizontalInput * moveSpeed, rb2D.velocity.y);
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
            rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            shouldJump = false;
        }
    }
}
