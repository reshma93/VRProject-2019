using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntoSceneManagerScript : MonoBehaviour
{
    float timer = 0;
    public float switchtime = 6;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>switchtime)
        {
            SceneManager.LoadScene(1);
        }
    }
}
