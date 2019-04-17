using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour {
    public delegate void BeginLevitate();
    public static event BeginLevitate beginLevitate;

    public delegate void StopLevitate();
    public static event StopLevitate stopLevitate;


    public delegate void FastenLevitate();
    public static event FastenLevitate fastenLevitate;

    public delegate void ActivatePot(string pot_tag);
    public static event ActivatePot activatePot;


    public int pot4_score = 0;
    public int pot8_score = 30;
    public int pot12_score = 60;
    public int pot16_score = 120;
    public int beginLevitateScore = 0;

    public int score = 0;
    public float startDelay = 10f;
    private float delay=0;

    private GameObject ScoreTextMesh;
    // Use this for initialization
    void Start () {

        //pseudoScoreStart();

        //LaserPointer.restartPot += pseudoScoreStart;

        SpadeController.increaseScore += IncrementScore;
        SpadeController.decreaseScore += DecrementScore;
        score = 0;
        

        ScoreTextMesh = GameObject.FindGameObjectWithTag("WatchTextMesh");
        //Debug.Log("watch score " + ScoreTextMesh);

    }



	
    void IncrementScore()
    {
        score+=3;
    }

    void DecrementScore()
    {
        score -= 1;
    }
    // Update is called once per frame
    void Update () {
        //transform.gameObject.GetComponent<TextMeshPro>().text = "Score : " + score;
        //ScoreTextMesh.GetComponent<TextMesh>().text = score.ToString();
        transform.gameObject.GetComponent<TextMesh>().text = score.ToString();
        if (score>=beginLevitateScore)
        {
            beginLevitate();


        }

        delay += Time.deltaTime;

        if (score == pot4_score)
        {
                
            Debug.Log("4 POT");

            //activatePot("pot_4");
            //Debug.Log(GameObject.FindGameObjectsWithTag("pot_4"));
            //foreach(GameObject g in GameObject.FindGameObjectsWithTag("pot_4"))
            //{
            //    Debug.Log("activating" + g);
            //    g.SetActive(true);
            //}

            GameObject.Find("/Pot_Parent/Column2/pot_final (4)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column3/pot_final (7)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column3/pot_final (9)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column2/pot_final (10)").SetActive(true);


        }
      
        if (score >= pot8_score)
        {
            Debug.Log("8 POT");
            GameObject.Find("/Pot_Parent/Column1/pot_final (6)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column1/pot_final (8)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column4/pot_final (5)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column4/pot_final (11)").SetActive(true);
            //activatePot("pot_8");      
        }
        if (score >= pot12_score)
        {
            GameObject.Find("/Pot_Parent/Column2/pot_final (1)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column3/pot_final (2)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column2/pot_final (15)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column3/pot_final (13)").SetActive(true);
            //Debug.Log("12 POT");
            //activatePot("pot_12");
        }
        if (score >= pot16_score)
        {
            GameObject.Find("/Pot_Parent/Column1/pot_final").SetActive(true);
            GameObject.Find("/Pot_Parent/Column4/pot_final (3)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column4/pot_final (14)").SetActive(true);
            GameObject.Find("/Pot_Parent/Column1/pot_final (12)").SetActive(true);
            //activatePot("pot_16");
        }

    }
}
