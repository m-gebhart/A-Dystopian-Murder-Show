/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    private Text killedBy;
    private Text currentScore;
    private Text highScore;

    void Start () {
        SetComponents();
    }

    void Update () {
		if (InputManager.RespawnInput())
        {
            Respawn();
        }
        if (InputManager.EscapeInput())
        {
            SceneManager.LoadScene(1);
            SceneManager.LoadSceneAsync(17, LoadSceneMode.Additive);
        }
    }

    void SetComponents()
    {
        if (!GameObject.Find("/Exit").GetComponent<Exit>().tutorial) //no tutorial level
        {
            //caused by Player.die()
            string enemy = PlayerPrefs.GetString("killedBy");
            if (enemy.StartsWith("Banana")) enemy = "Banana";
            else if (enemy.StartsWith("Lobster")) enemy = "Lobster";
            else if (enemy.StartsWith("Pie")) enemy = "Pie Cannon";
            else if (enemy.StartsWith("Bomb")) enemy = "Bomb";
            else if (enemy.StartsWith("Bat")) enemy = "Bat";
            else if (enemy.StartsWith("Ground") || enemy.StartsWith("Floor") || enemy.StartsWith("Spike")) enemy = "Spikes";
            GameObject.Find("/Canvas/EnemyText").GetComponent<Text>().text = "Killed by:  " + enemy;
            GameObject.Find("/Canvas/CurrentScore").GetComponent<Text>().text = "Current Score: " + Mathf.Floor(PlayerPrefs.GetFloat("CurrentScore"));
            GameObject.Find("/Canvas/HighScore").GetComponent<Text>().text = "High Score: " + Mathf.Floor(PlayerPrefs.GetFloat(PlayerPrefs.GetString("LastLevel"), 0));

            float time = PlayerPrefs.GetFloat("LevelTime");
            if (time % 60 < 10)
                GameObject.Find("/Canvas/Time").GetComponent<Text>().text = "Time survived: " + Mathf.FloorToInt(time / 60) + ":0" + Mathf.FloorToInt(time % 60);
            else
                GameObject.Find("/Canvas/Time").GetComponent<Text>().text = "Time survived: " + Mathf.FloorToInt(time / 60) + ":" + Mathf.FloorToInt(time % 60);
        }
        else
        {
            GameObject.Find("/Canvas/EnemyText").GetComponent<Text>().text = "CONGRATS";
            GameObject.Find("/Canvas/CurrentScore").GetComponent<Text>().text = "You died";
            GameObject.Find("/Canvas/HighScore").GetComponent<Text>().text = "in the";
            GameObject.Find("/Canvas/Time").GetComponent<Text>().text = "Tutorial";
            
            if (GamePadInUse.gamePadInUse)
            {
                GameObject.Find("/Canvas/R").GetComponent<Text>().text = "Press <X> to try again";
                GameObject.Find("/Canvas/ESC").GetComponent<Text>().text = "Press <B> to get back to menu";
            }
        }
    }

    public static void Respawn()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevelIndex"));
        if (PlayerPrefs.GetInt("LastLevelIndex") == 5 || PlayerPrefs.GetInt("LastLevelIndex") == 6)
            SceneManager.LoadSceneAsync("PilotBackground", LoadSceneMode.Additive);
        if (SceneManager.GetSceneByName("GameOver").isLoaded)
            SceneManager.UnloadSceneAsync("GameOver");
    }
}
