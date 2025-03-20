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
    [SerializeField] private GameObject tutorialHolder;
    public static float mouseValue;
    public GameObject player; 
    public Vector3 pos;
    

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
            tutorialHolder.SetActive(false);
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
        PlayerPrefs.SetInt("detuer", 1);
        PlayerPrefs.SetInt("protan", 0);
        PlayerPrefs.SetInt("trit", 0);
        FindObjectOfType<ColorBlindFilter>().LoadColorSettings();
    }
    public void tri(){
        PlayerPrefs.SetInt("detuer", 0);
        PlayerPrefs.SetInt("protan", 0);
        PlayerPrefs.SetInt("trit", 1);
        FindObjectOfType<ColorBlindFilter>().LoadColorSettings();
    }
    public void pro(){
        PlayerPrefs.SetInt("detuer", 0);
        PlayerPrefs.SetInt("protan", 1);
        PlayerPrefs.SetInt("trit", 0);
        FindObjectOfType<ColorBlindFilter>().LoadColorSettings();
    }

    public void normal(){
        PlayerPrefs.SetInt("detuer", 0);
        PlayerPrefs.SetInt("protan", 0);
        PlayerPrefs.SetInt("trit", 0);
        FindObjectOfType<ColorBlindFilter>().LoadColorSettings();
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
