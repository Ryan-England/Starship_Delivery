using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // A dictionary serving as the lookup table for items
    [System.Serializable]
    public class ItemEntry
    {
        public string itemName;
        public GameObject itemPrefab;
    }

    [SerializeField] private List<ItemEntry> itemsList = new List<ItemEntry>();
    private Dictionary<string, GameObject> itemsLookup;

    // Reference to the current item
    private GameObject currentItem;

    // Radius of the sphere collider to detect nearby objects
    public float detectionRadius = 5f;

    // Score variable
    private int score = 0;

    // Default score to be printed when an item is changed
    private int defaultScore = 100;

    // The layer mask to check only for "station" tagged objects
    public LayerMask LayerMask;

    private void Start()
    {
        FindInitialItem("Cube");
        
        //SetInitialItem("Item1", new Vector3(-0.26f, 2.11f, 2.55f));

        //Invoke("SwitchtoItem2", 4f);

        //Invoke("SwitchtoItem3", 8f);
    }

    private void Update()
    {
        // Detect if the "E" key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Perform a sphere check to detect objects tagged as "station" within the radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask);
            
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Stations"))
                {
                    // If a "station" is detected, change the item
                    ChangeItem("Item2");
                    break;
                }
            }
        }
    }

    private void Awake()
    {
        // Initialize the lookup dictionary
        itemsLookup = new Dictionary<string, GameObject>();
        foreach (var entry in itemsList)
        {
            if (!itemsLookup.ContainsKey(entry.itemName))
            {
                itemsLookup.Add(entry.itemName, entry.itemPrefab);
            }
        }
    }

    public void ChangeItem(string newItemName)
    {
        if (itemsLookup.TryGetValue(newItemName, out GameObject newItemPrefab))
        {
            Vector3 currentPosition = currentItem.transform.position;
            Vector3 scale = currentItem.transform.localScale;
            Quaternion rotation = Quaternion.identity;

            // If there's a current item, destroy it
            if (currentItem != null)
            {
                rotation = currentItem.transform.rotation;
                Destroy(currentItem);
            }

            score = score + defaultScore;

            Debug.Log($"{currentItem.name} changed to '{newItemName}'! Default score: {score}");

            // Instantiate the new item at the specified position and rotation
            currentItem = Instantiate(newItemPrefab, currentPosition, rotation);
            currentItem.transform.localScale = scale;
        }
        else
        {
            Debug.LogError($"Item '{newItemName}' not found in the lookup table.");
        }
    }

    public void FindInitialItem(string itemName)
    {
        GameObject foundItem = GameObject.Find(itemName);
        if (foundItem != null)
        {
            currentItem = foundItem;
            if (!itemsLookup.ContainsKey(itemName))
            {
                itemsLookup.Add(itemName, foundItem);
                Debug.Log($"Item '{itemName}' found in the scene and added to the lookup table.");
            }
            else
            {
                Debug.Log($"Item '{itemName}' is already in the lookup table.");
            }
        }
        else
        {
            Debug.LogError($"Item '{itemName}' not found in the scene.");
        }
    }

    public void SetInitialItem(string itemName, Vector3 position)
    {
        if (itemsLookup.TryGetValue(itemName, out GameObject itemPrefab))
        {
            currentItem = Instantiate(itemPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Item '{itemName}' not found in the lookup table.");
        }
    }

    // Helper methods for testing in Start()
    private void SwitchtoItem2()
    {
        ChangeItem("Item2");
    }

    private void SwitchtoItem3()
    {
        ChangeItem("Item3");
    }
}
