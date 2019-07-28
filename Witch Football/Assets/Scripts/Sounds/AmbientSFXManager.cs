using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSFXManager : MonoBehaviour
{
    // Seasons
    
    // Weather
    
    // Day & Night
    public AudioClip Ghostly;
    public AudioClip Lava;
    public AudioClip Night;
    public AudioClip Rainy;
    public AudioClip Snowy;
    public AudioClip Stormy;
    public AudioClip Sunny;
    public AudioClip Windy;
    private AudioSource audioSource;
    
    public float minTimeBetween = 5;
    public float maxTimeBetween = 20;
    private float TimeBetween = 10;
     

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        TimeBetween -= Time.deltaTime;
    }
    void LateUpdate() {
        if(!audioSource.isPlaying && TimeBetween <= 0){
            // <Edit later>
            Play(Sunny);
            TimeBetween = Random.Range(minTimeBetween, maxTimeBetween);
        }
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
