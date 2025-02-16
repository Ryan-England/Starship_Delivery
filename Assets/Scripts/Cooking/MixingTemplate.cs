using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Required for UI elements
using UnityEngine.EventSystems;

public class MixingTemplate : MonoBehaviour
{
    public PlayerMovement pm; 
    public GameObject kitchenUI;
    public GameObject startButton;
    public Text timerText; // Assign in Unity
    public float bakingTime = 5f; // Adjust as needed
    private bool isBaking = false;

    List<GameObject> cook_it = new List<GameObject>();
    public Transform cook_items;
    public Fridge f; 

    Dictionary<string, int> csub = new Dictionary<string, int>();
    public GameObject quaso;
    public GameObject spawner;
    void Start()
    {
        foreach(Transform k in cook_items){
            cook_it.Add(k.gameObject);
        }   
        startButton.GetComponent<Button>().interactable = false; // Disable button initially
    }

    void Update()
    {
        CheckSlots(); // Continuously check if the button should be enabled
    }

    public void KitchenMenu(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pm.enabled = false;
        Time.timeScale = 0f;
        kitchenUI.SetActive(true);
    }

    public void BackButtonKitchen(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pm.enabled = true;
        kitchenUI.SetActive(false);
    }

    public void MoveToCook(){
        GameObject temp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        Slot s = temp.GetComponent<Slot>();

        if(s.filled && s.action == "mixing"){
            AddToCook(s.name, 1);
        }
    }

    public void AddToCook(string name, int quantity){
        foreach(GameObject j in cook_it){
             GameObject temp = j.transform.Find("Items").Find(name).gameObject;
             GameObject qty = j.transform.Find("qty").gameObject; 
             Text t = qty.GetComponent<Text>();
             Slot s = j.GetComponent<Slot>();

             if(!s.filled){
                temp.SetActive(true);
                t.text= "x" + quantity;
                s.filled = true;
                s.name = name;

                if(!csub.ContainsKey(name)){
                    csub.Add(name, quantity);
                }
                else{
                    csub[name] +=1; 
                }

                f.DeleteItems(name, 1);
                return;
            }
        }
    }

    private void CheckSlots()
    {
        int filledCount = 0;
        foreach (GameObject slotObj in cook_it)
        {
            Slot slot = slotObj.GetComponent<Slot>();
            if (slot.filled) filledCount++;
        }

        startButton.GetComponent<Button>().interactable = (filledCount >= 2 && !isBaking);
    }

    public void StartMixing()
    {
        if (!isBaking)
        {
            StartCoroutine(MixingProcess());
        }
    }

    private IEnumerator MixingProcess()
{
    isBaking = true;
    float timer = bakingTime;

    // Clear the UI slots at the start of baking
    foreach (GameObject slotObj in cook_it)
    {
        Slot slot = slotObj.GetComponent<Slot>();
        if (slot.filled)
        {
            slot.filled = false;
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
    Instantiate(quaso, spawn_coords, Quaternion.identity);
}

}
