using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu1: MonoBehaviour
{
    public GameObject start;
    public GameObject options;
    void Start(){

    }
    // Update is called once per frame
    void Update()
    {
        
    }    
    public void Switch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Trigger_Options(){
        options.SetActive(true);
        start.SetActive(false);
    }
    public void Back(){
        options.SetActive(false);
        start.SetActive(true);        
    }
    public void ExitGame()
    {
        Debug.Log("Application has ended!");
        Application.Quit();
    }
}
