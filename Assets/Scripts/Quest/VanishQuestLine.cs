using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishQuestLine : MonoBehaviour
{
    public GameObject q;
    public void Vanish(){
        q.SetActive(false);
    }
}
