/* Yannick Renz
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour {

    public Text Viewers; //

    public Text NewHighScore;

    public Text HighScoreText;
    public Text HighScore;

    public Text TimeText; //
    public Text FavouriteCharacter;
    public Text EnemiesKilled;

    public Text countBat; //
    public Text countBanana;
    public Text countLobster;
    public Text countCannon;

    public AudioLowPassFilter lowpass;
    public AudioHighPassFilter highpass;

    private string lastLevel;
    private bool hasNewHighScore;
    private int lastLevelIndex;

    private Animator animator;

    // Use this for initialization
    void Start () {

        lastLevelIndex = PlayerPrefs.GetInt("LastLevelIndex");

        float viewers = PlayerPrefs.GetFloat("LastScore");
        Viewers.text = viewers.ToString("f0");

        lastLevel = PlayerPrefs.GetString("LastLevel");
        float lastHighscore = PlayerPrefs.GetFloat(lastLevel, 0);
        if (viewers > lastHighscore)
        {
            HighScore.gameObject.SetActive(false);
            HighScoreText.gameObject.SetActive(false);
            NewHighScore.gameObject.SetActive(true);
            PlayerPrefs.SetFloat(lastLevel, viewers);
            hasNewHighScore = true;
        }
        else
        {
            HighScore.text = lastHighscore.ToString("f0");
            hasNewHighScore = false;
        }

        EnemiesKilled.text = "" + (
            PlayerPrefs.GetInt("BananasKilled") +
            PlayerPrefs.GetInt("BatsKilled") +
            PlayerPrefs.GetInt("LobstersKilled") +
            PlayerPrefs.GetInt("CannonsKilled"));

        countBanana.text = "x" + PlayerPrefs.GetInt("BananasKilled");
        countBat.text = "x" + PlayerPrefs.GetInt("BatsKilled");
        countLobster.text = "x" + PlayerPrefs.GetInt("LobstersKilled");
        countCannon.text = "x" + PlayerPrefs.GetInt("CannonsKilled");

        FavouriteCharacter.text = PlayerPrefs.GetString("FavouriteCharacter");

        

        float time = PlayerPrefs.GetFloat("LevelTime");
        if (time % 60 < 10)
            TimeText.text = Mathf.FloorToInt(time / 60) + ":0" + Mathf.FloorToInt(time % 60);
        else
            TimeText.text = Mathf.FloorToInt(time / 60) + ":" + Mathf.FloorToInt(time % 60);

        animator = GameObject.Find("/Background").GetComponent<Animator>();

        if (GamePadInUse.gamePadInUse)
            GameObject.Find("/Overlay/ESC").GetComponent<Text>().text = "PRESS >B< TO GET BACK TO MENU";
    }

    private float timer;
	// Update is called once per frame
	void Update () {

        if (InputManager.LeaveLevelInput())
        {
            if (lastLevelIndex >= 12) //Last level leads to main menu
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(lastLevelIndex + 1);
            }
        }
        else if (InputManager.EscapeInput())
        {
            SceneManager.LoadScene(1);
            SceneManager.LoadSceneAsync(17, LoadSceneMode.Additive);
        }

        if (hasNewHighScore)
        {
            if (lowpass.cutoffFrequency < 20000) lowpass.cutoffFrequency += 4;
            if (highpass.cutoffFrequency > 20) highpass.cutoffFrequency -= 1;
            if (timer > 1)
            {
                if (NewHighScore.gameObject.activeInHierarchy) NewHighScore.gameObject.SetActive(false);
                else NewHighScore.gameObject.SetActive(true);

                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

    /* Michael Gebhart
     * Cologne Game Lab
     * Ludic Game - BA1 2018/2019
     * */

    bool objectFalling = false;
    public float frequency;
    public float fallingTime;

    void LateUpdate()
    {
        if (!objectFalling)
            StartCoroutine(CheckFallingObjects());
    }

    IEnumerator CheckFallingObjects()
    {
        objectFalling = true;
        Random.seed = (int)System.DateTime.Now.Ticks;
        int rnd = Random.Range(0,6); 
        //50% chance to drop an object
        //random Object dropping at 0, 1, 2 and 3
        //no drop at 3, 4, 5 and 6
        animator.SetInteger("randomObject", rnd);
        animator.SetBool("rndApplied", true);
        yield return new WaitForSeconds(fallingTime);

        animator.SetBool("rndApplied", false);
        yield return new WaitForSeconds(frequency - fallingTime);
        objectFalling = false;
    }
}
