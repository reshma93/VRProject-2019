//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.IO;
//using TMPro;
//public class LeaderboardFetch : MonoBehaviour
//{

//    public GameObject scoreTextMesh;
//    public GameObject nameTextMesh;
//    // Start is called before the first frame update
//    void Start()
//    {
        
//        LaserPointer.fetchScores += getScores;
      
//    }

//    void getScores()
//    {
//        Debug.Log("hello");
//        int[] ScoreList = new int[11];
//        string[] entry = new string[2];
//        int i = 0;
//        try
//        {
//            using (StreamReader sr = new StreamReader("Assets\\Scripts\\Leaderboard.txt"))
//            {
//                string line;

//                scoreTextMesh.GetComponent<TextMeshPro>().text = "";
//                nameTextMesh.GetComponent<TextMeshPro>().text = "";
//                while ((line = sr.ReadLine()) != null)
//                {
//                    entry = line.Split(' ');
//                    scoreTextMesh.GetComponent<TextMeshPro>().text += entry[1] + "\n";
//                    nameTextMesh.GetComponent<TextMeshPro>().text += entry[0] + "\n";
//                    //ScoreList[i] = Int32.Parse(line);
//                    //i++;
//                }
//              sr.Close();
//            }


            
//        }
//        catch (Exception e)
//        {
//            // Let the user know what went wrong.
//            Debug.Log("The file could not be read:");
//            Debug.Log(e.Message);
//        }
//    }

   
//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
