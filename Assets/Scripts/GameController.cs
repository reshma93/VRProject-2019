using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private GameObject collidingObject;
    private SteamVR_TrackedObject trackedObj;

    public delegate void BlinkGlassStop(string fish_number);
    public static event BlinkGlassStop blinkGlassStop;

    public delegate void OnCorrectGlassHitDelegate();
    public static OnCorrectGlassHitDelegate onCorrectGlassHitDelegate;

    // Use this for initialization
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
   
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    void Start () {
		
	}

    void sharkClicked()
    {
        GameObject grandParent = collidingObject.transform.parent.parent.gameObject;
        string fish_number = grandParent.ToString().Substring(6,2);
        GameObject shark_obj = GameObject.Find("shark_" + fish_number);

        int sharkType = shark_obj.GetComponent<SharkController>().sharkType;
        if (shark_obj.GetComponent<SharkController>().clickable)
        {
            if (sharkType == 0 && trackedObj.name.Equals("Controller (left)") || sharkType == 1 && trackedObj.name.Equals("Controller (right)"))
            {
                shark_obj.GetComponent<SharkController>().chosenShark.gameObject.SetActive(false);
                shark_obj.GetComponent<SharkController>().setInitialStates();
                blinkGlassStop(fish_number);
                //pass an event for score
                onCorrectGlassHitDelegate();

                Controller.TriggerHapticPulse(500);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Controller.GetHairTriggerDown())
        {
            //Debug.Log("hair trigger got");
            if (collidingObject)
            {
                sharkClicked();
            }
        }
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
       // Debug.Log(col.gameObject + " col.gameObject");
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Ontriggerenter");
        SetCollidingObject(other);

    }
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
 
        collidingObject = null;
    }

}
