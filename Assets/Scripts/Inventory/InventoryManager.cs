using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;

    private void Start()
    {
        // inventory = GetComponent<Inventory>();

        // // Add sample items to the inventory
        // inventory.AddItem("Fishing Rod", Inventory.ItemType.Tool, 1);
        // inventory.AddItem("Bait", Inventory.ItemType.Ingredient, 8);

        // Print initial inventory contents
        // inventory.PrintInventory();
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Collectible"))
    //     {
    //         CollectItem(collision.gameObject, Inventory.ItemType.Ingredient);
    //     }
    // }

    public void CollectItem(GameObject collectible, Inventory.ItemType type)
    {
        CollectibleItem collectibleItem = collectible.GetComponent<CollectibleItem>();
        CarolynCollectibleItem c_collectibleItem = collectible.GetComponent<CarolynCollectibleItem>();
       
       
        if (c_collectibleItem != null)
        {
            Debug.Log(c_collectibleItem.itemName);
            // Debug.Log(collectibleItem.quantity);
            inventory.AddItem(c_collectibleItem.itemName, type, c_collectibleItem.quantity);
            c_collectibleItem.harvest();

            // Print new acquired item message
            inventory.PrintItem(c_collectibleItem.itemName);
            return;
            // Print updated inventory contents after collection
            //inventory.PrintInventory();
        }
        else if (collectibleItem != null)
        {
            Debug.Log(collectibleItem.itemName + "not");
            // Debug.Log(collectibleItem.quantity);
            inventory.AddItem(collectibleItem.itemName, type, collectibleItem.quantity);
            collectibleItem.harvest();

            // Print new acquired item message
            inventory.PrintItem(collectibleItem.itemName);

            // Print updated inventory contents after collection
            //inventory.PrintInventory();
        }

    }
}
