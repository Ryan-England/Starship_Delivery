using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    [SerializeField] private KeyCode journalKey = KeyCode.J;

    void Start()
    {
        HideAllPages();
    }

    void Update()
    {
        if (Input.GetKeyDown(journalKey))
        {
            OpenTab(0);
        }
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

    private void HideAllPages()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }
}
