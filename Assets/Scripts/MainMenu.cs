using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Easy()
    {
        SceneManager.LoadScene("Easy");
    }
    public void Medium()
    {
        SceneManager.LoadScene("Medium");
    }
    public void Hard()
    {
        SceneManager.LoadScene("Hard");
    }
    public void Quit() {
        Application.Quit();
        Debug.Log("Player has quit the game.");
    }
}

