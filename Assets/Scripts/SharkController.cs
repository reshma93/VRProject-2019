using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour {

	// Use this for initialization

    private Vector3 initial_position;
    private float activeTimer;
    private float deadStateTimer=10;
    private bool isSharkClicked;
    private bool isColourSetToRed;
    private int sharkColour;
    public Transform chosenShark;
    private Transform[] shark = new Transform[2];
    public int sharkType;
    public bool clickable=true;

    public delegate void BlinkGlassBegin(string fish_number, int colour);
    public static event BlinkGlassBegin blinkGlassBegin;

    public delegate void BlinkGlassStop(string fish_number);
    public static event BlinkGlassStop blinkGlassStop;

    
    void Start () {
        //Debug.Log("Transform" + transform);
        int i = 0;
        foreach(Transform child in transform)
        {
            shark[i] = child;
            i++;
            child.gameObject.SetActive(false);
            //Debug.Log(child);

            initial_position = child.position;
        }
        //initial_position = transform.position;
   
        //initial_position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Debug.Log("initial: " + initial_position);
        setInitialStates();

        
    }
	public void setInitialStates()
    {
        if(chosenShark)
        {          
            chosenShark.position = initial_position;          
        }
        activeTimer = Random.Range(2, 11);
        Debug.Log("Active Timer:" + activeTimer);
        sharkColour = Random.Range(1, 3);
        if(sharkColour == 1)
        {
            sharkType = 1;
            chosenShark = shark[1];
            Debug.Log("Shark type:" + sharkType);
            Debug.Log("Chosen Shark:" + chosenShark);
        }
        else
        {
            sharkType = 0;
            chosenShark = shark[0];
            Debug.Log("Shark type:" + sharkType);
            Debug.Log("Chosen Shark:" + chosenShark);
        }
        isSharkClicked = false;
        isColourSetToRed = false;
        deadStateTimer = 10;
        //transform.position = initial_position;
    }
	// Update is called once per frame
	void Update () {
        activeTimer -= Time.deltaTime;
        if(activeTimer < 0 && !isColourSetToRed)
        {
            chosenShark.gameObject.SetActive(true);
            isColourSetToRed = true;
            blinkGlassBegin( transform.ToString().Substring(6, 2), sharkType);
        }
        if(isColourSetToRed && deadStateTimer > 0)
        {
            //Debug.Log("should translate now");
            deadStateTimer -= Time.deltaTime;
            chosenShark.Translate(Vector3.right * 0.5f * Time.deltaTime);
        }
        if(deadStateTimer < 0 )
        {
            if(!isSharkClicked)
            {

                string fish_number = transform.ToString().Substring(6, 2);

                //saving num into some set 
                //GameObject.Find("Glass_" + fish_number)
                clickable = false;
                blinkGlassStop(fish_number);

                //Debug.Log("boo");
            }
        }
	}
}
