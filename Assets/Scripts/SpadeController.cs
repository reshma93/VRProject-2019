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

    private int frozenPotCount;
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
   

    private IEnumerator coroutine;
    private IEnumerator coroutine_pot;
    private IEnumerator coroutine_collectCoins;
    private IEnumerator coroutine_handAnim;

    AsyncOperation scene;

    // Start is called before the first frame update
    void Start()
    {
        
        PotController.increaseFrozenPotsCount += IncrementPotCount;
        ScoreController.activatePot += updateFrozenPotCount;

        leftHand_anim = trackedObj.transform.GetChild(2).GetComponent<Animator>();
        rightHand_anim = trackedObj.transform.GetChild(2).GetComponent<Animator>();


        TreasureChest = GameObject.FindGameObjectWithTag("TreasureChest");
        ScoreTextMesh = GameObject.FindGameObjectWithTag("WatchTextMesh");
    

        CameraRigHead = GameObject.FindGameObjectWithTag("MainCamera");
        Blackout_Camera = GameObject.FindGameObjectWithTag("Blackout_Camera");

        wrongPotAudio = GameObject.FindGameObjectWithTag("WrongPotAudio").GetComponent<AudioSource>();

        

        frozenPotSet.Clear();
        frozenPotCount = 4;

        scene = SceneManager.LoadSceneAsync("House", LoadSceneMode.Single);
        scene.allowSceneActivation = false;

       
       

    }
    public static void IncrementPotCount(GameObject pot)
    {
     
         frozenPotSet.Add(pot);
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
        string layerName = "";
        collectCoinAudio = collidingObject.GetComponent<AudioSource>();
        if (!gameOver)
        {

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
                            
                            coins_anim.runtimeAnimatorController = goldAnimate;
                        }
                        else
                        {
                            
                            coins_anim.runtimeAnimatorController = silverAnimate;
                        }

                        coins_anim.enabled= true;
                        

                        coroutine = delayTimer(coins);
                        StartCoroutine(coroutine);                        
                        
                        Controller.TriggerHapticPulse(1000);
                        increaseScore();
                    }
                    else if ((coinType == 1 && trackedObj.name.Equals("Controller (left)")) || (coinType == 0 && trackedObj.name.Equals("Controller (right)")))
                    {
                        
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

    }

    IEnumerator delayTimer(Transform coins)
    {

        yield return new WaitForSeconds(1f);
        coins_anim = coins.GetComponent<Animator>();
        coins.gameObject.SetActive(false);
        coins_anim.enabled = false;

    }
    IEnumerator delayTimerForPot(Animator potAnim)
    {
        
        yield return new WaitForSeconds(0.5f);

    }

    IEnumerator collectCoinsTimer(AudioSource collectCoins)
    {

        yield return new WaitForSeconds(0.5f);
        collectCoins.enabled = false;
 
    }

   IEnumerator handAnimDelayTimer(Animator hand)
    {
        
        yield return new WaitForSeconds(0.25f);
        hand.StopPlayback();
  
        
    }
    void Update()
    {
       

        if (frozenPotSet.Count >= frozenPotCount)
        {
            
            if (!gameOver)
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
              
                //SceneManager.LoadScene(3);
                scene.allowSceneActivation = true;
            }

            BGM.GetComponent<AudioSource>().Stop();

            gameOverAudio.SetActive(true);

           
        }
        if (Controller.GetHairTriggerDown())
        {
           
            if (trackedObj.name.Equals("Controller (left)"))
            {

                leftHand_anim.Play("Take 01", 0, 0);
                coroutine_handAnim = handAnimDelayTimer(leftHand_anim);
                StartCoroutine(coroutine_handAnim);

            }

            if (trackedObj.name.Equals("Controller (right)"))
            {

        
                rightHand_anim.Play("Take 01", 0, 0);
                coroutine_handAnim = handAnimDelayTimer(rightHand_anim);
                StartCoroutine(coroutine_handAnim);

            }


            if (collidingObject)
            {
              
          
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
