/* Yannick Renz
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrManager : MonoBehaviour
{
    public float musicVolume;
    private float musicTimer;
    private AudioSource audioSource;
    private bool clipSwitched;
    
    public AudioSource loopSource;
    private GameObject loopObject;

    private Exit exitScript; //prevent null reference bug between tutorial levels

    private GameObject laurel;
    private GameObject hardy;


    // Use this for initialization
    void Start()
    {
        Volumes.musicVol = PlayerPrefs.GetFloat("MusicVolume");
        Volumes.effectsVol = PlayerPrefs.GetFloat("EffectsVolume");
        updatePlayer();
        loopObject = loopSource.gameObject;
        audioSource = gameObject.GetComponent<AudioSource>();
        clipSwitched = false;
        loopSource.volume = 0;
        musicTimer = 10;
        musicVolume *= PlayerPrefs.GetFloat("MusicVolume");
        audioSource.volume = musicVolume;
        if (GameObject.Find("/Exit") != null)
            exitScript = GameObject.Find("/Exit").GetComponent<Exit>();
    }

    // Update is called once per frame
    void Update()
    {
        if(exitScript != null)
            if (exitScript.tutorial && (laurel == null || hardy == null))
                updatePlayer(); //preventing null reference in tutorial (!enemyLevels) levels -M
            if (!clipSwitched)
            {
                if (musicTimer > 0)
                {
                    loopSource.volume = 0;
                    musicTimer -= Time.deltaTime;
                }
                else
                {
                    loopSource.volume = musicVolume;
                    //audioSource.Pause();
                    audioSource.volume = 0;
                    clipSwitched = true;
                }
            }
    }

    void updatePlayer()
    {
        laurel = GameObject.Find("PlayerLaurel");
        hardy = GameObject.Find("PlayerHardy");
    }

    public void playDeathSound(AudioClip clip, float volume)
    {
        volume *= Volumes.musicVol;
        //AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        if (!clipSwitched) audioSource.PlayOneShot(clip, volume);
        else loopSource.PlayOneShot(clip, volume);
    }
}
