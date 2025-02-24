using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crosshairscript : MonoBehaviour
{
    public bool enabled = true;
    // Start is called before the first frame update
    void Start()
    {
        if (!enabled)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVisible(bool visible)
    {
        foreach (Transform child in gameObject.transform) { 
            child.gameObject.SetActive(visible);
        }
    }
}
