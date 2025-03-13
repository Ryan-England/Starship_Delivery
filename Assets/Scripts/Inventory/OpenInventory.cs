using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    public GameObject Menu;
    public GameObject DiaBox; 
    public GameObject DiaChoice;
    public GameObject Inventory;
    public PlayerMovement pm;

    [SerializeField] private GameObject crosshair;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(gameObject.name);
            DiaChoice.SetActive(false);
            DiaBox.SetActive(false);
            Menu.SetActive(false);
            Inventory.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pm.enabled = false;
            Time.timeScale = 0f;

            if (crosshair != null)
            {
                crosshairscript chs = crosshair.GetComponent<crosshairscript>();
                if (chs != null && chs.enabled)
                {
                    chs.SetVisible(false);
                }
            }
        }
    }
    public void BackButton(){
            DiaChoice.SetActive(false);
            DiaBox.SetActive(false);
            Menu.SetActive(false);
            Inventory.SetActive(false);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pm.enabled = true;
        Time.timeScale = 1;

        if (crosshair != null)
        {
            crosshairscript chs = crosshair.GetComponent<crosshairscript>();
            if (chs != null && chs.enabled)
            {
               chs.SetVisible(true);
            }
        }
    }
}
