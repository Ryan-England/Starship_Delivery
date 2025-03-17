using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    // The detection range of the sphere that is created to find objects nearby the player
    [SerializeField] private float detectionRange = 1f;
    // If we want to search for a specific layer of objects so not everything is being searched, we use this.
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private Transform cam;

    [SerializeField] private GameObject crosshair;

    public InventoryManager im; 
    public Inventory inv;

    public Fridge fridge;
    public CookingTemplate kitchen;
    public BakingTemplate oven;
    public MixingTemplate mix;

    private void Update()
    {
        // Pressing E to interact with objects/npcs from a range
        if(Input.GetKeyDown(KeyCode.E))
        {
            
            Debug.Log("Pressed E");
            DetectObjects();
        }

    }

    private void DetectObjects()
    {

        // Checks for a game object within a specified distance in front of the player.
        RaycastHit hit;
        Physics.Raycast(cam.position, cam.forward, out hit, detectionRange, detectionLayer);
        Collider collider = hit.collider;
        //Debug.Log(cam.forward.x + " " + cam.forward.y + " " + cam.forward.z);

        if (collider != null)//If one is found, see if/how it can be interacted with.
        {

            // Sets collider as an object reference
            GameObject obj = collider.gameObject;
            Debug.Log("Collided with " + obj.name);
            //Debug.Log("Hit: " + obj);
            Debug.Log(obj.tag);
            Debug.Log(obj.name);
            // Returns true/false as it calls a function to compare the object's tag, will follow through if true.
            if (MatchesTag(obj))
            {
                Debug.Log(obj.tag);
                if (obj.tag == "Collectible")
                {
                    im.CollectItem(obj, Inventory.ItemType.Ingredient);
                }
                else if (obj.tag == "Stations")
                {
                    if (crosshair != null)
                    {
                        crosshairscript chs = crosshair.GetComponent<crosshairscript>();
                        if (chs != null && chs.enabled)
                        {
                            chs.SetVisible(false);
                        }
                    }
                    Debug.Log("found a station. ");
                    switch (obj.name)
                    {
                        case "Fridge":
                            fridge.FridgeMenu();
                            break;
                        case "Kitchen":
                            kitchen.KitchenMenu();
                            break;
                        case "Oven":
                            oven.KitchenMenu();
                            break;
                        case "Mixing":
                            mix.KitchenMenu();
                            break;
                        default:
                            break;
                    }
                }
                
                // Debug.Log("Interaction found");
                // // Makes a interactionhandler reference and connects with the script on the object found
                InteractionHandler interaction = obj.GetComponent<InteractionHandler>();
                // If the script on the object does exist, do the thing
                if (interaction != null)
                {
                    // Debug.Log("Call interactor");
                    interaction.Interact(); // Call the interactionhandler's interact function
                }
                else
                {
                    Debug.Log(interaction);
                }
            }

        }
        else
        {
            Debug.Log("Didn't collide with anything.");
        }

        
    }

    // Function to compare the object's tag to see if it matches
    private bool MatchesTag(GameObject obj)
    {
        switch (obj.tag)
        {
            case "NPC":
            case "Object":
            case "Objective":
            case "Collectible":
            case "Stations":
                return true; // Accept any matching tags
            default:
                return false; //appropriate tag not found
        }
    }
}
