using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSFXManager : MonoBehaviour
{
    // Seasons
    public AudioClip Lava;
    public AudioClip Snow;
    public AudioClip Snowing;
    public AudioClip SteamUp;

    // Weather
    public AudioClip Wind;
    public AudioClip Storm;
    public AudioClip Rain;
    public AudioClip HeavyThunder;
    public AudioClip RainAndThunder;
    public AudioClip ThunderStorm;

    // Day & Night
    public AudioClip Ghostly;
    public AudioClip Night;
    public AudioClip Sunny;
    public AudioClip NightFrog;
    
    // Other
    public AudioClip Frogs;
    public AudioClip Crickets;
    public AudioClip BirdsCricketsDogs;
    public AudioClip Wolves;
    public AudioClip Raven;


    private AudioSource audioSource;
    
    public float minTimeBetween = 5;
    public float maxTimeBetween = 20;
    private float TimeBetween = 10;
     

    void Start(){
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
