using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBGM : MonoBehaviour
{
    public AudioClip[] soundtracks;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(!audioSource.playOnAwake){
            RandomPlay();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {
        if(!audioSource.isPlaying){
            RandomPlay();
        }
    }

    void RandomPlay(){
        audioSource.clip = soundtracks[Random.Range(0, soundtracks.Length)];
        audioSource.Play();
    }
}
