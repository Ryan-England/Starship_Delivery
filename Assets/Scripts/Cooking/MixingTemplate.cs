using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Required for UI elements
using UnityEngine.EventSystems;

public class MixingTemplate : MonoBehaviour
{
    public PlayerMovement pm; 
    public GameObject kitchenUI;
    public GameObject start;
    List<GameObject> cook_it = new List<GameObject>();
    public Transform cook_items;
    public Fridge f; 

    Dictionary<string, int> csub = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform k in cook_items){
            cook_it.Add(k.gameObject);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void KitchenMenu(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pm.enabled = false;
        Time.timeScale = 0f;
        kitchenUI.SetActive(true);
    }

    //basically we want to check if the items have a 
    public void BackButtonKitchen(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pm.enabled =true;
        kitchenUI.SetActive(false);
    }

    public void MoveToCook(){
        //go inside the canvas inventory next to the fridge
        GameObject temp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        // GameObject temp = gameObject.transform.parent.gameObject;
        Slot s  = temp.GetComponent<Slot>();
        if(s.filled && s.action == "chopping"){
            AddToCook(s.name, 1);
        }
    }

    public void AddToCook(string name, int quantity){
        foreach(GameObject j in cook_it){
             GameObject temp = j.transform.Find("Items").Find(name).gameObject;
             GameObject qty = j.transform.Find("qty").gameObject; 
             Text t = qty.GetComponent<Text>();
             Slot s = j.GetComponent<Slot>();

             if(!s.filled && !csub.ContainsKey(name)){
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

                        t.text= "x" + quantity;

                        s.filled = true;

                        s.name = name;

                        csub.Add(name, quantity);
                        
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
                        temp.SetActive(true);

                        if(csub[name]+1 <= f.fsub[name]){
                            t.text= "x" + (csub[name]+1);
                            csub[name] +=1;
                        }
                        else{
                            t.text= "x" + (csub[name]);
                        }
                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }

            }
            f.DeleteItems(name, 1);
        }
    }
}
