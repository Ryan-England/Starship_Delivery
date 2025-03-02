using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu1 : MonoBehaviour
{
    public GameObject start;
    public GameObject options;
    public GameObject colorblind; // Reference to the Colorblind menu

    public void Switch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Trigger_Options()
    {
        options.SetActive(true);
        start.SetActive(false);
    }

    public void Back()
    {
        options.SetActive(false);
        start.SetActive(true);
    }

    public void OpenColorblindMenu()
    {
        colorblind.SetActive(true);
        options.SetActive(false);
    }

    public void BackToOptions()
    {
        colorblind.SetActive(false);
        options.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Application has ended!");
        Application.Quit();
    }
}
