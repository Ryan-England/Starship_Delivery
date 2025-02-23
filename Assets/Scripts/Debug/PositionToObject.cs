using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionToObject : MonoBehaviour
{
    [Tooltip("Assign a gameObject to set the position of the current object.")]
    [SerializeField] private GameObject targetObject;
    [Tooltip("Assign a value for the x or y offset that you want to include.")]
    [SerializeField] private Vector3 offset = new Vector3(0f,0f,0f);

    private void Start()
    {
        // Check if the targetObject is assigned
        if (targetObject != null)
        {
            // Set the position of the current object (this script's GameObject) to the target object's position
            transform.position = targetObject.transform.position + offset;
        }
        else
        {
            Debug.LogError("Target object not assigned!");
        }
    }
}
