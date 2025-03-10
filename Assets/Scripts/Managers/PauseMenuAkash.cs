using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuAkash : MonoBehaviour
{

    public GameObject pause;
    public GameObject options;
    public GameObject map;
    public PlayerMovement pm; 
    public GameObject player; 
    public Vector3 pos;
    public Slider mouseSens;
    public float mouseValue;
    public int test;

    // Start is called before the first frame update
    void Start()
    {
        pause.SetActive(false);
        mouseValue = PlayerPrefs.GetFloat("MouseSensitivity", 10.0f);
        mouseSens.value = mouseValue;
    }

    // Update is called once per frame
    void Update()
    {
        pos = player.transform.position;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(true);
            options.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pm.enabled = false;
            Time.timeScale = 0f;
        }    
    }

    public void SaveMouseSensitivity()
    {
        mouseValue = mouseSens.value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseValue);
        PlayerPrefs.Save();
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
    public void BackButtonMap(){
        map.SetActive(false);
        pause.SetActive(true);
    }
    public void ExitButtonPause(){
        Application.Quit();
    }

    public void OptionsButtonPause(){
        pause.SetActive(false);
        options.SetActive(true);
    }
    public void MapButtonPause(){
        pause.SetActive(false);
        map.SetActive(true);
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
