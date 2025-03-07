using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    private PointerEventData eventData;
    private GameObject box;
    private GameObject txt;
    private Text tttext;
    // Start is called before the first frame update
    void Start()
    {
        
        //eventSystem = GetComponent<EventSystem>();
        //raycaster = GetComponent<GraphicRaycaster>();
        box = gameObject.transform.Find("background").gameObject;
        txt = gameObject.transform.Find("txt").gameObject;
        tttext = txt.GetComponent<Text>();
        tttext.text = "ball";
        box.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Input.mousePosition.x + 100, Input.mousePosition.y - 60, 0);

        eventData = new PointerEventData(eventSystem);
        
        eventData.position = Input.mousePosition;
        //Debug.Log("x " + eventData.position.x.ToString() + " y " + eventData.position.y.ToString());
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);
        Debug.Log(results.Count);
        Slot s = FindSlot(results);
        if (s != null)
        {
            //Debug.Log("Slot found!");
            tttext.text = s.name;
            box.SetActive(true);
        }
        else
        {
            //Debug.Log("No slot found.");
            tttext.text = "";
            box.SetActive(false);
        }        
    }

    private Slot FindSlot(List<RaycastResult> results)
    {
        foreach (RaycastResult r in results)
        {
            
            GameObject g = r.gameObject;
            Debug.Log(g.name);
            Slot slot = g.GetComponent<Slot>();
            if (slot != null && slot.filled)
            {
                return(slot);
            }
        }
        return (null);
    }
}
