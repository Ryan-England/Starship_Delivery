using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    public int ans = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Opt1(){
        Debug.Log("option 1 pressed.");
        ans = 1;
    }
    public void Opt2(){
        Debug.Log("option 2 pressed.");
        ans = 2;
    }
}
