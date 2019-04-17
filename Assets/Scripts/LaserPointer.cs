//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;


//public class LaserPointer : MonoBehaviour
//{

//    public delegate void FetchScores();
//    public static event FetchScores fetchScores;
//    // Start is called before the first frame update
//    private GameObject collidingObject;
//    private SteamVR_TrackedObject trackedObj;
//    public GameObject laserPrefab;
//    // 2
//    private GameObject laser;
//    // 3
//    private Transform laserTransform;
//    // 4
//    private Vector3 hitPoint;
//    public GameObject instructionsPlane;
//    public GameObject leaderboardPlane;
//    public LayerMask teleportMask;

//    public delegate void RestartPot();
//    public static event RestartPot restartPot;
//    void Start()
//    {
//        // 1
//        laser = Instantiate(laserPrefab);
//        // 2
//        laserTransform = laser.transform;
//        //leaderboardPlane.gameObject.SetActive(false);

//    }

//    // Update is called once per frame
//    void Update()
//    {

        
//        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
//        {
//            //Debug.Log(gameObject.name + " Grip Press");
//            RaycastHit hit;

//            // 2
//            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
//            {
//                //Debug.Log("inside");
//                hitPoint = hit.point;
//                ShowLaser(hit);
//                if (Controller.GetHairTriggerDown())
//                {
//                    //Debug.Log(hit.collider);
//                    if (hit.collider.tag == "Instructions_Rock")
//                    {
//                        //Call instructions
//                        //Debug.Log("Instructions");
//                        if (leaderboardPlane.activeSelf)
//                        {
//                            leaderboardPlane.gameObject.SetActive(false);
//                        }

//                        instructionsPlane.gameObject.SetActive(true);

//                    }
//                    if(hit.collider.tag == "Play_Rock")
//                    {
//                        //Debug.Log("PlayRock");
//                        //Call play scene
//                        SceneManager.LoadScene(2, LoadSceneMode.Single);
//                        //restartPot();
//                        ScoreController.score = 0;
                        
//                    }
//                    if (hit.collider.tag == "Leaderboard")
//                    {
//                        if(instructionsPlane.activeSelf)
//                        {
//                            instructionsPlane.gameObject.SetActive(false);
//                        }
//                        leaderboardPlane.gameObject.SetActive(true);
//                        fetchScores();
                        

//                    }
//                    if (hit.collider.tag == "Close")
//                    {
//                        //Debug.Log("Close");
//                        //Call play scene
//                        //SceneManager.LoadScene(2);
//                    }

//                }
//                //if (Controller.GetHairTriggerDown())
//                //{
//                //     Debug.Log("hair trigger got");
//                //    if (collidingObject)
//                //    {
//                //        Debug.Log(collidingObject.name + " Grip Press");
//                //        Debug.Log("inside if");
//                //        //potClicked();


//                //    }
//                //}

//                //Debug.Log(hitPoint + " hit point");
//            }

//        }
//        else // 3
//        {
//            laser.SetActive(false);
//        }
//    }
    

//    private SteamVR_Controller.Device Controller
//    {
//        get { return SteamVR_Controller.Input((int)trackedObj.index); }
//    }

//    void Awake()
//    {
//        trackedObj = GetComponent<SteamVR_TrackedObject>();
//    }
//    private void ShowLaser(RaycastHit hit)
//    {
//        // 1
//        laser.SetActive(true);
//        // 2
//        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
//        // 3
//        laserTransform.LookAt(hitPoint);
//        // 4
//        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
//            hit.distance);
//    }

//    //private void SetCollidingObject(Collider col)
//    //{
//    //    if (collidingObject || !col.GetComponent<Rigidbody>())
//    //    {
//    //        return;
//    //    }
//    //    Debug.Log(col.gameObject + " col.gameObject");
//    //    collidingObject = col.gameObject;
//    //}
//    //public void OnTriggerEnter(Collider other)
//    //{
//    //    Debug.Log("Ontriggerenter");
//    //    SetCollidingObject(other);

//    //}
//    //public void OnTriggerStay(Collider other)
//    //{
//    //    //Debug.Log("set collidingr");
//    //    SetCollidingObject(other);
//    //}
//    //public void OnTriggerExit(Collider other)
//    //{
//    //    if (!collidingObject)
//    //    {
//    //        return;
//    //    }

//    //    collidingObject = null;
//    //}
//}

