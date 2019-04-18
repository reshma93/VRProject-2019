using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseController : MonoBehaviour
{

    private GameObject collidingObject;
    private GameObject KnobLight;

    private SteamVR_Controller.Device device;
    private SteamVR_TrackedObject trackedObj;

    private int lightIntensity = 0;
    private float lightTimer = 0;
    private Animator bookAnim;

    AsyncOperation scene_1;
    // Start is called before the first frame update
    void Start()
    {
        KnobLight = GameObject.FindGameObjectWithTag("KnobLight");

        //scene_1 = SceneManager.LoadSceneAsync("Demo", LoadSceneMode.Single);
        //scene_1.allowSceneActivation = false;
    }

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        lightTimer += Time.deltaTime;
        if(lightTimer >= 0.7)
        {
            KnobLight.GetComponent<Light>().intensity += 5;
            if (KnobLight.GetComponent<Light>().intensity > 50)
            {
                KnobLight.GetComponent<Light>().intensity = lightIntensity;
            }
            lightTimer = 0;

        }

        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject.name == "book")
            {
               
                bookAnim = collidingObject.GetComponent<Animator>();
                bookAnim.enabled = true;
            
                collidingObject.GetComponent<AudioSource>().enabled = true;
            }

            if(collidingObject.name == "Door")
            {
                SceneManager.LoadScene(2);
                //scene_1.allowSceneActivation = true;
            }
        }
            
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
    }
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
        //hapticFlag = true;

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
        //hapticFlag = false;
    }
}
