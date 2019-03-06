using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyObject : MonoBehaviour {

    // Use this for initialization
    public float movementSpeed = 10;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(Vector3.back * movementSpeed*Time.deltaTime);
		
	}
}
