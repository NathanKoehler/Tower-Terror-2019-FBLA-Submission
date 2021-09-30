using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameController_S GC = GameObject.Find("Game Controller").GetComponent<GameController_S>();
        GC.StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
    public void goToOptions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void backToStart()
    {
        SceneManager.LoadScene("Start");
    }
    public void goToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
