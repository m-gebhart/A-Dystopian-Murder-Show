/* Yannick Renz
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrManagerDeath : MonoBehaviour
{
    public float musicVolume;
    private AudioSource audioSource;

    public AudioSource loopSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        loopSource.volume = 0;
        audioSource.volume = 0;
        musicVolume = Volumes.musicVol;
    }

    void Update()
    {
        if (loopSource.volume < musicVolume && PlayerPrefs.GetFloat("MusicVolume") != 0f) loopSource.volume += 0.002f;
    }
}
