using System.Collections;
using System;

using System.Collections.Generic;
using UnityEngine;

public class CubitoHitController : MonoBehaviour
{

    // Use this for initialization
    private SteamVR_TrackedObject trackedObj;
    public Material offColor;
    public int numCubes;
    public int numFrozenCubes = 3;
    public int time_limit = 5;
    public Material leftControllerColor;
    public Material rightControllerColor;
    public int leftControllerIndex = 3;
    public int rightControllerIndex = 4;
    //private int[] AllowedValues = new int[] { 1, 2, 3, 4,5,6, 7,8,9,10, 11,12 };
    private int[] timerArr = new int[12];
    public int timerVar = 15;

    private GameObject child;

    public Material frozenColor;
    private GameObject collidingObject;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    void Start()
    {
        Debug.Log("rand Started choosen : ");
        //InvokeRepeating("randomSwitchOnCube", 0.8f, 0.8f);
        //InvokeRepeating("killCube", 0.8f, 1f);
        //InvokeRepeating("endGame", 0.8f, 0.8f);
    }

    private Vector3 RandomVector(float min, float max)
    {
        var x = UnityEngine.Random.Range(min, max);
        var y = UnityEngine.Random.Range(min, max);
        var z = UnityEngine.Random.Range(min, max);
        return new Vector3(x, y, z);
    }
    private void ChangeColor()
    {
        Debug.Log(collidingObject + " OBJECT");
        Material currentColor= collidingObject.GetComponentInChildren<Renderer>().sharedMaterial;
       
        Debug.Log("color:" + currentColor);
        //Material currentColor = collidingObject.GetComponent<Renderer>().sharedMaterial;
        //if (((int)trackedObj.index) == leftControllerIndex && currentColor == leftControllerColor || ((int)trackedObj.index) == rightControllerIndex && currentColor == rightControllerColor)
        //{

        Debug.Log("1234");
        List<Renderer> results = new List<Renderer>();
        //collidingObject.GetComponentsInChildren<Renderer>(false, results);
        //foreach (object o in results)
        //{
        //    Debug.Log(o);
        //}
        //results.ForEach(Debug.Log);
        //Debug.Log(results);
        collidingObject.GetComponentInChildren<Renderer>().sharedMaterial = offColor;
        Debug.Log("4321");
        Debug.Log("color:" + collidingObject.GetComponentInChildren<Renderer>().sharedMaterial);
        int x;
            String S = collidingObject.ToString().Substring(4);
            String[] parts = S.Split();
            //Debug.Log("Length:"+ collidingObject.ToString().Length);
            //Debug.Log("Length:" + collidingObject.ToString());
            //Debug.Log("String:" + S);
            //Debug.Log("Switching the cube of " + int.TryParse(parts[0],out x));
            //int.TryParse(parts[0], out x);
            //timerArr[x - 1] = 0;

           // var rb = GetComponent<Rigidbody>();
            //rb.velocity = RandomVector(0f, 2f);
            //timerArr[int.TryParse(collidingObject.ToString().Substring(4))-1] = 0;
            //StopCoroutine(freezeCube(collidingObject));
        //}

    }
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        Debug.Log(col.gameObject + " col.gameObject");
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ontriggerenter");
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
    /*
    public void endGame()
    {
        Debug.Log("In end game");
        if (numFrozenCubes==0)
        {
          Debug.Log("In end game if");
          CancelInvoke("randomSwitchOnCube");
        }
    }
    */
    IEnumerator freezeCube(GameObject currentCube, int timeSeconds)
    {
        yield return new WaitForSeconds(timeSeconds);
        numFrozenCubes--;
        currentCube.GetComponent<Renderer>().material = frozenColor;
    }
    public void randomSwitchOnCube()
    {
        //Debug.Log("rand choosen : " + choosenCube);
        System.Random rnd = new System.Random();
        int chosenCube = rnd.Next(1, numCubes + 1);
        //int chosenCube = AllowedValues[rnd.Next(AllowedValues.Length)];
        //Debug.Log("rand choosen : " + choosenCube);
        int chosenColor = rnd.Next(1, 3);
        //Debug.Log("COLOR: "+leftControllerColor.color);
        //Debug.Log("GAME OBJ: "+GameObject.Find(" / Cube" + chosenCube).GetComponent<Renderer>().material);
        GameObject currentCube = GameObject.Find("/Cube" + chosenCube);
        if (GameObject.Find("/Cube" + chosenCube).GetComponent<Renderer>().sharedMaterial == offColor)
        {
            if (chosenColor == 1)
            {
                GameObject.Find("/Cube" + chosenCube).GetComponent<Renderer>().material = leftControllerColor;
            }
            else
            {
                GameObject.Find("/Cube" + chosenCube).GetComponent<Renderer>().material = rightControllerColor;
            }

            setTimeForCube(chosenCube);
            //StartCoroutine(freezeCube(currentCube,50));
        }
    }
    public void setTimeForCube(int cubeIndex)
    {
        timerArr[cubeIndex - 1] = timerVar;
    }

    public void killCube()
    {
        for (int i = 0; i < numCubes; i++)
        {
            Debug.Log("DECR:" + timerArr[i]);
            if (timerArr[i] != 0)
            {
                timerArr[i] = timerArr[i] - 1;
                Debug.Log("Kill:" + timerArr[i]);
                if (timerArr[i] == 0)
                {
                    GameObject.Find("/Cube" + (i + 1)).GetComponent<Renderer>().material = frozenColor;
                }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
        if (Controller.GetHairTriggerDown())
        {
            Debug.Log("Inside IF, get Hair trigger down");
            if (collidingObject)
            {
                Debug.Log("colliding object");
                ChangeColor();

            }
        }
    }
}
