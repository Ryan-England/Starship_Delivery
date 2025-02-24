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
    public RecipeDictionary dict; 
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform i in items){
            it.Add(i.gameObject);
        }
        foreach(Transform k in fridge_items){
            fr_it.Add(k.gameObject);
        }

        if (dict == null) {
            Debug.Log("Recipe dictionary has not been supplied! Please link the RecipeDictionary script attached to the GameManager to the CanvasInventory script attached to " + gameObject.name);
        }
    }

    //Attempts to place a certain quantity of a given item into the player's inventory. If successful returns true, otherwise returns false
    public bool AddItems(string name, int quantity){

        if (dict.Items.ContainsKey(name))
        {
            foreach (GameObject j in it) {
                Slot s = j.GetComponent<Slot>();
                string path = "Icons";

                if (!s.filled && !sub.ContainsKey(name)) {
                    //check the name of the item
                    //check the quantity
                    //set active the icon in that slot 
                    //update the qty according to the item
                    GameObject fridge_slot = fr_it.Find(obj => obj.name == j.name);


                    GameObject temp = j.transform.Find("apple").gameObject;
                    GameObject temp_f = fridge_slot.transform.Find("Items").Find("apple").gameObject;

                    Image test = temp.GetComponent<Image>();
                    Debug.Log(test + "skjdflksdjflksjdklfj " + temp);
                    test.sprite = Resources.Load<Sprite>(path + "/" + name);
                    Image test2 = temp_f.GetComponent<Image>();
                    test2.sprite = Resources.Load<Sprite>(path + "/" + name);

                    GameObject prep = fridge_slot.transform.Find("Prep").gameObject;

                    GameObject qty = j.transform.Find("qty").gameObject;
                    GameObject qty_f = fridge_slot.transform.Find("qty").gameObject;

                    Text t = qty.GetComponent<Text>();
                    Text t_f = qty_f.GetComponent<Text>();

                    temp.SetActive(true);
                    temp_f.SetActive(true);
                    prep.SetActive(true);

                    t.text = "x" + quantity;
                    t_f.text = "x" + quantity;

                    s.filled = true;
                    fridge_slot.GetComponent<Slot>().filled = true;

                    s.name = name;
                    fridge_slot.GetComponent<Slot>().name = name;

                    sub.Add(name, quantity);

                    s.action = dict.Items[name];
                    fridge_slot.GetComponent<Slot>().action = dict.Items[name];

                    return (true);

                

                }
                else if (s.filled && s.name == name) {
                    Debug.Log("test1");
                    //check the name of the item
                    //check the quantity
                    //set active the icon in that slot 
                    //update the qty according to the item
                    GameObject fridge_slot = fr_it.Find(obj => obj.name == j.name);
                    
                    Debug.Log("test2");
                    GameObject temp = j.transform.Find("apple").gameObject;
                    GameObject temp_f = fridge_slot.transform.Find("Items").Find("apple").gameObject;

                    Image test = temp.GetComponent<Image>();
                    test.sprite = Resources.Load<Sprite>(path + "/" + name);
                    Image test2 = temp_f.GetComponent<Image>();
                    test2.sprite = Resources.Load<Sprite>(path + "/" + name);

                    GameObject prep = fridge_slot.transform.Find("Prep").gameObject;

                    GameObject qty = j.transform.Find("qty").gameObject;
                    GameObject qty_f = fridge_slot.transform.Find("qty").gameObject;


                    Text t = qty.GetComponent<Text>();
                    Text t_f = qty_f.GetComponent<Text>();

                    temp.SetActive(true);
                    temp_f.SetActive(true);
                    prep.SetActive(true);

                    sub[name] += quantity;
                    t.text = "x" + (sub[name]);
                    t_f.text = "x" + (sub[name]);

                    s.action = dict.Items[name];
                    fridge_slot.GetComponent<Slot>().action = dict.Items[name];

                    return (true);
                    
                    

                }

            }
            Debug.Log("couldn't find a slot for " + name);

        }
        else
        {
            Debug.Log("Item " + name + " is not in dictionary");
        }
        Debug.Log("returning false");
        return (false);
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
                
                Debug.Log("test2");
                GameObject temp = j.transform.Find("apple").gameObject;
                GameObject temp_f = fridge_slot.transform.Find("Items").Find("apple").gameObject;

                GameObject prep = fridge_slot.transform.Find("Prep").gameObject;

                GameObject qty = j.transform.Find("qty").gameObject; 
                GameObject qty_f = fridge_slot.transform.Find("qty").gameObject; 

                Text t = qty.GetComponent<Text>();
                Text t_f = qty_f.GetComponent<Text>();
                sub[name] -= quantity;

                if(sub[name] <= 0)
                {
                    s.filled = false;
                    temp.SetActive(false);
                    temp_f.SetActive(false);

                    t.text = "x0";
                    t_f.text = "x0";

                    prep.SetActive(false); 

                    sub.Remove(name);
                } 
                else 
                { 
                    temp.SetActive(true);
                    temp_f.SetActive(true);

                    t.text= "x" + (sub[name]);
                    t_f.text= "x" + (sub[name]);
                }
                

            }

        }
    }
}
