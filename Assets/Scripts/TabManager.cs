using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode journalKey = KeyCode.J;

    [SerializeField] private Slider mouseSens;
    public static float mouseValue;
    public GameObject player; 
    public Vector3 pos;

    public static bool detuer;
    public static bool trit;
    public static bool protan;
    public static bool none;

    void Start()
    {
        HideAllPages();
        mouseValue = PlayerPrefs.GetFloat("MouseSensitivity", 10.0f);
        mouseSens.value = mouseValue;
    }

    void Update()
    {
        pos = player.transform.position;
        if (Input.GetKeyDown(journalKey))
        {
            OpenTab(0);
        } else if (Input.GetKeyDown(pauseKey)) {
            OpenTab(2);
        }
    }

    public void SaveMouseSensitivity()
    {
        mouseValue = mouseSens.value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseValue);
        PlayerPrefs.Save();
    }

    public void OpenTab(int index)
    {
        HideAllPages();
        if (index >= 0 && index < pages.Count)
        {
            pages[index].SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void CloseMenu()
    {
        HideAllPages();
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart() {
        HideAllPages();
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Main_Example");
    }

    private void HideAllPages()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
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

    public void normal(){
        detuer = false; 
        protan = false; 
        trit = false;
        none = true;
    }

    public void ExitGame()
    {
        Debug.Log("Application has ended!");
        Application.Quit();
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
