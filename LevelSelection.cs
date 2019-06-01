/* Yannick Renz, Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelection : MonoBehaviour {

    /*
     * Here is a list of all the level names, which are used to store high scores and determine whether they are unlocked:
     * 
     * Level 1: Episode1_v2
     * Level 2: Episode2_v2
     * Level 3: Episode3_v2
     * Level 4: Episode4_v2
     * Level 5: Episode5_v2
     * Level 6: Episode6_v2
     * Level 7: Episode7_v2
     * 
     * The version can be changed to reset the highscores without having to change the level name.
     * For each "loadEpisode" function, enter the previous level's name in the if condition.
     * (- Yannick)
     */

    public int lvlsUnlocked;

    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        for (int lvl = 1; lvl < 7; lvl++)
        {
            if (!(PlayerPrefs.GetFloat("Episode" + lvl.ToString() + "_v2", 0) > 0)) //getting highscore of level 
            {
                int nextLvl = lvl + 1;
                GameObject.Find("Ep. " + nextLvl.ToString()).SetActive(false);
                lvlsUnlocked--;
            }
        }
        GameObject.Find("/Canvas/GamePadCursor").GetComponent<GamePadControls>().levelsUnlocked = lvlsUnlocked;
        if (SceneManager.GetSceneByBuildIndex(14).isLoaded)
            SceneManager.UnloadSceneAsync(14);
    }


    void GoingBack()
    {
        SceneManager.LoadScene(14, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(1);
    }

    void LoadEpisode1() //Pilot
    {
        SceneManager.LoadScene(4);
    }

    void LoadEpisode2()
    {
        SceneManager.LoadScene(7);
    }

    void LoadEpisode3()
    {
        SceneManager.LoadScene(8);
    }

    void LoadEpisode4()
    {
        SceneManager.LoadScene(9);
    }

    void LoadEpisode5()
    {
        SceneManager.LoadScene(10);
    }

    void LoadEpisode6()
    {
        SceneManager.LoadScene(11);
    }

    void LoadEpisode7()
    {
        SceneManager.LoadScene(12);
    }
}
