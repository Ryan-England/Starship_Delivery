using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class OpenCookbook : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void Start()
    {
        menu.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) {
            menu.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) {
            menu.SetActive(false);
        }
    }
}
