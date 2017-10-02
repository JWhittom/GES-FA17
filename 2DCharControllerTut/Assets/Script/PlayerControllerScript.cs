using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {

    [SerializeField] float maxSpeed = 5f;
    bool facingRight = true;
    Rigidbody2D rb2D;
    Animator anim;
    bool grounded = false;
    [SerializeField] Transform groundCheck;
    float groundRadius = .2f;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float jumpForce = 700f;
	// Use this for initialization
	void Start () {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", rb2D.velocity.y);
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
        rb2D.velocity = new Vector2(move * maxSpeed, rb2D.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
	}

    void Update()
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("Ground", false);
            rb2D.AddForce(new Vector2(0, jumpForce));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
