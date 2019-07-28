using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSFXManager : MonoBehaviour
{
    public AudioClip Kicked;
    public AudioClip Controlled;
    public AudioClip Falling;
    public AudioClip Goal;
    public AudioClip HitingPole;
    public AudioClip Raged;
    public AudioClip DribbleNormal;
    public AudioClip DribbleSpring;
    public AudioClip DribbleSummer;
    public AudioClip DribbleAutumn;
    public AudioClip DribbleWinter;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    public void PlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    void StopSounds(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
