using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Required for UI elements
using UnityEngine.EventSystems;
public class Fridge : MonoBehaviour
{

    public GameObject FridgeUI;
    public PlayerMovement pm; 
    public Transform fridge_items;
    public Transform chop_items;
    public Transform bake_items;

    public Transform items;
    List<GameObject> fridge_it = new List<GameObject>();
    List<GameObject> chop_it = new List<GameObject>();
    List<GameObject> bake_it = new List<GameObject>();
    List<GameObject> inv_it = new List<GameObject>();
    public Dictionary<string, int> fsub = new Dictionary<string, int>();
    public CanvasInventory ci; 
    public Inventory inv;

    void Start(){
        foreach(Transform k in fridge_items){
            fridge_it.Add(k.gameObject);
        }
        foreach(Transform j in items){
            inv_it.Add(j.gameObject);
        }
        foreach(Transform i in chop_items){
            chop_it.Add(i.gameObject);
        }   
        foreach(Transform f in bake_items){
            bake_it.Add(f.gameObject);
        }       
    }
    public void FridgeMenu(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pm.enabled = false;
        Time.timeScale = 0f;
        FridgeUI.SetActive(true);
    }

    //basically we want to check if the items have a 
    public void BackButtonFridge(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pm.enabled =true;
        FridgeUI.SetActive(false);
    }

    public void Prep(){
        //go inside the canvas inventory next to the fridge
        GameObject temp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        // GameObject temp = gameObject.transform.parent.gameObject;
        Slot s  = temp.GetComponent<Slot>();
        if(s.action != "food"){AddToFridge(s.name, 1);}
    }

    //add items
    //add them to the inventory as well
    //check the fridge ui inventory and make the prep buttons visible for every item
    // if an item gets removed and it's now gone, then we need to remove the prep buttons for that slot
    // the prep function basically needs to do the same thing as the AddItems function and it needs to add ONE of the current item into the fridge
            // remove one of that item from the inventory
            // add one of that item to the fridge
            // can we use an inventory object for this?
            //honestly yeah probably, inventory script at the end of the day is just a generic that can be applied to the fridge
    public void AddToFridge(string name, int quantity){
        foreach(GameObject j in fridge_it){
            GameObject chop_slot = chop_it.Find(obj => obj.name == j.name);
            GameObject bake_slot = bake_it.Find(obj => obj.name == j.name);
            
            GameObject temp = j.transform.Find("Items").Find(name).gameObject;
            GameObject temp_c = chop_slot.transform.Find("Items").Find(name).gameObject;
            GameObject temp_b = bake_slot.transform.Find("Items").Find(name).gameObject;

            GameObject qty = j.transform.Find("qty").gameObject; 
            GameObject qty_c = chop_slot.transform.Find("qty").gameObject; 
            GameObject qty_b = bake_slot.transform.Find("qty").gameObject; 

            Text t = qty.GetComponent<Text>();
            Text t_c = qty_c.GetComponent<Text>();
            Text t_b = qty_b.GetComponent<Text>();

            Slot s = j.GetComponent<Slot>();
            if(!s.filled && !fsub.ContainsKey(name)){
                //check the name of the item
                //check the quantity
                //set active the icon in that slot 
                //update the qty according to the item
                switch (name){
                    case "apple":
                    case "banana":
                    case "Apple": 
                    case "Banana":
                        temp.SetActive(true);
                        temp_c.SetActive(true);
                        temp_b.SetActive(true);

                        t.text= "x" + quantity;
                        t_c.text = "x" + quantity;
                        t_b.text = "x" + quantity;

                        s.filled = true;
                        chop_slot.GetComponent<Slot>().filled = true;
                        bake_slot.GetComponent<Slot>().filled = true;

                        s.name = name;
                        chop_slot.GetComponent<Slot>().name = name;
                        bake_slot.GetComponent<Slot>().name = name;

                        fsub.Add(name, quantity);
                        if(name == "apple"){
                            s.action = "chopping";
                            chop_slot.GetComponent<Slot>().action = "chopping";
                            bake_slot.GetComponent<Slot>().action = "chopping";

                        }
                        else if(name == "banana"){
                            s.action = "baking";
                            chop_slot.GetComponent<Slot>().action = "baking";
                            bake_slot.GetComponent<Slot>().action = "baking";
                        }
                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }
                inv.RemoveItem(name, Inventory.ItemType.Ingredient, 1);
                ci.DeleteItems(name, 1);
            }
            else if(s.filled && s.name == name){
                Debug.Log("test1");
                //check the name of the item
                //check the quantity
                //set active the icon in that slot 
                //update the qty according to the item
                switch (name){
                    case "apple":
                    case "banana":
                    case "Apple": 
                    case "Banana":
                        temp.SetActive(true);
                        temp_c.SetActive(true);
                        temp_b.SetActive(true);
                        fsub[name] +=1;

                        t.text= "x" + (fsub[name]);
                        t_c.text= "x" + (fsub[name]);
                        t_b.text= "x" + (fsub[name]);
                        if(name == "apple"){
                            s.action = "chopping";
                            chop_slot.GetComponent<Slot>().action = "chopping";
                            bake_slot.GetComponent<Slot>().action = "chopping";

                        }
                        else if(name == "banana"){
                            s.action = "baking";
                            chop_slot.GetComponent<Slot>().action = "baking";
                            bake_slot.GetComponent<Slot>().action = "baking";
                        }
                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }
                inv.RemoveItem(name, Inventory.ItemType.Ingredient, 1);
                ci.DeleteItems(name, 1);

            }
            // ci.DeleteItems(name, 1);
        }
    }
    public void DeleteItems(string name, int quantity){
        Debug.Log("1");
        foreach(GameObject j in fridge_it){
            Debug.Log("2");
            GameObject chop_slot = chop_it.Find(obj => obj.name == j.name);
            GameObject bake_slot = bake_it.Find(obj => obj.name == j.name);

            GameObject temp = j.transform.Find("Items").Find(name).gameObject;
            GameObject temp_c = chop_slot.transform.Find("Items").Find(name).gameObject;
            GameObject temp_b = bake_slot.transform.Find("Items").Find(name).gameObject;

            GameObject qty = j.transform.Find("qty").gameObject; 
            GameObject qty_c = chop_slot.transform.Find("qty").gameObject; 
            GameObject qty_b = bake_slot.transform.Find("qty").gameObject; 

            Text t = qty.GetComponent<Text>();
            Text t_c = qty_c.GetComponent<Text>();
            Text t_b = qty_b.GetComponent<Text>();

            Slot s = j.GetComponent<Slot>();
            Debug.Log("3");
            if(s.filled){
                Debug.Log("test1");
                //check the name of the item
                //check the quantity
                //set active the icon in that slot 
                //update the qty according to the item
                switch (name){
                    case "apple":
                    case "banana":
                    Debug.Log("4");
                        Debug.Log("test2");
                        fsub[name] -=1;

                        if(fsub[name] <= 0){
                         temp.SetActive(false);
                         temp_c.SetActive(false);
                         temp_b.SetActive(false);

                         fsub[name] = 0;
                         t.text = "x0";
                         t_c.text = "x0";
                         t_b.text = "x0";

                         break;   
                        }
                        temp.SetActive(true);
                        temp_c.SetActive(true);
                        temp_b.SetActive(true);

                        t.text= "x" + (fsub[name]);
                        t_c.text= "x" + (fsub[name]);
                        t_b.text= "x" + (fsub[name]);

                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }

            }

        }
    }

}