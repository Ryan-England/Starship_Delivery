using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Enum to categorize items
    public enum ItemType
    {
        Tool,
        Ingredient
    }

    // Item class to hold item details
    [System.Serializable]
    public class Item
    {
        public string itemName;
        public ItemType itemType;
        public int quantity;

        public Item(string name, ItemType type, int qty)
        {
            itemName = name;
            itemType = type;
            quantity = qty;
        }
    }

    // Dictionary to store items based on their category
    public Dictionary<ItemType, List<Item>> itemCollection = new Dictionary<ItemType, List<Item>>();
    public CanvasInventory ci;
    private void Awake()
    {
        // Initialize the dictionary with empty lists for each item type
        itemCollection[ItemType.Tool] = new List<Item>();
        itemCollection[ItemType.Ingredient] = new List<Item>();
    }

    // Method to add item to the inventory
    public bool AddItem(string name, ItemType type, int qty)
    {
        if(!ci.AddItems(name, qty))
        {
            Debug.Log("Couldn't add item to inventory");
            return (false);
        }
        Debug.Log("Successfully added item");
        // Debug.Log("2"); 
        // Check if the item already exists in the inventory
        foreach (var item in itemCollection[type])
        {
            // Debug.Log("3"); 
            if (item.itemName == name)
            {
                // Debug.Log("4"); 
                // Increment the quantity of the existing item
                item.quantity += qty;
                return(true);
                
            }
            // Debug.Log("5"); 
        }
        // Debug.Log("6"); 

        // Add the new item if it doesn't exist
        Item newItem = new Item(name, type, qty);
        // Debug.Log("7"); 
        itemCollection[type].Add(newItem);
        return(true);
        // Debug.Log("8"); 

    }

    // Method to remove an item from the inventory
    public void RemoveItem(string name, ItemType type, int qty)
    {
        // Iterate through the items in the specified category
        for (int i = 0; i < itemCollection[type].Count; i++)
        {
            var item = itemCollection[type][i];
            if (item.itemName == name)
            {
                // Decrement the quantity of the existing item
                item.quantity -= qty;

                // If the quantity reaches zero or less, remove the item
                if (item.quantity <= 0)
                {
                    itemCollection[type].RemoveAt(i);
                    Debug.Log($"Item {name} removed from inventory.");
                }
                return;
            }
        }

        Debug.LogWarning($"Item {name} not found in inventory.");
    }

    // Method to get the list of items of a specific type
    public List<Item> GetItems(ItemType type)
    {
        return itemCollection[type];
    }

    // Method to print inventory contents
    public void PrintInventory()
    {
        Debug.Log("Inventory Contents:");
        
        foreach (var category in itemCollection)
        {
            Debug.Log($"Category: {category.Key}");
            foreach (var item in category.Value)
            {
                Debug.Log($"Item: {item.itemName}, Quantity: {item.quantity}");
            }
        }
    }

    // method to print a specific item by name
    public void PrintItem(string itemName)
    {
        foreach (var category in itemCollection)
        {
            foreach (var item in category.Value)
            {
                // Debug.Log(item.itemName);
                // Debug.Log(itemName);
                if (item.itemName == itemName)
                {
                    Debug.Log($"Found Item: {item.itemName}, Quantity: {item.quantity}, Category: {category.Key}");
                    return;
                }
            }
        }
    }

    public bool FindItem(string itemName)
    {
        foreach (var category in itemCollection)
        {
            foreach (var item in category.Value)
            {
                // Debug.Log(item.itemName);
                // Debug.Log(itemName);
                if (item.itemName == itemName)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
