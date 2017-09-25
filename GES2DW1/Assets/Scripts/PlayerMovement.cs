using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    // Rigidbody for 2D Physics
    Rigidbody2D rb2D;
    // Is the player on the ground?
    bool isOnGround;
    // Movement speed
    [SerializeField]
    float moveSpeed = 5.0f;
    // Jump strength (force added when jumping)
    [SerializeField]
    float jumpStrength = 7.0f;
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
        UpdateIsOnGround();
        Move();
        Jump();
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
        float horizontalInput = Input.GetAxis("Horizontal");

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
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            // Don't use different/amalgamate systems for movement (it's like mixing battery types)
            //transform.translate(0, 1.0f, 0);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpStrength);
            isOnGround = false;
        }
    }
}
