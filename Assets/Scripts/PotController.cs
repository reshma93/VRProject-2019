using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour
{

    private float activeTimer;
    private float levitateTimer;
    private float deadStateTimer = 15;
    private float levitateDuration = 10;

    private bool isPotClicked;
    private bool isColourSetToRed;
    public bool levitateFlag = false;
    private bool gameOver = false;
    public bool alreadyKilled = false;
    private bool up = true;
    private bool down = false;
    public bool isPotFrozen = false;

    private int coinsColour;
    public int coinType;
    //private Transform chosenCoins;

    public Material gold;
    public Material silver;
    private Material chosenMaterial;
    public Material frozenMaterial;

    private Transform coins;
    private Transform pot;
    
    private string tag;

    public delegate void IncreaseFrozenPotsCount(GameObject pot);
    public static event IncreaseFrozenPotsCount increaseFrozenPotsCount;

    // Start is called before the first frame update
    void Start()
    {

        coins = transform.GetChild(1);
        pot = transform.GetChild(0);
        tag = transform.tag;
        coins.gameObject.SetActive(false);
        setInitialStates();
        ScoreController.beginLevitate += levitatePot;
        //ScoreController.stopLevitate += stopLevitatePot;
        SpadeController.stopGame += finishGame;
        ScoreController.activatePot += activatePot;
        SpadeController.gameOverEvent += resetGameOverBool;
        transform.gameObject.SetActive(false);
    }

    public void finishGame()
    {
        gameOver = true;
       // Debug.Log("Finish game");
    }
     public void levitatePot()
    {
        levitateFlag = true;
    }

    public void resetGameOverBool()
    {
        gameOver = false;
    }

    //public void stopLevitatePot()
    //{
    //    levitateFlag = false;
    //}



    public void activatePot(string pot_tag)
    {
        if(tag.Equals(pot_tag))
        {
            transform.gameObject.SetActive(true);
            
        }
    }
    public void setInitialStates()
    {

        activeTimer = Random.Range(2, 11);
       // Debug.Log("Active Timer:" + activeTimer);
        coinsColour = Random.Range(1, 3);
        if (coinsColour == 1)
        {
            chosenMaterial = gold;
            coinType = 1;

        }
        else
        {
            chosenMaterial = silver;
            coinType = 0;

        }
        isPotClicked = false;
        isColourSetToRed = false;
        deadStateTimer = 10;

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer < 0 && !isColourSetToRed)
            {
                coins.gameObject.SetActive(true);
                foreach (Transform layer in coins)
                {
                    layer.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = chosenMaterial;
                    layer.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().sharedMaterial = chosenMaterial;
                }

                isColourSetToRed = true;

            }
            if (isColourSetToRed && deadStateTimer > 0)
            {
                deadStateTimer -= Time.deltaTime;

            }
            if (deadStateTimer < 0)
            {
                if (!isPotClicked)
                {

                    //foreach (Transform layer in coins)
                    //{
                    //    layer.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = frozenMaterial;
                    //    layer.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Renderer>().sharedMaterial = frozenMaterial;
                    //    coinType = -1;

                    //}
                    coins.gameObject.SetActive(false);
                    pot.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = frozenMaterial;

                    if (!alreadyKilled)
                    {
                        increaseFrozenPotsCount(transform.gameObject);
                        alreadyKilled = true;
                        Debug.Log("froze " + transform.gameObject);
                    }                   
                }
            }
            //levitate
            if (levitateFlag)
            {

                if (levitateTimer >= levitateDuration || levitateTimer <= -5)
                {
                    up = !up;
                    down = !down;
                }
                if (up && !down)
                {
                    transform.Translate(Vector3.up * 0.5f * Time.deltaTime);
                    levitateTimer += Time.deltaTime;
                }
                else
                {
                    transform.Translate(Vector3.down * 0.5f * Time.deltaTime);
                    levitateTimer -= Time.deltaTime;
                }

            }
        }  

    }

}
