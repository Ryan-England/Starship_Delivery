using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class RecipeDictionary : MonoBehaviour
{
    public Dictionary<string, string> Items = new Dictionary<string, string>();
    public Dictionary<string, recipe> MixDict = new Dictionary<string, recipe>();
    public Dictionary<string, recipe> BakeDict = new Dictionary<string, recipe>();
    // Start is called before the first frame update
    void Start()
    {
        LoadItemDict();
        BakeDict.Add("smolderdough", new recipe("quaso", 1));
        MixDict.Add("applebanana", new recipe("ice", 1));
        MixDict.Add("cinderwheatsalt", new recipe("smolderdough", 1));
        MixDict.Add("glegle", new recipe("salad", 1));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetKey(string[] ingredients)
    {
        UnityEngine.Debug.Log(ingredients);
        System.Array.Sort(ingredients);
        UnityEngine.Debug.Log(ingredients);
        string key = "";
        for (int i = 0; i < ingredients.Length; i++)
        {
            UnityEngine.Debug.Log(ingredients[i]);
            key += ingredients[i];
        }
        UnityEngine.Debug.Log("Key: " + key);
        return key;
    }

    private void LoadItemDict()
    {
        Items.Add("apple", "ingredient");
        Items.Add("banana", "ingredient");
        Items.Add("cinderwheat", "ingredient");
        Items.Add("salt", "ingredient");
        Items.Add("smolderdough", "ingredient");
        Items.Add("glegle", "ingredient");
        Items.Add("quaso", "food");
        Items.Add("ice", "food");
        Items.Add("salad", "food");

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

