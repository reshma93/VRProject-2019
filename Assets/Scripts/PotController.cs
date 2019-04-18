using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour
{

    private float activeTimer;
    private float levitateTimer;
    private float deadStateTimer = 15;
    private float fastenTimer = 60f;
    private float groupLevStartTimer = 60f;

    private float levitateDuration = 10;
    private float levitateSpeed = 0.1f;
    private float levitateDistance;

    private bool isPotClicked;
    private bool isColourSetToRed;
    public bool levitateFlag = false;
    private bool gameOver = false;
    public bool alreadyKilled = false;
    private bool up = true;
    private bool down = false;
    public bool isPotFrozen = false;
    private bool scaleFlag = false;

    private int coinsColour;
    public int coinType;


    public Material gold;
    public Material silver;
    private Material chosenMaterial;
    public Material frozenMaterial;
    public Material originalMaterial;

    private Transform coins;
    private Transform pot;
    private Transform potPrefab;
    private string tag;

    private Animator potAnim;

    public delegate void IncreaseFrozenPotsCount(GameObject pot);
    public static event IncreaseFrozenPotsCount increaseFrozenPotsCount;

    

    // Start is called before the first frame update
    void Start()
    {
        ScoreController.activatePot += activatePot;
        transform.gameObject.SetActive(false);
        Debug.Log("Starting Pot Controller"+transform);

        potPrefab = transform;
       
       
        coins = transform.GetChild(1);
        pot = transform.GetChild(0);
        tag = transform.tag;
        coins.gameObject.SetActive(false);
        setInitialStates();

       
        pot.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = originalMaterial;


        ScoreController.beginLevitate += levitatePot;
        SpadeController.stopGame += finishGame;
        ScoreController.fastenLevitate += calculateLeviatateSpeed;

        gameOver = false;

        levitateDistance = levitateSpeed * levitateDuration;

 
        potAnim=transform.GetComponent<Animator>();

        GameObject.FindGameObjectWithTag("PotParent").SetActive(true);

        Debug.Log("active " + transform.gameObject.activeSelf);

    }



   

    void calculateLeviatateSpeed()
    {
        levitateSpeed += 0.05f;

        Debug.Log("LSPEED= "+levitateSpeed);
        levitateDuration = levitateDistance / (levitateSpeed);

       
    }
    public void finishGame()
    {
        gameOver = true;
        //transform.gameObject.SetActive(false);

    }
     public void levitatePot()
    {
        levitateFlag = true;
    }



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
        scaleFlag = false;

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


                coins.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = chosenMaterial;
                coins.GetChild(1).gameObject.GetComponent<Renderer>().sharedMaterial = chosenMaterial;
                isColourSetToRed = true;
            }
            if (isColourSetToRed && deadStateTimer > 0)
            {
                deadStateTimer -= Time.deltaTime;

            }
            if (deadStateTimer <= 6 && deadStateTimer > 0)
            {

                
                if(!scaleFlag)
                {
                    scaleFlag = true;
                    
                    if (!isPotClicked)
                    {
                        potAnim.Play("PotBlink", 0, 0);

                    }
                }

            }

            if (deadStateTimer < 0)
            {

                  
                if (!isPotClicked)
                {
    
                    potAnim.Play("NoAnim", 0, 0);
                    coins.gameObject.SetActive(false);
                    pot.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = frozenMaterial;

                    if (!alreadyKilled && transform.gameObject.activeSelf)
                    {
                        
                        increaseFrozenPotsCount(transform.gameObject);
                        alreadyKilled = true;
                        
                    }                   
                }
            }
           
            if (levitateFlag)
            {
                groupLevStartTimer -= Time.deltaTime;
                if (levitateTimer >= levitateDuration || levitateTimer <= -5)
                {
                    up = !up;
                    down = !down;
                }
                if (up && !down)
                {
                    transform.Translate(Vector3.up * levitateSpeed * Time.deltaTime);
                    levitateTimer += Time.deltaTime;
                }
                else
                {
                    transform.Translate(Vector3.down * levitateSpeed * Time.deltaTime);
                    levitateTimer -= Time.deltaTime;
                }

                fastenTimer -= Time.deltaTime;

                if (fastenTimer <= 0)
                {
                    fastenTimer = 60f;
                    calculateLeviatateSpeed();
                    
                }

                if(groupLevStartTimer <= 0)
                {
                    
                    if(transform.parent.tag.Equals("Column1") || transform.parent.tag.Equals("Column4"))
                    {
                        up = true;
                        down = false;
                    }
                    if (transform.parent.tag.Equals("Column2") || transform.parent.tag.Equals("Column3"))
                    {
                        up = false;
                        down = true;
                    }

                    groupLevStartTimer = 1 / 0f;
                }


            }
        }

    }


}
