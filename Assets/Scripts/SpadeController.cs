using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpadeController : MonoBehaviour
{

    private GameObject collidingObject;
    private SteamVR_TrackedObject trackedObj;

    public delegate void IncreaseScore();
    public static event IncreaseScore increaseScore;

    public delegate void DecreaseScore();
    public static event DecreaseScore decreaseScore;

    public delegate void GameOverEvent();
    public static event GameOverEvent gameOverEvent;

    public delegate void StopGame();
    public static event StopGame stopGame;

    private SteamVR_Controller.Device device;
    private static HashSet<GameObject> frozenPotSet = new HashSet<GameObject>();
    private bool gameOver = false;
    private bool hapticFlag = false;
    private int frozenPotCount = 4;
    private float gameOverTimer;
    public GameObject MainMenuButton;


    public AudioSource BGM;

    public GameObject gameOverAudio;
    // Start is called before the first frame update
    void Start()
    {
        PotController.increaseFrozenPotsCount += IncrementPotCount;
        // Debug.Log("blah"+GameObject.Find("gover"));
        //GameObject.Find("gover").SetActive(false);
        ScoreController.activatePot += updateFrozenPotCount;
    }
    public static void IncrementPotCount(GameObject pot)
    {
        //Debug.Log("In increment pot count");
         frozenPotSet.Add(pot);
        //Debug.Log("Called from " + pot);
        //Debug.Log("Killed pots - " + frozenPots);
    }

    public void updateFrozenPotCount(string tag)
    {
        if (tag.Equals("pot_8"))
        {
            //Debug.Log(frozenPotCount);
            frozenPotCount = 5;
            //Debug.Log(frozenPotCount);
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
            if (!collidingObject.GetComponent<PotController>().alreadyKilled)
            {
                Transform coins = collidingObject.transform.GetChild(1);
                if(coins.gameObject.activeSelf)
                {
                    int coinType = collidingObject.GetComponent<PotController>().coinType;
                    if ((coinType == 0 && trackedObj.name.Equals("Controller (left)")) || (coinType == 1 && trackedObj.name.Equals("Controller (right)")))
                    {
                        coins.gameObject.SetActive(false);
                        collidingObject.GetComponent<PotController>().setInitialStates();
                        Controller.TriggerHapticPulse(1000);
                        increaseScore();
                    }
                    else if ((coinType == 1 && trackedObj.name.Equals("Controller (left)")) || (coinType == 0 && trackedObj.name.Equals("Controller (right)")))
                    {
                        decreaseScore();
                    }
                }

            }
        }
        else
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }

    void Update()
    {
        
        //Debug.Log("Spade controller running");
        //GameObject.Find("gover").SetActive(false);
        if (frozenPotSet.Count >= frozenPotCount)
        {
            //Debug.Log("Enter here.....................!!!!");
            stopGame();
            gameOver = true;
            GameObject.Find("gover").GetComponent<TextMeshPro>().text = "GAME OVER";
            BGM.GetComponent<AudioSource>().Stop();
            MainMenuButton.SetActive(true);
            gameOverAudio.SetActive(true);

           
        }
        if (Controller.GetHairTriggerDown())
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);
            //  Debug.Log("hair trigger got");

            if (collidingObject)
            {
              //  Debug.Log("inside if");
              if(hapticFlag)
                {
                    device.TriggerHapticPulse(3000);
                }
                potClicked();

            }
        }
        //  Debug.Log("froze " + frozenPots);

        //if (gameOver)
        //{
        //    gameOverTimer += Time.deltaTime;
        //    if (gameOverTimer >= 10)
        //    {
        //        gameOver = false;
        //        gameOverEvent();
        //        SceneManager.LoadScene(1);
        //    }
        //}
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

        hapticFlag = true;

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
        hapticFlag = false;
    }
}
