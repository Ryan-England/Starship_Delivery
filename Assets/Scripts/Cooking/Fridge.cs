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
    public Transform items;
    List<GameObject> fridge_it = new List<GameObject>();
    List<GameObject> inv_it = new List<GameObject>();
    Dictionary<string, int> fsub = new Dictionary<string, int>();
    public CanvasInventory ci; 

    void Start(){
        foreach(Transform k in fridge_items){
            fridge_it.Add(k.gameObject);
        }
        foreach(Transform j in items){
            inv_it.Add(j.gameObject);
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
        AddToFridge(s.name, 1);
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
                        GameObject temp = j.transform.Find("Items").Find(name).gameObject;

                        GameObject qty = j.transform.Find("qty").gameObject; 

                        Text t = qty.GetComponent<Text>();

                        temp.SetActive(true);

                        t.text= "x" + quantity;

                        s.filled = true;

                        s.name = name;

                        fsub.Add(name, quantity);
                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }

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
                        Debug.Log("test2");
                        GameObject temp = j.transform.Find("Items").Find(name).gameObject;


                        GameObject qty = j.transform.Find("qty").gameObject; 
                        

                        Text t = qty.GetComponent<Text>();
                        
                        temp.SetActive(true);
                        if(fsub[name]+1 <= ci.sub[name]){
                            t.text= "x" + (fsub[name]+1);
                            fsub[name] +=1;
                        }
                        else{
                            t.text= "x" + (fsub[name]);
                        }
                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }

            }
            ci.DeleteItems(name, 1);
        }
    }
    

}
