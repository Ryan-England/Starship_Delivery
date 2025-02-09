using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Required for UI elements

public class CanvasInventory : MonoBehaviour
{
    public Transform items;
    List<GameObject> it = new List<GameObject>();
    public Inventory inv;
    Dictionary<string, int> sub = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform i in items){
            it.Add(i.gameObject);
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
                switch (name){
                    case "apple":
                    case "banana":
                        GameObject temp = j.transform.Find(name).gameObject;
                        GameObject qty = j.transform.Find("qty").gameObject; 
                        Text t = qty.GetComponent<Text>();
                        temp.SetActive(true);
                        t.text= "x" + quantity;
                        s.filled = true;
                        s.name = name;
                        sub.Add(name, quantity);
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
                        Debug.Log("test2");
                        GameObject temp = j.transform.Find(name).gameObject;
                        GameObject qty = j.transform.Find("qty").gameObject; 
                        Text t = qty.GetComponent<Text>();
                        temp.SetActive(true);
                        t.text= "x" + (sub[name]+1);
                        sub[name] +=1;
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
                switch (name){
                    case "apple":
                    case "banana":
                        Debug.Log("test2");
                        GameObject temp = j.transform.Find(name).gameObject;
                        GameObject qty = j.transform.Find("qty").gameObject; 
                        Text t = qty.GetComponent<Text>();
                        if(quantity-1 <= 0){
                         temp.SetActive(false);
                         t.text = "x0";
                         break;   
                        }
                        temp.SetActive(true);
                        t.text= "x" + (sub[name]-1);
                        sub[name] -=1;
                        break;
                    default:
                        Debug.Log("sdf;lks;dlfk;sldkf");
                        break;
                }

            }

        }
    }
}
