using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPotParent : MonoBehaviour
{


    private static DontDestroyPotParent instanceRef;

    void Awake()
    {
        if (instanceRef == null)
        {
            instanceRef = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

    }



        // Start is called before the first frame update
    //    void Start()
    //{
    //    DontDestroyOnLoad(transform.gameObject);
    //}

    //void Awake()
    //{

    //    DontDestroyOnLoad(transform.gameObject);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
