using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFXManager : MonoBehaviour
{
    public AudioClip StartBtn;
    public AudioClip Pause;
    public AudioClip Clicked;
    public AudioClip Released;
    public AudioClip Earthquake;
    public AudioClip Go;
    public AudioClip TimeUp;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void VoicePlay(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    public void VoicePlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    void StopVoices(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
