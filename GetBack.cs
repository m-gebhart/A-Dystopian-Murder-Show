using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetBack : MonoBehaviour
{
    public Button backButton;
    public int lastSceneInt;
    public int thisSceneInt;

    void Start()
    {
        Time.timeScale = 1;
        backButton.onClick.AddListener(GettingBack);
        if (SceneManager.GetSceneByBuildIndex(lastSceneInt).isLoaded)
            SceneManager.UnloadSceneAsync(lastSceneInt);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(thisSceneInt));
    }

    void GettingBack()
    {
        SceneManager.LoadScene(lastSceneInt, LoadSceneMode.Additive);
        PlayerPrefs.SetInt("LastScn", thisSceneInt);
        SceneManager.UnloadSceneAsync(thisSceneInt);
    }
}