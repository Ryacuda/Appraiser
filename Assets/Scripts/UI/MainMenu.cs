using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelWindows;

    public void StartGame()
    {
        levelWindows.SetActive(true);
    }

    public void LevelOne()
    {
        SceneManager.LoadScene("Level Luc");
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("Level Adam");
    }



    public void QuitGame()
    {
        Application.Quit();
    }
}
