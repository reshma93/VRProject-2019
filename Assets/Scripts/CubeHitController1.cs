using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeHitController1 : MonoBehaviour
{

    // Use this for initialization
    private SteamVR_TrackedObject trackedObj;
    public Material offColor;
    public int numCubes;
    public int numFrozenCubes = 3;
    public int time_limit = 5;
    public Material leftControllerColor;
    public Material rightControllerColor;
    public int leftControllerIndex = 2;
    public int rightControllerIndex = 1;
    //private int[] AllowedValues = new int[] { 1, 2, 3, 4,5,6, 7,8,9,10, 11,12 };
    private int[] timerArr = new int[12];
    public int timerVar = 5;

    private IEnumerator[] timer = new IEnumerator[13];

    public Material frozenColor;
    private GameObject collidingObject;

    private List<Collider> TriggerList = new List<Collider>();
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    void Start()
    {
       // Debug.Log("rand Started choosen : ");
        InvokeRepeating("randomSwitchOnCube", 2.0f, 0.8f);
        //InvokeRepeating("killCube", 0.8f, 1.0f);
        //InvokeRepeating("endGame", 0.8f, 0.8f);
    }
    private void ChangeColor()
    {
        //Debug.Log(collidingObject + " OBJECT");
        Material currentColor = collidingObject.GetComponent<Renderer>().sharedMaterial;
       // if (((int)trackedObj.index) == leftControllerIndex && currentColor == leftControllerColor || ((int)trackedObj.index) == rightControllerIndex && currentColor == rightControllerColor)
        //{
          //  Debug.Log("Inside!");
            collidingObject.GetComponent<Renderer>().material = offColor;

        // Stop coroutine
           
        int x;
            String S = collidingObject.ToString().Substring(4);
            String[] parts = S.Split();
            //Debug.Log("Length:"+ collidingObject.ToString().Length);
            //Debug.Log("Length:" + collidingObject.ToString());
            //Debug.Log("String:" + S);
            //Debug.Log("Switching the cube of " + int.TryParse(parts[0],out x));
            int.TryParse(parts[0], out x);

        if (timer[x] != null)
        {
            Debug.Log("Stopping coroutine for " +x);
            StopCoroutine(timer[x]);
        }
        
        //Debug.Log("Decolored the cube : " + x + "  initial time" + timerArr[x - 1]);
        //timerArr[x - 1] = 0;
        //Debug.Log("Decolored the cube : " + x + "  final time is " + timerArr[x - 1]);
        //timerArr[int.TryParse(collidingObject.ToString().Substring(4))-1] = 0;
        
        //}
    }

    private void ChangeColor_List(List<Collider> TriggerList)
    {
        foreach(var collidingObject in TriggerList)
        {
            //Debug.Log(collidingObject + " OBJECT");
            Material currentColor = collidingObject.GetComponent<Renderer>().sharedMaterial;
            if (((int)trackedObj.index) == leftControllerIndex && currentColor == leftControllerColor || ((int)trackedObj.index) == rightControllerIndex && currentColor == rightControllerColor)
            {
              //  Debug.Log("Inside!");
                collidingObject.GetComponent<Renderer>().material = offColor;
                int x;
                String S = collidingObject.ToString().Substring(4);
                String[] parts = S.Split();
                //Debug.Log("Length:"+ collidingObject.ToString().Length);
                //Debug.Log("Length:" + collidingObject.ToString());
                //Debug.Log("String:" + S);
                //Debug.Log("Switching the cube of " + int.TryParse(parts[0],out x));
                int.TryParse(parts[0], out x);
                
                timerArr[x - 1] = 0;
                
                //timerArr[int.TryParse(collidingObject.ToString().Substring(4))-1] = 0;
                //StopCoroutine(freezeCube(collidingObject));
            }
        }
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
        //Debug.Log(col.gameObject + " col.gameObject");
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Ontriggerenter");
        SetCollidingObject(other);

        if (!TriggerList.Contains(other))
        {
            //Debug.Log("Added:"+ other);
            TriggerList.Add(other);
        }

        TriggerList.ForEach(Debug.Log);
      //  Debug.Log("Length:" + TriggerList.Count);
       // Debug.Log("End");
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
        //Swipe Check
        
        collidingObject = null;
    }
    public void VerifySwipe() { 

         Material correctColour = offColor;
        if (((int)trackedObj.index) == leftControllerIndex)
        {
            correctColour = leftControllerColor;
        }

        else if (((int)trackedObj.index) == rightControllerIndex)
        {
            correctColour = rightControllerColor;
        }

      // var allvalue = true;
        int flag = 0;
       // Debug.Log("Swipe");
        foreach (var block in TriggerList)
        {
            //Debug.Log("block "+ block);

            if (block.gameObject.GetComponent<Renderer>().sharedMaterial != correctColour)
            {
                flag = 1;
                Debug.Log("Wrong Swipe!");
                break;

            }
        }
        if(flag == 0)
        {
           // Debug.Log("Verified!");
            ChangeColor_List(TriggerList);
        }
       TriggerList.Clear();
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
        //numFrozenCubes--;
        Debug.Log("Current cube froze : " + currentCube);
        currentCube.GetComponent<Renderer>().material = frozenColor;
    }
    public void randomSwitchOnCube()
    {
        //Debug.Log("rand choosen : " + choosenCube);
        System.Random rnd = new System.Random();
        int chosenCube = rnd.Next(1, numCubes+1);
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
                Debug.Log("if: Called start corutine for " + chosenCube);
                timer[chosenCube] = freezeCube(currentCube, 5);
                StartCoroutine(timer[chosenCube]);
            }
            else
            {
                GameObject.Find("/Cube" + chosenCube).GetComponent<Renderer>().material = rightControllerColor;
                Debug.Log("else : Called start corutine for " + chosenCube);
                timer[chosenCube] = freezeCube(currentCube, 5);
                StartCoroutine(timer[chosenCube]);
            }
            //Debug.Log("set the timer for cube index : " + chosenCube);
            //setTimeForCube(chosenCube);
            //Debug.Log("Time became for cube index : " +chosenCube+" - "+ timerArr[chosenCube-1]);


           
        }
    }
    public void setTimeForCube(int cubeIndex)
    {
        timerArr[cubeIndex-1] = timerVar;
    }

    public void killCube()
    {
        for(int i=0;i<numCubes;i++)
        {
            timerArr[i] = timerArr[i] - 1;
            Debug.Log(timerArr[i]);
            if (timerArr[i] ==1)
            {
                Debug.Log("Killed :" + (i + 1));
                GameObject.Find("/Cube" + (i + 1)).GetComponent<Renderer>().material = frozenColor;
            }
           // Debug.Log("DECR:"+timerArr[i]);    
            //if (timerArr[i] != 0)
            //{
            //    timerArr[i] = timerArr[i] - 1;
            //   // Debug.Log("Kill:"+timerArr[i]);
            //    if (timerArr[i] == 0)
            //    {
            //        Debug.Log("Killed :" + (i+1));
            //        GameObject.Find("/Cube" + (i+1)).GetComponent<Renderer>().material = frozenColor;
            //    }
            //}
           
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                ChangeColor();
            }
        }
        if (Controller.GetHairTriggerUp())
        {
            //Debug.Log(gameObject.name + " Trigger Release");
          //  VerifySwipe();
        }
    }
}
