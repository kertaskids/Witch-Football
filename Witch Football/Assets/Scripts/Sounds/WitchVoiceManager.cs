using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchVoiceManager : MonoBehaviour
{
    //Activity
    public AudioClip Shooting;
    public AudioClip Passing;
    public AudioClip Tackling;
    public AudioClip Controlling;
    public AudioClip Scoring;
    public AudioClip Falling;
    public AudioClip Tackled;

    //Expression
    public AudioClip Happy;
    public AudioClip Sad;
    public AudioClip Laugh;
    public AudioClip Cry;
    public AudioClip Boring;

    //Skills
    public AudioClip LightDamage;
    public AudioClip HeavyDamage;
    public AudioClip LightHeal;
    public AudioClip HeavyHeal;
    public AudioClip LightPower;
    public AudioClip HeavyPower;
    public AudioClip LightSpeed;
    public AudioClip HeavySpeed;

    private AudioSource audioSource;
    
    //Play, SetVolume
    //Voice Categories: 
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
