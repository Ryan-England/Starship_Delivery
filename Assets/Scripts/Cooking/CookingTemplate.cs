using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CookingTemplate : MonoBehaviour
{
    public PlayerMovement pm; 
    public GameObject kitchenUI;
    public GameObject startButton;
    public Text timerText;
    public float cookingTime = 5f;
    private bool isCooking = false;

    List<GameObject> cook_it = new List<GameObject>();
    public Transform cook_items;
    public Fridge f;

    Dictionary<string, int> csub = new Dictionary<string, int>();

    void Start()
    {
        foreach (Transform k in cook_items)
        {
            cook_it.Add(k.gameObject);
        }
        startButton.GetComponent<Button>().interactable = false;
    }

    void Update()
    {
        CheckSlots();
    }

    public void KitchenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pm.enabled = false;
        Time.timeScale = 0f;
        kitchenUI.SetActive(true);
    }

    public void BackButtonKitchen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pm.enabled = true;
        kitchenUI.SetActive(false);
    }

    public void MoveToCook()
    {
        GameObject temp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        Slot s = temp.GetComponent<Slot>();

        if (s.filled && s.action == "chopping")
        {
            AddToCook(s.name, 1);
        }
    }

    public void AddToCook(string name, int quantity)
    {
        foreach (GameObject j in cook_it)
        {
            GameObject temp = j.transform.Find("Items").Find(name).gameObject;
            GameObject qty = j.transform.Find("qty").gameObject;
            Text t = qty.GetComponent<Text>();
            Slot s = j.GetComponent<Slot>();

            if (!s.filled)
            {
                temp.SetActive(true);
                t.text = "x" + quantity;
                s.filled = true;
                s.name = name;

                if (!csub.ContainsKey(name))
                {
                    csub.Add(name, quantity);
                }
                else
                {
                    csub[name] += 1;
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
        startButton.GetComponent<Button>().interactable = (filledCount >= 2 && !isCooking);
    }

    public void StartCooking()
    {
        if (!isCooking)
        {
            StartCoroutine(CookingProcess());
        }
    }

    private IEnumerator CookingProcess()
    {
        isCooking = true;
        float timer = cookingTime;

        foreach (GameObject slotObj in cook_it)
        {
            Slot slot = slotObj.GetComponent<Slot>();
            if (slot.filled)
            {
                slot.filled = false;
                slot.name = "";
                Transform itemContainer = slotObj.transform.Find("Items");
                foreach (Transform item in itemContainer)
                {
                    item.gameObject.SetActive(false);
                }
                Text qtyText = slotObj.transform.Find("qty").GetComponent<Text>();
                qtyText.text = "";
            }
        }

        while (timer > 0)
        {
            timerText.text = "Cooking... " + Mathf.Ceil(timer) + "s";
            yield return new WaitForSeconds(1f);
            timer--;
        }

        timerText.text = "Done Cooking!";
        isCooking = false;
    }
}
