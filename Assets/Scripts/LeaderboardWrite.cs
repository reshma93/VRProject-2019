using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LeaderboardWrite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScoreController.updateLeaderBoard += updateLeaderboard;
    }
    void updateLeaderboard(int score)
    {

        Debug.Log("come once");
        int[] ScoreList = new int[11];
        string[] NameList = new string[11];
        int i = 0;
        string[] linePairs = new string[2];
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader("Assets\\Scripts\\Leaderboard.txt"))
            {
                string line;

                // Read and display lines from the file until 
                // the end of the file is reached. 
                while ((line = sr.ReadLine()) != null)
                {
                    Debug.Log(line);
                    linePairs = line.Split(' ');
                    ScoreList[i] = Int32.Parse(linePairs[1]);
                    NameList[i] = linePairs[0];
                    i++;
                }

                ScoreList[i] = score;
                NameList[i] = "You";

                Array.Sort(ScoreList, NameList);
                Array.Reverse(ScoreList);
                Array.Reverse(NameList);

                sr.Close();
            }

        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }

        try
        {
            using (StreamWriter sw = new StreamWriter("Assets\\Scripts\\Leaderboard.txt"))
            {
                int j = 0;
                foreach (int s in ScoreList)
                {
                    if (j == 10)
                    {
                        break;
                    }
                    sw.WriteLine(NameList[j]+" "+s);
                    j++;
                }
                sw.Close();
            }

        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be written:");
            Debug.Log(e.Message);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
