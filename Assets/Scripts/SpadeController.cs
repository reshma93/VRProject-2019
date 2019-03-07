﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpadeController : MonoBehaviour
{

    private GameObject collidingObject;
    private SteamVR_TrackedObject trackedObj;

    public delegate void IncreaseScore();
    public static event IncreaseScore increaseScore;

    public delegate void StopGame();
    public static event StopGame stopGame;

    private static HashSet<GameObject> frozenPotSet = new HashSet<GameObject>();
    private bool gameOver = false;
    private int frozenPotCount = 4;
    // Start is called before the first frame update
    void Start()
    {
        PotController.increaseFrozenPotsCount += IncrementPotCount;
        // Debug.Log("blah"+GameObject.Find("gover"));
        //  GameObject.Find("gover").SetActive(false);
        //ScoreController.activatePot += updateFrozenPotCount;
    }
    public static void IncrementPotCount(GameObject pot)
    {
        Debug.Log("In increment pot count");
         frozenPotSet.Add(pot);
        //Debug.Log("Called from " + pot);
        //Debug.Log("Killed pots - " + frozenPots);
    }

    public void updateFrozenPotCount(string tag)
    {
        if (tag.Equals("pot_8"))
        {
            frozenPotCount = 5;
        }
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

    void potClicked()
    {
        if (!gameOver)
        {
            //if(!collidingObject.GetComponent<PotController>().alreadyKilled)
            //{
                Transform coins = collidingObject.transform.GetChild(1);
                int coinType = collidingObject.GetComponent<PotController>().coinType;
                if ((coinType == 0 && trackedObj.name.Equals("Controller (left)")) || (coinType == 1 && trackedObj.name.Equals("Controller (right)")))
                {
                    coins.gameObject.SetActive(false);
                    collidingObject.GetComponent<PotController>().setInitialStates();
                    increaseScore();
                }
            //}

        }
    }

    void Update()
    {
        if (frozenPotSet.Count >= 5)
        {
            Debug.Log("Enter here.....................!!!!");
            stopGame();
            gameOver = true;
            GameObject.Find("gover").GetComponent<TextMeshPro>().text = "GAME OVER";
        }
        if (Controller.GetHairTriggerDown())
        {
          //  Debug.Log("hair trigger got");
            if (collidingObject)
            {
              //  Debug.Log("inside if");
                potClicked();
            }
        }
      //  Debug.Log("froze " + frozenPots);
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
        //Debug.Log("Ontriggerenter");
        SetCollidingObject(other);

    }
    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("set collidingr");
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
