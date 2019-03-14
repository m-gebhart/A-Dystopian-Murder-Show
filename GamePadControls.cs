using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePadControls : MonoBehaviour {

    public bool instantGetBack;
    public Button[] buttons;
    public GameObject canvas;
    public int currentPos;
    private RectTransform thisTransform;
    public int levelsUnlocked;
    private int lastButton;

    void Start()
    {
        if (GamePadInUse.gamePadInUse)
            ActivateControls();
    }

    void ActivateControls()
    {
        Debug.Log("activating");
        GamePadInUse.gamePadInUse = true;
        thisTransform = this.gameObject.GetComponent<RectTransform>();
        //set cursor to position
        if (PlayerPrefs.GetInt("LastScn") != null)
        {
            switch (PlayerPrefs.GetInt("LastScn"))
            {
                case 0: currentPos = 0; break; //start
                case 16: currentPos = 1; break; //options
                case 18: currentPos = 2; break; //controls
                case 15: currentPos = 3; break; //credits
            }
            PlayerPrefs.SetInt("LastScn", 0);
        }
        else
            currentPos = 0;
        thisTransform.anchoredPosition = buttons[currentPos].GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        Debug.Log(currentPos + GamePadInUse.gamePadInUse.ToString());
        if (GamePadInUse.gamePadInUse)
        {
            CheckInput();
            thisTransform.anchoredPosition = buttons[currentPos].GetComponent<RectTransform>().anchoredPosition;
        }
        else if (InputManager.LeftInput() || InputManager.RightInput() || InputManager.DownInput() || InputManager.UpInput())
            ActivateControls();
        else if (Input.GetMouseButtonDown(0));
        {
            GamePadInUse.gamePadInUse = false;
            PlayerPrefs.SetInt("LastScn", 0);
        }
    }

    void CheckInput()
    {
        if (instantGetBack && (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("XBoxB")))
            canvas.SendMessage("GettingBack");
        if (InputManager.ApplyInput())
            ApplyTransition();
        else if (InputManager.UpInput())
        {
            if (0 < currentPos && currentPos <= buttons.Length - 1)
                currentPos--;
            else if (currentPos == 0)
                currentPos = buttons.Length - 1;
            CheckLevels(false);
        }
        else if (InputManager.DownInput())
        {
            if (0 <= currentPos && currentPos < buttons.Length - 1)
                currentPos++;
            else if (currentPos == buttons.Length - 1)
                currentPos = 0;
            CheckLevels(true);
        }
        else if (InputManager.LeftInput() && SceneManager.GetSceneByBuildIndex(16).isLoaded)
            switch (currentPos)
            {
                case 0: canvas.GetComponent<Options>().LowerMusicVolume(); break;
                case 1: canvas.GetComponent<Options>().LowerEffectsVolume(); break;
            }
        else if (InputManager.RightInput() && SceneManager.GetSceneByBuildIndex(16).isLoaded)
        {
            switch (currentPos)
            {
                case 0: canvas.GetComponent<Options>().GreaterMusicVolume(); break;
                case 1: canvas.GetComponent<Options>().GreaterEffectsVolume(); break;
            }
        }
    }

    void CheckLevels(bool downwards)
    {
        if (SceneManager.GetSceneByBuildIndex(1).isLoaded && (currentPos >= levelsUnlocked && currentPos != 7))
        {
            if (downwards)
                currentPos = 7;
            else if (!downwards)
                currentPos = levelsUnlocked - 1;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.SendMessage("OnMouseOver");
    }

    void OnTriggerExit2D(Collider2D col)
    {
        col.gameObject.SendMessage("OnMouseExit");
    }

    void ApplyTransition()
    {
        if (SceneManager.GetSceneByBuildIndex(14).isLoaded) //Main Menu
        {
            PlayerPrefs.SetInt("LastBttn", currentPos);
            switch (currentPos)
            {
                case 0: canvas.SendMessage("StartingGame"); break;
                case 1: canvas.SendMessage("ShowOptions"); break;
                case 2: canvas.SendMessage("ShowControls"); break;
                case 3: canvas.SendMessage("ShowCredits"); break;
                case 4: canvas.SendMessage("ExitingGame"); break;
            }
        }
        else if ((SceneManager.GetSceneByBuildIndex(15).isLoaded || SceneManager.GetSceneByBuildIndex(18).isLoaded) && currentPos == 0) //Credits / Controls
            canvas.SendMessage("GettingBack");
        else if (SceneManager.GetSceneByBuildIndex(16).isLoaded && currentPos == 2) //Options
            canvas.SendMessage("GettingBack");
        else if (SceneManager.GetSceneByBuildIndex(1).isLoaded) //Levels
        {
            switch (currentPos)
            {
                case 0: canvas.SendMessage("LoadEpisode1"); break;
                case 1: canvas.SendMessage("LoadEpisode2"); break;
                case 2: canvas.SendMessage("LoadEpisode3"); break;
                case 3: canvas.SendMessage("LoadEpisode4"); break;
                case 4: canvas.SendMessage("LoadEpisode5"); break;
                case 5: canvas.SendMessage("LoadEpisode6"); break;
                case 6: canvas.SendMessage("LoadEpisode7"); break;
                case 7: canvas.SendMessage("GoingBack"); break;
            }

        }
    }
}