using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu1 : MonoBehaviour
{
    public GameObject start;
    public GameObject options;
    public GameObject colorblind; // Reference to the Colorblind menu
    public GameObject credits;

    public void Switch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Trigger_Options()
    {
        options.SetActive(true);
        start.SetActive(false);
        colorblind.SetActive(false);
        credits.SetActive(false);
    }

    public void Back()
    {
        options.SetActive(false);
        start.SetActive(true);
        colorblind.SetActive(false);
        credits.SetActive(false);
    }

    public void OpenColorblindMenu()
    {
        colorblind.SetActive(true);
        options.SetActive(false);
        start.SetActive(false);
        credits.SetActive(false);
    }

    public void BackToOptions()
    {
        colorblind.SetActive(false);
        options.SetActive(true);
        start.SetActive(false);
        credits.SetActive(false);
    }
    public void openCredits()
    {
        colorblind.SetActive(false);
        options.SetActive(false);
        start.SetActive(false);
        credits.SetActive(true);
    }
    public void det(){
        PlayerPrefs.SetInt("detuer", 1);
        PlayerPrefs.SetInt("protan", 0);
        PlayerPrefs.SetInt("trit", 0);
    }
    public void tri(){
        PlayerPrefs.SetInt("detuer", 0);
        PlayerPrefs.SetInt("protan", 0);
        PlayerPrefs.SetInt("trit", 1);
    }
    public void pro(){
        PlayerPrefs.SetInt("detuer", 0);
        PlayerPrefs.SetInt("protan", 1);
        PlayerPrefs.SetInt("trit", 0);
    }

    public void normal(){
        PlayerPrefs.SetInt("detuer", 0);
        PlayerPrefs.SetInt("protan", 0);
        PlayerPrefs.SetInt("trit", 0);
    }

    public void ExitGame()
    {
        Debug.Log("Application has ended!");
        Application.Quit();
    }
}
