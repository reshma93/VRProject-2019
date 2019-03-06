using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour {
    public delegate void BeginLevitate();
    public static event BeginLevitate beginLevitate;

    public delegate void StopLevitate();
    public static event StopLevitate stopLevitate;

    public delegate void ActivatePot(string pot_tag);
    public static event ActivatePot activatePot;

    public int beginLevitateScore=45;
    public int pot4_score = 0;
    public int pot8_score = 10;
    public int pot12_score = 20;
    public int pot16_score = 40;
    // Use this for initialization
    void Start () {
        SpadeController.increaseScore += IncrementScore;
	}
	private int score =0;
    void IncrementScore()
    {
        score++;
    }
	// Update is called once per frame
	void Update () {
        transform.gameObject.GetComponent<TextMeshPro>().text = "Score : " + score;
        if(score==beginLevitateScore)
        {
            beginLevitate();
        }

        if (score == pot4_score)
        {
            activatePot("pot_4");
        }
        if (score == pot8_score)
        {
            activatePot("pot_8");
            
        }
        if (score == pot12_score)
        {
            activatePot("pot_12");
            
        }
        if (score == pot16_score)
        {
            activatePot("pot_16");
            
        }
    }
}
