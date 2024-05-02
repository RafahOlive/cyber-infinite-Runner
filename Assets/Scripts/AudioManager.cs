using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    
    void Start()
    {
       audioSource = GetComponent<AudioSource>(); 
    }

    
    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play(); 
    }
}
