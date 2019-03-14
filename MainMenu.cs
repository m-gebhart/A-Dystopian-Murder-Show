/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private Button startGame;
    private Button exitGame;
    private Button credits;
    private Button options;
    private Button controls;

    private GameObject mainLight;

    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(14));
        SetComponents();
    }


    void SetComponents()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(14));

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(14));

        startGame = GameObject.Find("/Canvas/Start").GetComponent<Button>();
        startGame.onClick.AddListener(StartingGame);

        exitGame = GameObject.Find("/Canvas/Exit").GetComponent<Button>();
        exitGame.onClick.AddListener(ExitingGame);

        credits = GameObject.Find("/Canvas/Credits").GetComponent<Button>();
        credits.onClick.AddListener(ShowCredits);

        options = GameObject.Find("/Canvas/Options").GetComponent<Button>();
        options.onClick.AddListener(ShowOptions);

        controls = GameObject.Find("/Canvas/Controls").GetComponent<Button>();
        controls.onClick.AddListener(ShowControls);
    }

    void StartingGame()
    {
        Destroy(GameObject.Find("/Canvas"));
        SceneManager.LoadScene("Levels", LoadSceneMode.Additive);
    }

    void ExitingGame()
    {
        Application.Quit();
    }

    void ShowCredits()
    {
        SceneManager.LoadScene(15, LoadSceneMode.Additive);
    }

    void ShowOptions()
    {
        SceneManager.LoadScene(16, LoadSceneMode.Additive);
    }

    void ShowControls()
    {
        SceneManager.LoadScene(18, LoadSceneMode.Additive);
    }

    void GettingBack()
    {
    /*    Debug.Log("exiting");
        ExitingGame(); */
    }
}