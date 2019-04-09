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

    public delegate void UpdateLeaderBoard(int score);
    public static event UpdateLeaderBoard updateLeaderBoard;


    public int pot4_score = 0;
    public int pot8_score = 30;
    public int pot12_score = 60;
    public int pot16_score = 120;
    public int beginLevitateScore = 0;

    public static int score = 0;
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
        SpadeController.stopGame += sendScore;

        ScoreTextMesh = GameObject.FindGameObjectWithTag("WatchTextMesh");
        //Debug.Log("watch score " + ScoreTextMesh);

    }

    void sendScore()
    {
        updateLeaderBoard(score);
    }

    //void pseudoScoreStart()
    //{

    //    Debug.Log("In pseudo score start");
        
    //    //transform.gameObject.SetActive(false);
    //    //GameObject.Find("TimerWarning").SetActive(true);
    //    score = 0;
    //}
	
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

        //if(delay>=startDelay)
        //{
            if (score == pot4_score)
            {
                
                //Debug.Log("4 POT");
                //GameObject.Find("TimerWarning").SetActive(false);
                //transform.gameObject.SetActive(true);
                activatePot("pot_4");
            }
        //}
        
        if (score >= pot8_score)
        {
            //Debug.Log("Pot8!!!!");
            activatePot("pot_8");      
        }
        if (score >= pot12_score)
        {
            activatePot("pot_12");
        }
        if (score >= pot16_score)
        {
            activatePot("pot_16");
        }
        //if(score == 30)
        //{
        //    fastenLevitate();
        //}
        //if ((score - beginLevitateScore) % 30 == 0)
        //{
        //    fastenLevitate();
        //}
    }
}
