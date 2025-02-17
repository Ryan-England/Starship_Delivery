using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    public GameObject Menu;
    public GameObject DiaBox; 
    public GameObject DiaChoice;
    public GameObject Inventory;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DiaChoice.SetActive(false);
            DiaBox.SetActive(false);
            Menu.SetActive(false);
            Inventory.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void BackButton(){
            DiaChoice.SetActive(false);
            DiaBox.SetActive(false);
            Menu.SetActive(false);
            Inventory.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    }
}
