using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        // Perform additional actions when pausing/resuming, such as showing/hiding UI
        if (isPaused)
        {
            Debug.Log("Game Paused");
            // Add code to show pause menu or UI
        }
        else
        {
            Debug.Log("Game Resumed");
            // Add code to hide pause menu or UI
        }
    }
}
