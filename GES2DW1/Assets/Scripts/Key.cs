using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour {

    public static int keyCount = 0;
    public static int totalKeys = 0;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCol;
    Text keyCountText;
    private void Start()
    {
        totalKeys++;
        keyCountText = GameObject.Find("KeyCountText").GetComponent<Text>();
        UpdateText();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(keyCount == totalKeys)
        {
            MoveOn();
        }
    }

    private void MoveOn()
    {
        keyCountText.color = Color.yellow;
        GameObject.Find("Player").GetComponent<PlayerMovement>().canWin = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            audioSource.Play();
            keyCount++;
            spriteRenderer.enabled = false;
            boxCol.enabled = false;
            UpdateText();
            // Destroy(gameObject);
        }
    }

    void UpdateText()
    {
        keyCountText.text = "Keys: " + keyCount + "/" + totalKeys;
    }
}
