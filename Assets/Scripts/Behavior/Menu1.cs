using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu1 : MonoBehaviour
{
    public GameObject start;
    public GameObject options;
    public GameObject colorblind; // Reference to the Colorblind menu

    public static bool detuer;
    public static bool trit;
    public static bool protan;

    public void Switch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Trigger_Options()
    {
        options.SetActive(true);
        start.SetActive(false);
        colorblind.SetActive(false);
    }

    public void Back()
    {
        options.SetActive(false);
        start.SetActive(true);
        colorblind.SetActive(false);
    }

    public void OpenColorblindMenu()
    {
        colorblind.SetActive(true);
        options.SetActive(false);
        start.SetActive(false);
    }

    public void BackToOptions()
    {
        colorblind.SetActive(false);
        options.SetActive(true);
        start.SetActive(false);
    }
    public void det(){
        detuer = true; 
        protan = false; 
        trit = false;
    }
    public void tri(){
        detuer = false; 
        protan = false; 
        trit = true;
    }
    public void pro(){
        detuer= false;
        protan= true; 
        trit = false;
    }

    public void ExitGame()
    {
        Debug.Log("Application has ended!");
        Application.Quit();
    }
}
