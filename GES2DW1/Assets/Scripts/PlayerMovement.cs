using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    Rigidbody2D rb2D;
    [SerializeField]
    float moveSpeed = 5.0f;
    // Use this for initialization
    void Start() {
        // this code teleports the game object
        //transform.position = new Vector3(0, 0, 0);
        rb2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("Velocity: " + rb2D.velocity);
        // Translate() doesn't use physics
        //transform.Translate(new Vector3(0.01f, 0, 0));
        // GetKey is difficult if things are going to change even a little bit
        //else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.Translate(new Vector3(-0.1f, 0, 0));
        //}

        float horizontalInput = Input.GetAxis("Horizontal");

        rb2D.velocity = new Vector2(horizontalInput * moveSpeed, rb2D.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            transform.Translate(0, 1.0f, 0);
        }
    }
}
