using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTemplate : MonoBehaviour
{
    public PlayerMovement pm; 
    public GameObject kitchenUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void KitchenMenu(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pm.enabled = false;
        Time.timeScale = 0f;
        kitchenUI.SetActive(true);
    }

    //basically we want to check if the items have a 
    public void BackButtonKitchen(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pm.enabled =true;
        kitchenUI.SetActive(false);
    }
}
