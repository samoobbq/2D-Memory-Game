using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script

    // Method to handle pause button click
    public void OnPauseButtonClick()
    {
        if (gameManager != null)
        {
            gameManager.TogglePause();
            Time.timeScale = 0;
        }
    }

    // Method to handle resume button click
    public void OnResumeButtonClick()
    {
        if (gameManager != null)
        {
            gameManager.TogglePause(); // Assuming TogglePause handles both pause and resume
            Time.timeScale = 1;
        }
    }

    public void OnMenuButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
