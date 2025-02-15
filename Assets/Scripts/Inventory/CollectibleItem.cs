using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName;
    public int quantity;
    public Inventory.ItemType type;
    private bool harvested = false;
    [SerializeField]
    private bool regenerating = false;
    [SerializeField]
    private int regenTime = 30;
    [SerializeField]
    private GameObject FreshModel;
    [SerializeField]
    private GameObject DepleteModel; //collectables can optionally be given a child mesh, which is rendered only when the resource is depleted. Otherwise, the resource just disappears.

    private void Start()
    {
        
        if (DepleteModel != null) 
        {
            DepleteModel.transform.GetComponent<Renderer>().enabled = false;
        }
        
    }


    public void harvest() //Destroys the resource if nonrenewable, depletes it otherwise
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }

        if (!regenerating)
        {
            Destroy(this.gameObject);
        }

        harvested = true;
        gameObject.tag = "Harvested";

        visibilityToggle();

        StartCoroutine(timedRefresh());

    }

    public void refresh()
    {
        harvested = false;
        gameObject.tag = "Collectible";


        visibilityToggle();
    }

    private void visibilityToggle()
    {
        //Script can be applied to a mesh directly or to a holder object
        if (FreshModel != null)
        {
            FreshModel.transform.GetComponent<Renderer>().enabled = !harvested;
        }
        else
        {
            gameObject.transform.GetComponent<Renderer>().enabled = !harvested;
        }

        if (DepleteModel != null)
        {
            DepleteModel.transform.GetComponent<Renderer>().enabled = harvested;
        }

        DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        if (dialogueTrigger != null)
        {
            dialogueTrigger.enabled = !harvested;
        }
    }


    private IEnumerator timedRefresh()
    {
        yield return new WaitForSeconds(regenTime);

        refresh();
    }


}


