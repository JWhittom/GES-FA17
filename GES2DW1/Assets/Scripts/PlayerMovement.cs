using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    
    // Public variables

    // Win state enabled
    public bool canWin;

    // Private variables

    // Animator
    Animator animator;
    // Audio Source
    AudioSource audioSource;
    // Can the player control the character?
    bool canControl;
    // Can the player grab?
    bool canGrab;
    // Should the scene change?
    bool changeScene;
    // Character direction
    bool facingRight = true;
    // Is the player grabbing?
    bool isGrabbing;
    // Is the player on the ground?
    bool isOnGround;
    // Check death
    bool shouldDie;
    // Check jumping
    bool shouldJump;
    // Collision Data
    BoxCollider2D boxCol;
    // Length of timer
    float changeTime = 2.0f;
    // Timer for scene change
    float changeTimeStart = 0.0f;
    // Length for death
    float dieTime = 1.0f;
    // Timer for death
    float dieTimeStart;
    // Ungrab Cooldown
    float grabTime;
    // Horizontal input check
    float horizontalInput;
    // Grab joint
    HingeJoint2D grab;
    // Rigidbody for 2D Physics
    Rigidbody2D rb2D;
    // Object to grab
    Rigidbody2D ropeRB;
    // Jump force
    Vector2 jumpForce;
    // Respawn position
    Vector3 respPos;

    // SerializeFields

    // Audio clips
    [SerializeField]
    AudioClip[] audioClips;
    // Radius for ground detection circle
    [SerializeField]
    float groundDetectRadius = 0.25f;
    // Jump strength (force added when jumping)
    [SerializeField]
    float jumpStrength;
    // Movement speed
    [SerializeField]
    float moveSpeed = 5.0f;
    // Y value for bottomless pit death
    [SerializeField]
    float pitThreshhold;
    // Rope detection radius
    [SerializeField]
    float ropeRad = 0.55f;
    // Rope layer
    [SerializeField]
    LayerMask countsAsRope;
    // Ground layer
    [SerializeField]
    LayerMask whatCountsAsGround;
    // Name of next scene
    [SerializeField]
    string nextScene;
    // Center point for ground detection circle
    [SerializeField]
    Transform groundDetectPoint;
    // Center point for rope detection circle
    [SerializeField]
    Transform ropeCheck;

    // Use this for initialization
    void Start()
    {
        // this code teleports the game object
        //transform.position = new Vector3(0, 0, 0);
        jumpForce = new Vector2(0, jumpStrength);
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        respPos = transform.position;
        canControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetJumpInput();
        checkDie();
        CanGrab();
        Grab();
        UnGrab();
        UpdateIsOnGround();
    }

    // FixedUpdate is called once every 50th of a second
    private void FixedUpdate()
    {
        if (canControl)
        {
            Move();
            Jump();
            AnimUpdate();
        }
        if (shouldDie)
            Die();
        if (changeScene)
        {
            if (changeTimeStart == 0.0f)
                changeTimeStart = Time.time;
            if (Time.time - changeTimeStart >= changeTime)
            {
                Key.keyCount = 0;
                Key.totalKeys = 0;
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    // Called when the player collides
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Spikes"))
            shouldDie = true;
    }

    // Called when the player enters a trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
            respPos = collision.gameObject.transform.position;
        if (collision.gameObject.tag == "Victory" && canWin && isOnGround)
        {
            AudioSource win = collision.gameObject.GetComponent<AudioSource>();
            canControl = false;
            win.Play();
            animator.SetFloat("vert", 1f);
            animator.SetFloat("speed", 0.0f);
            changeScene = true;
        }
    }

    // Update animator variables
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

    // Check grab state
    private void CanGrab()
    {
        Collider2D[] ropeObjects = Physics2D.OverlapCircleAll(ropeCheck.position, ropeRad, countsAsRope);
        if (ropeObjects.Length > 0 && !isGrabbing)
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

    // Check for death state
    private void checkDie()
    {
        if (transform.position.y < pitThreshhold || (dieTimeStart != 0 && Time.time - dieTimeStart >= dieTime))
            shouldDie = true;
    }

    // Play death sound/animation and respawn player
    private void Die()
    {
        audioSource.clip = audioClips[1];
        animator.SetBool("die", true);
        if (dieTimeStart == 0.0f)
        {
            audioSource.Play();
            canControl = false;
            dieTimeStart = Time.time;
            rb2D.velocity = new Vector2(0f, 0f);
        }
        if (Time.time - dieTimeStart >= dieTime)
        {
            transform.position = respPos;
            animator.SetBool("die", false);
            dieTimeStart = 0.0f;
            canControl = true;
            shouldDie = false;
        }
    }

    // Flip player horizontally
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Take jump input
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

    // Take movement input
    private void GetMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }
    
    // Grab a rope
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

    private void UnGrab()
    {
        if(Input.GetButtonDown("Jump") && isGrabbing && Time.time - grabTime > .5f)
        {
            Destroy(grab);
            ropeRB = null;
            isGrabbing = false;
            boxCol.enabled = false;
        }
    }

    // Checks for ground below and updates the isOnGround variable accordingly
    private void UpdateIsOnGround()
    {
        Collider2D[] groundObjects = Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatCountsAsGround);
        isOnGround = groundObjects.Length > 0;
        boxCol.enabled = true;
        if (isGrabbing)
            isOnGround = true;
    }
}
