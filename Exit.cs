/* Yannick Renz
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private Animator animator;

    public string levelName; //Used to store highscores

    public int nextLevelInt;
    public bool tutorial; //if current Level is a tutorial
    private bool nextLvlLoaded = false;
    private Laurel laurel;
    private Hardy hardy;

    public bool open = false;
    public float openingTime;
    private bool canLeave;

    private float LaurelTimer;
    private float HardyTimer;

    private GameObject buttonPrompt;
    public Sprite gamePadPrompt;

    void Start()
    {
        if ((nextLevelInt == 5 || nextLevelInt == 6 || nextLevelInt == 1))
        {
            open = true; // if tutorial level is complete
            switch (nextLevelInt)
            {
                case 5: SceneManager.LoadSceneAsync("PilotBackground", LoadSceneMode.Additive); break;//Loading background music
                case 6: SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(5)); break;
                case 1: SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(6)); break;
            }
        }
      /*  laurel = GameObject.Find("/Playermanager/PlayerLaurel").GetComponent<Laurel>();
        hardy = GameObject.Find("/Playermanager/PlayerHardy").GetComponent<Hardy>(); */
        animator = this.gameObject.GetComponent<Animator>();
        buttonPrompt = GameObject.Find("ElevatorButtonPrompt");
        buttonPrompt.SetActive(false);
        canLeave = false;

        if (GamePadInUse.gamePadInUse)
        {
            buttonPrompt.GetComponent<SpriteRenderer>().sprite = gamePadPrompt;
        }
    }

    void Update()
    {
        if (Playermanager.isLaurel)
            LaurelTimer += Time.deltaTime;
        else
            HardyTimer += Time.deltaTime;

        if (canLeave && InputManager.LeaveLevelInput())
            ExitLevel(Playermanager.isLaurel);
        if (open && !animator.GetBool("isOpening"))
            animator.SetBool("isOpening", true);
    }

    private void ExitLevel(bool asLaurel)
    {
        if (!nextLvlLoaded) //prevent multiple execution
        {
            nextLvlLoaded = true;

            UIManager ui = GameObject.Find("UIManager").GetComponent<UIManager>();

            PlayerPrefs.SetFloat("LastScore", ui.score);

            //saveLastChar
            if (asLaurel)
                PlayerPrefs.SetString("LastCharacter", "L");
            else
                PlayerPrefs.SetString("LastCharacter", "H");

            //save Level Time
            PlayerPrefs.SetFloat("LevelTime", Time.timeSinceLevelLoad);

            //save FavChar
            if (HardyTimer > LaurelTimer)
                PlayerPrefs.SetString("FavouriteCharacter", "Hardy");
            else if (LaurelTimer > HardyTimer)
                PlayerPrefs.SetString("FavouriteCharacter", "Laurel");
            else
                PlayerPrefs.SetString("FavouriteCharacter", "Neither!"); //unlikely, though

            //save Level Name
            PlayerPrefs.SetString("LastLevel", levelName);
            PlayerPrefs.SetInt("LastLevelIndex", SceneManager.GetActiveScene().buildIndex);

            if (!tutorial) ShowKills();

            if (levelName.StartsWith("Episode1")) PlayerPrefs.SetFloat(levelName, 1); //Simply for unlocking the second level.

            PlayerPrefs.Save();

            LoadNextScene();
        }
    }

    void LoadNextScene() {

        if (nextLevelInt == 5 || nextLevelInt == 6) //if Tutorial
        {
            SceneManager.UnloadSceneAsync(nextLevelInt - 1);
            SceneManager.LoadSceneAsync(nextLevelInt, LoadSceneMode.Additive);
            GameObject.Find("/Main Camera").SendMessage("UpdatePlayer");
            nextLvlLoaded = true;
        }
        else
        {
            SceneManager.LoadScene(nextLevelInt);
            SceneManager.LoadSceneAsync(17, LoadSceneMode.Additive);
        }
    }

    void ShowKills()
    {
        AllEnemyManagers allManagers = new AllEnemyManagers();
        allManagers.SaveKills();
    }
        

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.name.StartsWith("PlayerHardy") || col.gameObject.name.StartsWith("PlayerLaurel")) && open)
        {
            buttonPrompt.SetActive(true);
            canLeave = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if ((col.gameObject.name.StartsWith("PlayerHardy") || col.gameObject.name.StartsWith("PlayerLaurel")) && open)
        {
            buttonPrompt.SetActive(false);
            canLeave = false;
        }
    }
}