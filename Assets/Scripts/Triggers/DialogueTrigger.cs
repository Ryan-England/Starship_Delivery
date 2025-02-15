using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public LayerMask mask;
    public Transform dialogueSphere;
    public float dialogueDistance = 10f;
    public bool isTriggered;

    public GameObject selector;
    void Update()
    {
        isTriggered = Physics.CheckSphere(dialogueSphere.position, dialogueDistance, mask);
        if(isTriggered){
            selector.SetActive(true);
            // else{
            //     box.SetActive(false);
            // }
        }
        else{
            // Debug.Log("Not within range.");
            selector.SetActive(false);
            // box.SetActive(false);
        }
        // if(!Input.GetKeyDown("e") && !isTriggered){
        //     box.SetActive(false);
        // }
        // else{
        //     box.SetActive(true);
        // }
    }
}
