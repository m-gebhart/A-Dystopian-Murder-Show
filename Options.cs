/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    private Button lowerMusic;
    private Button greaterMusic;
    private Button lowerEffects;
    private Button greaterEffects;
    private Text musicVolText;
    private Text effectsVolText;

    private AudioSource audioSource;
    public AudioClip testSound;

    void Start()
    {
        SetComponents();
    }

    void SetComponents()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(16));

        lowerMusic = GameObject.Find("/Canvas/LowerMusicVolume").GetComponent<Button>();
        lowerMusic.onClick.AddListener(LowerMusicVolume);

        greaterMusic = GameObject.Find("/Canvas/GreaterMusicVolume").GetComponent<Button>();
        greaterMusic.onClick.AddListener(GreaterMusicVolume);

        lowerEffects = GameObject.Find("/Canvas/LowerEffectsVolume").GetComponent<Button>();
        lowerEffects.onClick.AddListener(LowerEffectsVolume);

        greaterEffects = GameObject.Find("/Canvas/GreaterEffectsVolume").GetComponent<Button>();
        greaterEffects.onClick.AddListener(GreaterEffectsVolume);

        musicVolText = GameObject.Find("/Canvas/MusicVolume").GetComponent<Text>();
        musicVolText.text = (Mathf.Round(PlayerPrefs.GetFloat("MusicVolume") * 10f)).ToString(); ;

        effectsVolText = GameObject.Find("/Canvas/EffectsVolume").GetComponent<Text>();
        effectsVolText.text = (Mathf.Round(PlayerPrefs.GetFloat("EffectsVolume") * 10f)).ToString();

        audioSource = this.gameObject.GetComponent<AudioSource>();
        UpdateEffectsVolume(0f, false);
        UpdateMusicVolume(0f, false);
    }

    public void LowerMusicVolume()
    {
        if (PlayerPrefs.GetFloat("MusicVolume") > 0.1f)
            UpdateMusicVolume(-0.1f, true);
    }

    public void GreaterMusicVolume()
    {
        if (PlayerPrefs.GetFloat("MusicVolume") < 1f)
            UpdateMusicVolume(0.1f, true);
    }

    public void UpdateMusicVolume(float summand, bool restart)
    {
        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") + summand);
        Volumes.musicVol = PlayerPrefs.GetFloat("MusicVolume");
        musicVolText.text = (Mathf.Round(Volumes.musicVol * 10f)).ToString();
        if (restart)
        {
            SceneManager.UnloadSceneAsync(17);
            SceneManager.LoadScene(17, LoadSceneMode.Additive);
        }
    }

    public void LowerEffectsVolume()
    {
        if (PlayerPrefs.GetFloat("EffectsVolume") > 0.1f)
            UpdateEffectsVolume(-0.1f, true);
    }

    public void GreaterEffectsVolume()
    {
        if (PlayerPrefs.GetFloat("EffectsVolume") < 1f)
            UpdateEffectsVolume(0.1f, true);
    }

    void UpdateEffectsVolume(float summand, bool playSound)
    {
        PlayerPrefs.SetFloat("EffectsVolume", PlayerPrefs.GetFloat("EffectsVolume") + summand);
        effectsVolText.text = (Mathf.Round(PlayerPrefs.GetFloat("EffectsVolume") * 10f)).ToString();
        Volumes.effectsVol = PlayerPrefs.GetFloat("EffectsVolume");
        if (playSound)
            audioSource.PlayOneShot(testSound, Volumes.effectsVol);
    }
}