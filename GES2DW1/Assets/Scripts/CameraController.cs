﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    // Location of object for camera to follow
    [SerializeField]
    Transform objectToFollow;
    [SerializeField]
    float cameraFollowSpeed = 5;
    [SerializeField]
    float xOffset;
    [SerializeField]
    float yOffset;

    float ymin = 0;
    float xmin = 0;
    float zOffset = -10;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 newPosition = new Vector3(objectToFollow.position.x + xOffset, objectToFollow.position.y + yOffset, zOffset);
        if (newPosition.x < xmin)
            newPosition.x = xmin;
        if (newPosition.y < ymin)
            newPosition.y = ymin;
        transform.position = Vector3.Lerp(transform.position, newPosition, cameraFollowSpeed * Time.deltaTime);
	}
}
