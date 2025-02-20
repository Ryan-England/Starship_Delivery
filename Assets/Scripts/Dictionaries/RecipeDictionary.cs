using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Data.Common;
using Unity.VisualScripting.ReorderableList;

public class RecipeDictionary : MonoBehaviour
{
    public Dictionary<string, recipe> MixDict = new Dictionary<string, recipe>();
    public Dictionary<string, recipe> BakeDict = new Dictionary<string, recipe>();
    // Start is called before the first frame update
    void Start()
    {
        BakeDict.Add("smolderdough", new recipe("quaso", 1));
        MixDict.Add("applebanana", new recipe("ice", 1));
        MixDict.Add("cinderwheatsalt", new recipe("smolderdough", 1));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetKey(string[] ingredients)
    {
        Debug.Log(ingredients);
        System.Array.Sort(ingredients);
        Debug.Log(ingredients);
        string key = "";
        for (int i = 0; i < ingredients.Length; i++)
        {
            Debug.Log(ingredients[i]);
            key += ingredients[i];
        }
        Debug.Log("Key: " + key);
        return key;
    }
}

public struct recipe
{
    public string product { get; }
    public int batchSize { get; }

    public recipe(string p, int b)
    {
        product = p;
        batchSize = b;
    }


}

