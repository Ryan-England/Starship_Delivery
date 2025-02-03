using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close the application
            Application.Quit();

            // Log a message to the console (useful for debugging in the editor)
            Debug.Log("Game is exiting...");
        }
    }
}
