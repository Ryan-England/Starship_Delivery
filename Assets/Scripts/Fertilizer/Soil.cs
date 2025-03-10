using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public GameObject SoilUI;
    public PlayerMovement pm;
    [SerializeField] private GameObject crosshair;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SoilMenu(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pm.enabled = false;
        Time.timeScale = 0f;
        SoilUI.SetActive(true);
    }

    //basically we want to check if the items have a 
    public void BackButtonSoil(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pm.enabled =true;
        SoilUI.SetActive(false);

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
