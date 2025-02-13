using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Required for UI elements

public class CanvasInventory : MonoBehaviour
{
    public Transform items;
    List<GameObject> it = new List<GameObject>();
    public Inventory inv;
    public Dictionary<string, int> sub = new Dictionary<string, int>();
    public Transform fridge_items; 
    List<GameObject> fr_it = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform i in items){
            it.Add(i.gameObject);
        }
        foreach(Transform k in fridge_items){
            fr_it.Add(k.gameObject);
        }
    }

    public void AddItems(string name, int quantity){
        foreach(GameObject j in it){
            Slot s = j.GetComponent<Slot>();
            if(!s.filled && !sub.ContainsKey(name)){
                //check the name of the item
                //check the quantity
                //set active the icon in that slot 
                //update the qty according to the item
                GameObject fridge_slot = fr_it.Find(obj => obj.name == j.name);
                switch (name){
                    case "apple":
                    case "banana":
                    case "Apple": 
                    case "Banana":
                    case "quaso":
                        GameObject temp = j.transform.Find(name).gameObject;
                        GameObject temp_f = fridge_slot.transform.Find("Items").Find(name).gameObject;

                        GameObject prep = fridge_slot.transform.Find("Prep").gameObject;

                        GameObject qty = j.transform.Find("qty").gameObject; 
                        GameObject qty_f = fridge_slot.transform.Find("qty").gameObject; 

                        Text t = qty.GetComponent<Text>();
                        Text t_f = qty_f.GetComponent<Text>();

                        temp.SetActive(true);
                        temp_f.SetActive(true);
                        prep.SetActive(true);

                        t.text= "x" + quantity;
                        t_f.text = "x" + quantity;

                        s.filled = true;
                        fridge_slot.GetComponent<Slot>().filled = true;

                        s.name = name;
                        fridge_slot.GetComponent<Slot>().name = name;

                        sub.Add(name, quantity);
                        if(name == "apple"){
                            s.action = "chopping";
                            fridge_slot.GetComponent<Slot>().action = "chopping";
                        }
                        else if(name == "banana"){
                            s.action = "baking";
                            fridge_slot.GetComponent<Slot>().action = "baking";
                        }
                        else{
                            s.action = "food";
                            fridge_slot.GetComponent<Slot>().action = "food";
                        }
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
                GameObject fridge_slot = fr_it.Find(obj => obj.name == j.name);
                switch (name){
                    case "apple":
                    case "banana":
                    case "Apple": 
                    case "Banana":
                    case "quaso":
                        Debug.Log("test2");
                        GameObject temp = j.transform.Find(name).gameObject;
                        GameObject temp_f = fridge_slot.transform.Find("Items").Find(name).gameObject;

                        GameObject prep = fridge_slot.transform.Find("Prep").gameObject;

                        GameObject qty = j.transform.Find("qty").gameObject; 
                        GameObject qty_f = fridge_slot.transform.Find("qty").gameObject; 
                        

                        Text t = qty.GetComponent<Text>();
                        Text t_f = qty_f.GetComponent<Text>();
                        
                        temp.SetActive(true);
                        temp_f.SetActive(true);
                        prep.SetActive(true);

                        t.text= "x" + (sub[name]+1);
                        t_f.text= "x" + (sub[name]+1);

                        sub[name] +=1;
                        if(name == "apple"){
                            s.action = "chopping";
                            fridge_slot.GetComponent<Slot>().action = "chopping";
                        }
                        else if(name == "banana"){
                            s.action = "baking";
                            fridge_slot.GetComponent<Slot>().action = "baking";
                        }
                        else{
                            s.action = "food";
                            fridge_slot.GetComponent<Slot>().action = "food";
                        }
                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }

            }

        }
    }

    public void DeleteItems(string name, int quantity){
        foreach(GameObject j in it){
            Slot s = j.GetComponent<Slot>();
            if(s.filled && s.name == name){
                Debug.Log("test1");
                //check the name of the item
                //check the quantity
                //set active the icon in that slot 
                //update the qty according to the item
                GameObject fridge_slot = fr_it.Find(obj => obj.name == j.name);
                switch (name){
                    case "apple":
                    case "banana":
                        Debug.Log("test2");
                        GameObject temp = j.transform.Find(name).gameObject;
                        GameObject temp_f = fridge_slot.transform.Find("Items").Find(name).gameObject;

                        GameObject prep = fridge_slot.transform.Find("Prep").gameObject;

                        GameObject qty = j.transform.Find("qty").gameObject; 
                        GameObject qty_f = fridge_slot.transform.Find("qty").gameObject; 

                        Text t = qty.GetComponent<Text>();
                        Text t_f = qty_f.GetComponent<Text>();
                        sub[name] -=1;

                        if(sub[name] <= 0){
                         temp.SetActive(false);
                         temp_f.SetActive(false);

                         t.text = "x0";
                         t_f.text = "x0";

                        prep.SetActive(false);
                         break;   
                        }
                        temp.SetActive(true);
                        temp_f.SetActive(true);

                        t.text= "x" + (sub[name]);
                        t_f.text= "x" + (sub[name]);

                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }

            }

        }
    }
}
