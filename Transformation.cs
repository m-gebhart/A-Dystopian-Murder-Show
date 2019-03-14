/* Yannick Renz
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour {

    public float minPitch;
    public float maxPitch;

    private AudioSource audioSource;
    
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume *= Volumes.effectsVol;
        audioSource.Play();
	}
}
