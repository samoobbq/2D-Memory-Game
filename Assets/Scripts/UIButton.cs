using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] string targetMessage;
    public Color highlightColor = Color.cyan;

    private bool isButtonActive = true;

    private void Update()
    {
        // Check for input events in Update instead of using mouse events
        if (isButtonActive && Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; // Ensure the z position is the same as the button

            // Check if the mouse click hits the button's collider
            if (GetComponent<Collider2D>().bounds.Contains(mousePosition))
            {
                ExecuteAction();
            }
        }
    }

    private void ExecuteAction()
    {
        if (targetObject != null)
        {
            targetObject.SendMessage(targetMessage);
            Debug.Log("Message sent to scene controller");
        }
    }

    public void SetButtonActive(bool active)
    {
        isButtonActive = active;
    }

    public void MoveOffScreen()
    {
        // Move the button off screen along the X-axis
        transform.position = new Vector3(-40f, transform.position.y, transform.position.z);
    }

    public void MoveOnScreen()
    {
        // Move the button back on screen along the X-axis
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);
    }
}
