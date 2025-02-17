using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    public int ans = 0;

    public void Opt1(){
        Debug.Log("option 1 pressed.");
        ans = 1;
    }
    public void Opt2(){
        Debug.Log("option 2 pressed.");
        ans = 2;
    }
}
