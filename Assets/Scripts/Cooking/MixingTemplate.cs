using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Required for UI elements
using UnityEngine.EventSystems;
using UnityEditor;
using System.Data.Common;
using Unity.VisualScripting.ReorderableList;
using System;

public class MixingTemplate : MonoBehaviour
{
    public PlayerMovement pm;
    public GameObject kitchenUI;
    public GameObject startButton;
    public MixingMinigame minigame; // Reference to the minigame script
    public Text timerText; // Assign in Unity
    public float bakingTime = 5f; // Adjust as needed
    private bool isBaking = false;

    List<GameObject> cook_it = new List<GameObject>();
    public Transform cook_items;
    public Fridge f;

    private Dictionary<string, int> csub = new Dictionary<string, int>();
    public GameObject product;
    public GameObject spawner;

    public GameObject Dictionary;
    private RecipeDictionary cookbook;
    private int SLOT_COUNT = 2;

    private string path = "Objects/";
    private string parent;
    void Start()
    {
        minigame = FindObjectOfType<MixingMinigame>(); // Ensure assignment
        parent = gameObject.name;
        cookbook = Dictionary.GetComponent<RecipeDictionary>();
        foreach (Transform k in cook_items) {
            cook_it.Add(k.gameObject);
        }
        startButton.GetComponent<Button>().interactable = false; // Disable button initially
    }

    void Update()
    {
        CheckSlots(); // Continuously check if the button should be enabled
    }

    public void KitchenMenu() {
        Debug.Log(parent);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pm.enabled = false;
        Time.timeScale = 0f;
        kitchenUI.SetActive(true);
    }

    public void BackButtonKitchen() {
        Debug.Log(parent);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pm.enabled = true;
        kitchenUI.SetActive(false);
    }

    public void MoveToCook() {
        Debug.Log(parent);
        GameObject temp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        Slot s = temp.GetComponent<Slot>();

        if (s.filled) {
            AddToCook(s.name, 1);
        }
    }

    public void AddToCook(string name, int quantity) {
        Debug.Log(parent);
        string path = "Icons";
        int i = 0;
        foreach (GameObject j in cook_it) {


            GameObject qty = j.transform.Find("qty").gameObject;
            Text t = qty.GetComponent<Text>();
            Slot s = j.GetComponent<Slot>();
            i++;
            if (!s.filled && !csub.ContainsKey(name))
            {
                GameObject temp = j.transform.Find("Items").Find("apple").gameObject;
                Image test = temp.GetComponent<Image>();
                test.sprite = Resources.Load<Sprite>(path + "/" + name);
                Debug.Log("Slot " + i + " is empty.");
                temp.SetActive(true);
                t.text = "x" + quantity;
                s.filled = true;
                s.name = name;
                Debug.Log("placing " + quantity + " " + name + " in slot " + i);
                if (!csub.ContainsKey(name))
                {
                    csub.Add(name, quantity);
                    Debug.Log("csub contains " + csub[name] + " " + name);
                }
                else
                {
                    csub[name] += quantity;
                    Debug.Log("csub contains " + csub[name] + " " + name);
                }

                Debug.Log("Removing " + quantity + " " + name + " from fridge");
                f.DeleteItems(name, quantity);
                return;
            }
            else if (s.name == name)
            {
                Debug.Log("slot " + i + " Has " + quantity + " " + name);
                csub[name] += quantity;
                Debug.Log("csub contains " + csub[name] + " " + name);
                t.text = "x" + csub[name];
                f.DeleteItems(name, quantity);
                return;
            }
        }
    }

    private void CheckSlots()
    {
        Debug.Log(parent);
        int filledCount = 0;
        foreach (GameObject slotObj in cook_it)
        {
            Slot slot = slotObj.GetComponent<Slot>();
            if (slot.filled) filledCount++;
        }

        startButton.GetComponent<Button>().interactable = (filledCount >= 1 && !isBaking);
    }

    public void StartMixing()
    {
        Debug.Log(parent);
        Debug.Log(GetSlotContents());
        string key = cookbook.GetKey(GetSlotContents());
        //string key = gameObject.GetComponent<RecipeDictionary>().GetKey(GetSlotContents());

        if (cookbook.MixDict.ContainsKey(key))
        {
            BackButtonKitchen();
            minigame.StartMinigame(); // Start the minigame independently of the timer
            StartCoroutine(MixingProcess(cookbook.MixDict[key]));
        }
        else
        {
            foreach (GameObject s in cook_it)
            {
                ReturnItems(s);
            }
        }
    }   

    private IEnumerator MixingProcess(recipe r)
    {

        isBaking = true;
        float timer = bakingTime;

        // Clear the UI slots at the start of baking
        /*foreach (GameObject slotObj in cook_it)
        {
            Clearslot(slotObj);
        }*/
        int b = batchcount();

        // Start the baking timer
        while (timer > 0)
        {
            timerText.text = "Mixing... " + Mathf.Ceil(timer) + "s";
            yield return new WaitForSeconds(1f);
            timer--;
        }

        // Baking complete
        timerText.text = "";
        isBaking = false;
        Vector3 spawn_coords = spawner.transform.position;
        GameObject output = (GameObject) Instantiate(Resources.Load(path + r.product), spawn_coords, Quaternion.identity);
        CollectibleItem i = output.GetComponent<CollectibleItem>();
        i.quantity = r.batchSize * b;
    }

    string[] GetSlotContents()
    {
        Debug.Log(parent);
        int i = 0;
        string[] ingredients = new string[SLOT_COUNT];

        foreach (GameObject j in cook_it)
        {
            ingredients[i] = "";
            GameObject qty = j.transform.Find("qty").gameObject;
            Text t = qty.GetComponent<Text>();
            Slot s = j.GetComponent<Slot>();
            if (s.filled)
            {
                Debug.Log(s.name);
                Debug.Log(csub.ContainsKey(s.name));
                ingredients[i] = s.name;
            }
            i++;
            
        }

        return ingredients;
    }

    void ReturnItems(GameObject slotObj)
    {
        Debug.Log(parent);
        Slot slot = slotObj.GetComponent<Slot>();
        Debug.Log(slot.name);
        Debug.Log(csub.ContainsKey(slot.name));
        if (slot.filled)
        {
            f.AddToFridge(slot.name, csub[slot.name]);

            Clearslot(slotObj);
        }
    }

    void Clearslot(GameObject slotObj)
    {
        Debug.Log(parent);
        Slot slot = slotObj.GetComponent<Slot>();
        if (slot.filled)
        {
            slot.filled = false;
            csub[slot.name] = 0;
            csub.Remove(slot.name);
            slot.name = "";

            // Hide the item UI
            Transform itemContainer = slotObj.transform.Find("Items");
            foreach (Transform item in itemContainer)
            {
                item.gameObject.SetActive(false);
            }

            // Reset the quantity text
            Text qtyText = slotObj.transform.Find("qty").GetComponent<Text>();
            qtyText.text = "";
        }
    }

    int batchcount()
    {
        int batches = 0;
        bool hasIngredients = true;
        while (hasIngredients)
        {
            batches++;
            foreach (GameObject slotObj in cook_it)
            {
                Slot s = slotObj.GetComponent<Slot>();
                GameObject qty = slotObj.transform.Find("qty").gameObject;
                Text t = qty.GetComponent<Text>();
                if (s.filled)
                {
                    csub[s.name] -= 1;
                    t.text = "x" + csub[s.name];
                    if (csub[s.name] <= 0)
                    {
                        hasIngredients = false;
                        Clearslot(slotObj);
                    }
                }
            }
        }
        return batches;
    }
}
