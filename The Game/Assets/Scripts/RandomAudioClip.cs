using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioClip : MonoBehaviour
{

    public List<AudioClip> clips;
    private AudioSource audSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audSource = GetComponent<AudioSource>();
        audSource.clip = clips[Random.Range(0, clips.Count)];
        audSource.Play(); 
    }

}
