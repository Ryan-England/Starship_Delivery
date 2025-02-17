using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue
{
    public string name;


    [TextArea(5, 10)]
    public string[] sentences;
    public string[] quest_check;
    public string[] quest_complete;
    public string[] choices;
}
