using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    //Pause Menu in Unity by Brackeys
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Slider mouseSens;
    public static float mouseValue;
    public GameObject player; 
    private Vector3 pos;
    public GameObject pause;
    public GameObject options;
    public PlayerMovement pm; 
    public int test;
    private void Start()
    {
        pauseMenuUI.SetActive(false);
        mouseValue = mouseSens.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            mouseValue = mouseSens.value;
        }

    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Quit()
    {
        Debug.Log("Application is quitting!");
        Application.Quit();
    }
    public void Restart()
    {
        Debug.Log("Restart button pressed!");
        Resume();
        SceneManager.LoadScene("MoveTestScene");
    }
    public void BackButtonPause(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pause.SetActive(false);
        Time.timeScale = 1;
        pm.enabled =true;
        options.SetActive(false);
    }
    public void BackButtonOptions(){
        options.SetActive(false);
        pause.SetActive(true);
    }

    public void ExitButtonPause(){
        Application.Quit();
    }

    public void OptionsButtonPause(){
        pause.SetActive(false);
        options.SetActive(true);
    }
    public void SaveGame(){
        Debug.Log(pos);
        SaveSystem.SavePlayer(pos);
    }
    public void LoadGame(){
        PlayerData data = SaveSystem.LoadPlayer();
        Debug.Log(data.position[0]);
        Debug.Log(data.position[1]);
        Debug.Log(data.position[2]);
        player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
    }
}
