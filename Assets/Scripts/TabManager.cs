using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    #region Member Variables
    [Header("Tabs")]
    [Tooltip("Insert a menu that you'd like to use.")]
    [SerializeField] private List<GameObject> pages = new List<GameObject>();

    [Header("Keybinds")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode journalKey = KeyCode.J;

    [Header("Player Position")]
    [Tooltip("Player position saved for savefile.")]
    public Vector3 pos;
    private GameObject player; 

    [Header("External References")]
    [SerializeField] private Slider mouseSensSlider;
    [SerializeField] private Slider playerFOVSlider;
    [SerializeField] private TMP_Text mouseSensText;
    [SerializeField] private TMP_Text FOVText;
    [Tooltip("Temporary tutorial reference for journals and gameplay.")]
    [SerializeField] private GameObject tutorialHolder;

    // Variables handled by PlayerCam
    public static float mouseValue;
    public static float FOV;
    public static bool journalActive;
    private PlayerCam playerCam;
    #endregion
    private void Start()
    {
        // Hide all pages upon start
        HideAllPages();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCam = Camera.main.GetComponent<PlayerCam>();

        // Set-up mouse sensitivity and FOV settings
        mouseValue = PlayerPrefs.GetFloat("MouseSensitivity", 5.0f);
        FOV = PlayerPrefs.GetFloat("FOV", 60f);
        playerFOVSlider.value = FOV;
        mouseSensSlider.value = mouseValue;
        UpdateFOVText();
        UpdateMouseSensText();
    }

    private void Update()
    {
        // Track player position at all times
        pos = player.transform.position;
        if (Input.GetKeyDown(journalKey))
        {
            OpenTab(0);
            RemoveTutorial(); // Clicking the 'J' button removes tutorial
        } else if (Input.GetKeyDown(pauseKey)) {
            OpenTab(2);
        }
    }

    // Accessed outside of script by InteractionHandler.cs
    public void RemoveTutorial() {
        tutorialHolder.SetActive(false);
    }

    #region Slider Functionality
    public void SaveMouseSensitivity()
    {
        mouseValue = mouseSensSlider.value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseValue);
        PlayerPrefs.Save();
    }

    public void SavePlayerFOV()
    {
        FOV = playerFOVSlider.value;
        PlayerPrefs.SetFloat("FOV", FOV);
        PlayerPrefs.Save();
        playerCam.UpdateFOV();
    }

    public void UpdateMouseSensText() {
        mouseValue = mouseSensSlider.value;
        mouseSensText.text = ((int)mouseValue).ToString();
    }

    public void UpdateFOVText() {
        FOV = playerFOVSlider.value;
        FOVText.text = ((int)FOV).ToString();
    }
    #endregion
    #region Menu Functionality
    public void OpenTab(int index)
    {
        HideAllPages();
        journalActive = true;
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
        journalActive = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HideAllPages()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }
    #endregion
    #region Color-blind Mode
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
    #endregion
    #region Save file functionality
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
    #endregion

    public void Restart() {
        HideAllPages();
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Main_Example");
    }

    public void ExitGame()
    {
        Debug.Log("Application has ended!");
        Application.Quit();
    }
}
