using UnityEngine;

public class SpecialCardController : MonoBehaviour
{
    public float additionalTime = 10.0f;

    private SceneController sceneController;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>(); // Get the SceneController reference
    }

    private void OnMouseDown()
    {
        // Add additional time to the timer
        sceneController.OnSpecialCardClicked(); // Call the correct method

        // Destroy the special card
        Destroy(gameObject);
    }
}
