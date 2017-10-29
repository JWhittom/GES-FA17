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
    // Respawn position
    Vector3 respPos;
    // Character direction
    bool facingRight = true;
    // Animator
    Animator animator;
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
    // Audio clips
    [SerializeField]
    AudioClip[] audioClips;
    [SerializeField]
    float pitThreshhold;

    // Use this for initialization
    void Start()
    {
        // this code teleports the game object
        //transform.position = new Vector3(0, 0, 0);
        jumpForce = new Vector2(0, jumpStrength);
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        respPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
        Die();
        AnimUpdate();
    }

    private void AnimUpdate()
    {
        if (!isGrabbing)
            animator.SetFloat("vSpeed", rb2D.velocity.y);
        else
            animator.SetFloat("vSpeed", 0f);
        animator.SetBool("grounded", isOnGround);
        animator.SetBool("grabbing", isGrabbing);
        animator.SetFloat("vert", Input.GetAxis("Vertical"));
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
        {
            rb2D.velocity = new Vector2(horizontalInput * moveSpeed, rb2D.velocity.y);
            if (rb2D.velocity.x > 0 && !facingRight)
                Flip();
            else if (rb2D.velocity.x < 0 && facingRight)
                Flip();
            animator.SetFloat("speed", Mathf.Abs(rb2D.velocity.x));
        }
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
            audioSource.clip = audioClips[0];
            audioSource.Play();
            rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            shouldJump = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Die()
    {
        if(transform.position.y < pitThreshhold)
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();
            transform.position = respPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
            respPos = collision.gameObject.transform.position;
    }
}
