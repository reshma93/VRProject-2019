using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SpadeController : MonoBehaviour
{

    public Image black; 
    private GameObject collidingObject;
    private SteamVR_Controller.Device device;
    private static HashSet<GameObject> frozenPotSet = new HashSet<GameObject>();
    private SteamVR_TrackedObject trackedObj;

    public delegate void IncreaseScore();
    public static event IncreaseScore increaseScore;

    public delegate void DecreaseScore();
    public static event DecreaseScore decreaseScore;

    public delegate void GameOverEvent();
    public static event GameOverEvent gameOverEvent;

    public delegate void StopGame();
    public static event StopGame stopGame;

    public RuntimeAnimatorController goldAnimate;
    public RuntimeAnimatorController silverAnimate;

    private bool gameOver = false;
    private bool hapticFlag = false;
    private bool isGold = true;

    private int frozenPotCount = 4;
    private int numOfClicks_layer = 10;
    private int currentLayerNumber = 1;
    private int clickCounter = 0;
    private int wrongPotCount = 0;
    private int wrongPotAudioNumber;

    private float gameOverTimer = 2f;
    private float jackTimer = 4.2f;
    private float activeTimer = 1f;

    private Animator coins_anim;
    private Animator wrong_pot_anim;
    private Animator leftHand_anim;
    private Animator rightHand_anim;
    public Animator blackFadeAnim;

    public AudioSource BGM;
    private AudioSource collectCoinAudio;
    private AudioSource wrongPotAudio;

    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public AudioClip clip4;


    public GameObject gameOverAudio;
    public GameObject Jack;
    public GameObject MainMenuButton;
    private GameObject TreasureChest;
    private GameObject CameraRigHead;
    private GameObject Blackout_Camera;
    private GameObject ScoreTextMesh;
    private GameObject PotParent;
    //public GameObject collectCoinAudio;

    private IEnumerator coroutine;
    private IEnumerator coroutine_pot;
    private IEnumerator coroutine_collectCoins;
    private IEnumerator coroutine_handAnim;

    // Start is called before the first frame update
    void Start()
    {
        //check here for which version
        PotController.increaseFrozenPotsCount += IncrementPotCount;
        ScoreController.activatePot += updateFrozenPotCount;

        leftHand_anim = trackedObj.transform.GetChild(2).GetComponent<Animator>();
        rightHand_anim = trackedObj.transform.GetChild(2).GetComponent<Animator>();


        TreasureChest = GameObject.FindGameObjectWithTag("TreasureChest");
        ScoreTextMesh = GameObject.FindGameObjectWithTag("WatchTextMesh");
    

        CameraRigHead = GameObject.FindGameObjectWithTag("MainCamera");
        Blackout_Camera = GameObject.FindGameObjectWithTag("Blackout_Camera");

        wrongPotAudio = GameObject.FindGameObjectWithTag("WrongPotAudio").GetComponent<AudioSource>();

        //blackFadeAnim = GameObject.FindGameObjectWithTag("BlackImg").GetComponent<Animator>();

        

    }
    public static void IncrementPotCount(GameObject pot)
    {
     
         frozenPotSet.Add(pot);
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
        string layerName = "";
        collectCoinAudio = collidingObject.GetComponent<AudioSource>();
        if (!gameOver)
        {
            //for (float i = 0; i < 60; i += Time.deltaTime)
            //{
            //    //Debug.Log(i);
            //    device.TriggerHapticPulse(3500);
            //    // yield return null; //every single frame for the duration of "length" you will vibrate at "strength" amount
            //}
            if (!collidingObject.GetComponent<PotController>().alreadyKilled)
            {
                Transform coins = collidingObject.transform.GetChild(1);
                if(coins.gameObject.activeSelf)
                {

                    coins_anim = coins.GetComponent<Animator>();
                    wrong_pot_anim = collidingObject.GetComponent<Animator>();
                    wrong_pot_anim.Play("NoAnim", 0, 0);
                    int coinType = collidingObject.GetComponent<PotController>().coinType;
                    if ((coinType == 0 && trackedObj.name.Equals("Controller (left)")) || (coinType == 1 && trackedObj.name.Equals("Controller (right)")))
                    {

                        clickCounter++;
                        if(clickCounter == numOfClicks_layer)
                        {
                            if(isGold)
                            {
                                layerName = "Gold_layer_" + currentLayerNumber.ToString();
                            }

                            else
                            {
                                layerName = "Silver_layer_" + currentLayerNumber.ToString();
                            }

                            TreasureChest.transform.Find(layerName).gameObject.SetActive(true);
                            //.enable = true;
                            currentLayerNumber++;
                            isGold = !isGold;
                            clickCounter = 0;
                        }

                        



                        collectCoinAudio.enabled = true;
                        coroutine_collectCoins = collectCoinsTimer(collectCoinAudio);
                        StartCoroutine(coroutine_collectCoins);

                        collidingObject.GetComponent<PotController>().setInitialStates();

                        if(coinType ==0)
                        {
                            //Debug.Log(coins_anim.runtimeAnimatorController);
                            coins_anim.runtimeAnimatorController = goldAnimate;
                        }
                        else
                        {
                            //Debug.Log(coins_anim.runtimeAnimatorController);
                            coins_anim.runtimeAnimatorController = silverAnimate;
                        }

                        coins_anim.enabled= true;
                        //Put delayyyyy!

                        coroutine = delayTimer(coins);
                        StartCoroutine(coroutine);                        
                        
                        Controller.TriggerHapticPulse(1000);
                        increaseScore();
                    }
                    else if ((coinType == 1 && trackedObj.name.Equals("Controller (left)")) || (coinType == 0 && trackedObj.name.Equals("Controller (right)")))
                    {
                        // wrong_pot_anim.enabled = true;
                        Debug.Log("Wrong pot");
                        wrong_pot_anim.Play("WrongPot", 0, 0);
                        coroutine_pot = delayTimerForPot(wrong_pot_anim);
                        StartCoroutine(coroutine_pot);
                        decreaseScore();

                        wrongPotCount++;

                        if (wrongPotCount % 3 == 0)
                        {
                            wrongPotAudioNumber = Random.Range(1, 5);
                            if (wrongPotAudioNumber == 1)
                            {
                                wrongPotAudio.clip = clip1;
                            }
                            else if (wrongPotAudioNumber == 2)
                            {
                                wrongPotAudio.clip = clip2;
                            }
                            else if (wrongPotAudioNumber == 3)
                            {
                                wrongPotAudio.clip = clip3;
                            }
                            else {
                                wrongPotAudio.clip = clip4;
                            }

                            wrongPotAudio.Play();
                        }
                    }
                }
            }
        }
        //else
        //{
        //    SceneManager.LoadScene(1, LoadSceneMode.Single);
        //}
    }
    IEnumerator FadeIn()
    {
        blackFadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(0);
    }

    IEnumerator delayTimer(Transform coins)
    {
        //print(Time.time);
        yield return new WaitForSeconds(1f);
        coins_anim = coins.GetComponent<Animator>();
        coins.gameObject.SetActive(false);
        coins_anim.enabled = false;
        //print(Time.time);
    }
    IEnumerator delayTimerForPot(Animator potAnim)
    {
        //print(Time.time);
        yield return new WaitForSeconds(0.5f);

        //potAnim.StopPlayback();
        //potAnim.enabled = false;
        //print(Time.time);
    }

    IEnumerator collectCoinsTimer(AudioSource collectCoins)
    {
        //print(Time.time);
        yield return new WaitForSeconds(0.5f);
        collectCoins.enabled = false;
        //print(Time.time);
    }

   IEnumerator handAnimDelayTimer(Animator hand)
    {
        
        yield return new WaitForSeconds(0.25f);
        hand.StopPlayback();
        //hand.enabled = false;
        
    }
    void Update()
    {
       

        if (frozenPotSet.Count >= frozenPotCount)
        {
            
            if(!gameOver && trackedObj.name.Equals("Controller (left)"))
            {
                stopGame();
               
            }
            gameOver = true;
           

            Jack.SetActive(true);
            jackTimer -= Time.deltaTime;
            if (jackTimer>=0)
            {
                Jack.transform.Translate(Vector3.back * 9f * Time.deltaTime);
            }
            else
            {
                //StartCoroutine(FadeIn());

                SceneManager.LoadScene(3);   
            }
                
            



            BGM.GetComponent<AudioSource>().Stop();
            //MainMenuButton.SetActive(true);
            gameOverAudio.SetActive(true);

           
        }
        if (Controller.GetHairTriggerDown())
        {
            // device = SteamVR_Controller.Input((int)trackedObj.index);

            if (trackedObj.name.Equals("Controller (left)"))
            {

                //leftHand_anim.enabled = true;
                leftHand_anim.Play("Take 01", 0, 0);
                coroutine_handAnim = handAnimDelayTimer(leftHand_anim);
                StartCoroutine(coroutine_handAnim);

            }

            if (trackedObj.name.Equals("Controller (right)"))
            {

                //rightHand_anim.enabled = true;
                rightHand_anim.Play("Take 01", 0, 0);
                coroutine_handAnim = handAnimDelayTimer(rightHand_anim);
                StartCoroutine(coroutine_handAnim);

            }


            if (collidingObject)
            {
              //  Debug.Log("inside if");
          
                potClicked();
               
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
        //Debug.Log("Ontriggerenter");
        SetCollidingObject(other);

        //hapticFlag = true;

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
        //hapticFlag = false;
    }
}
