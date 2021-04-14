using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        int slCount = FindObjectsOfType<SceneLoader>().Length;
        if (slCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadPreviousScene()  //Obsolete...
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);

    }

    public void LoadSecondScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLastScene()  //This will load what is presumably the game over screen;
    {
        //TODO universalize this to always be the game over scene.
        SceneManager.LoadScene(2);
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
