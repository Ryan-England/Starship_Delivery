using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu1 : MonoBehaviour
{
    #region Member Variables
    [Header("Tabs")]
    [Tooltip("Insert a menu that you'd like to use.")]
    [SerializeField] private List<GameObject> screens = new List<GameObject>();

    [Header("External References")]
    [SerializeField] private Slider mouseSensSlider;
    //[SerializeField] private Slider playerFOVSlider;
    [SerializeField] private TMP_Text mouseSensText;
    //[SerializeField] private TMP_Text FOVText;
    [Tooltip("Temporary tutorial reference for journals and gameplay.")]

    // Variables handled by PlayerCam
    public static float mouseValue;
    public static float FOV;
    #endregion
    private void Start()
    {
        // Open the start menu upon start!
        OpenTab(0);

        // Set-up mouse sensitivity and FOV settings
        mouseValue = PlayerPrefs.GetFloat("MouseSensitivity", 5.0f);
        FOV = PlayerPrefs.GetFloat("FOV", 60f);
        //playerFOVSlider.value = FOV;
        mouseSensSlider.value = mouseValue;
        //UpdateFOVText();
        UpdateMouseSensText();
    }

    #region Slider Functionality
    public void SaveMouseSensitivity()
    {
        mouseValue = mouseSensSlider.value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseValue);
        PlayerPrefs.Save();
    }

    // public void SavePlayerFOV()
    // {
    //     FOV = playerFOVSlider.value;
    //     PlayerPrefs.SetFloat("FOV", FOV);
    //     PlayerPrefs.Save();
    // }

    public void UpdateMouseSensText() {
        mouseValue = mouseSensSlider.value;
        mouseSensText.text = ((int)mouseValue).ToString();
    }

    // public void UpdateFOVText() {
    //     FOV = playerFOVSlider.value;
    //     FOVText.text = ((int)FOV).ToString();
    // }
    #endregion
    #region Menu Functionality
    public void OpenTab(int index)
    {
        HideAllScreens();
        if (index >= 0 && index < screens.Count)
        {
            screens[index].SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void HideAllScreens()
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
    }
    #endregion
    #region Color-blind Mode
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
    #endregion
    public void Switch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetQuestProgress()
    {
        PlayerPrefs.SetInt("TutorialCompleted", 0);
        PlayerPrefs.DeleteKey("Quest_" + "Find Apple.");
        PlayerPrefs.DeleteKey("Quest_" + "Find a Banana.");
        PlayerPrefs.DeleteKey("Quest_" + "Make Cinderloaf.");

        PlayerPrefs.Save();
        Debug.Log("All quest progress has been reset.");
    }

    public void ExitGame()
    {
        Debug.Log("Application has ended!");
        Application.Quit();
    }
}
