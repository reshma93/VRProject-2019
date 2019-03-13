using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAudio : MonoBehaviour
{
    public AudioSource m_MyAudioSource;
    private bool play=false;
    // Start is called before the first frame update
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();     
        m_MyAudioSource.Play();   
    } 
    void Update()
    {
        
    }
}
