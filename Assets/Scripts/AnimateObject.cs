using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateObject : MonoBehaviour {

   // public GameObject objectToMove;
    // Use this for initialization

    private Vector3 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = Random.Range(min, max);
        var z = Random.Range(min, max);
        return new Vector3(x, y, z);
    }
    void Start () {
        // objectToMove.velocity = RandomVector(0f, 5f);
        var rb = GetComponent<Rigidbody>();
        rb.velocity= RandomVector(0f, 2f);
    }
	
	// Update is called once per frame
	void Update () {
        
       
    }
}
