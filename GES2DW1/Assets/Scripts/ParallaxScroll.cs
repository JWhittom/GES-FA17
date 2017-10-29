using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour {

    [SerializeField]
    Transform cameraPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        FollowCamera();
	}

    private void FollowCamera()
    {
        Vector3 newPos = cameraPos.position;
        newPos.y = transform.position.y;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
