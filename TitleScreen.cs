/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    private bool titleScreen = true;

    void Start()
    {
        PlayerPrefs.SetFloat("MusicVolume", 1f); //Setting back Musics and Volume to 1 at every start
        Volumes.musicVol = 1f;
        PlayerPrefs.SetFloat("EffectsVolume", 1f);
        Volumes.effectsVol = 1f;
        SceneManager.LoadScene(17, LoadSceneMode.Additive); //Loading Music
        PlayerPrefs.SetInt("LastScn", 0);
    }

    void Update()
    {
        if (Input.anyKey && titleScreen)
        {
            titleScreen = false;
            Destroy(GameObject.Find("/Canvas"));
            Destroy(GameObject.Find("Directional Light Title"));
            GameObject.Find("Main Camera Title").SetActive(false);
            CheckGamePadInput();
            SceneManager.LoadScene(14, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(0);
        }
    }

    void CheckGamePadInput()
    {
        if (InputManager.LeftInput() 
            || InputManager.RightInput() 
            || InputManager.DownInput() 
            || InputManager.UpInput() 
            || Input.GetButtonDown("XBoxA") 
            || Input.GetButtonDown("XBoxB") 
            || Input.GetButtonDown("XBoxX") 
            || Input.GetButtonDown("XBoxY"))
        GamePadInUse.gamePadInUse = true;
    }

}
