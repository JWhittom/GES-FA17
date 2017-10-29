using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBG : MonoBehaviour {

    [SerializeField]
    Transform cameraPos;
    [SerializeField]
    float width;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        Repeat();
	}

    void Repeat()
    {
        Vector3 newPos = transform.position;
        if (cameraPos.position.x - width > transform.position.x)
            newPos.x += width * 2;
        else if (cameraPos.position.x + width < transform.position.x)
            newPos.x -= width * 2;
        transform.position = newPos;
    }
}
