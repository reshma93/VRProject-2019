using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour {

    // Use this for initialization

    private string glass_number;
    private bool blink = false;
    private Material blink_colour;
    public Material blink_colour_red;
    public Material blink_colour_black;
    private Material original_colour;
    private float blink_timer = 0.8f;
    private GameObject meshObject;



    void Start () {

        SharkController.blinkGlassBegin += beginBlink;
        GameController.blinkGlassStop += stopBlink;
        SharkController.blinkGlassStop += stopBlink;

        meshObject = transform.GetChild(1).GetChild(0).gameObject;
        original_colour = meshObject.GetComponent<Renderer>().sharedMaterial;


    }
	
    void beginBlink(string fish_number, int colour)
    {

        //Debug.Log(transform);
        glass_number =transform.gameObject.ToString().Substring(6, 2);
        if(glass_number.Equals(fish_number))
        {
            blink = true;
            if (colour == 1)
            {
                blink_colour = blink_colour_red;
            }
            else
            {
                blink_colour = blink_colour_black;
            }
        }

    }

    void stopBlink(string fish_number)
    {
        glass_number = transform.gameObject.ToString().Substring(6, 2);
        if (glass_number.Equals(fish_number))
        {
            blink = false;
            meshObject.GetComponent<Renderer>().material = original_colour;

        }
    }
	// Update is called once per frame
	void Update () {
        if(blink)
        {
            blink_timer -= Time.deltaTime;
            Debug.Log("This glass blink!! " + glass_number);
            meshObject.GetComponent<Renderer>().material = blink_colour;
            meshObject.GetComponent<Renderer>().enabled = true;
            
            if(blink_timer < 0)
            {
                meshObject.GetComponent<Renderer>().enabled = false;
                
                blink_timer = 0.8f;
            }
            



        }
        

    }
}
