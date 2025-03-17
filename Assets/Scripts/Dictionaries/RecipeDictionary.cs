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
        LoadMixDict();
        LoadBakeDict();
        
        

        
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
        //placeholder ingredients
        Items.Add("apple", "ingredient");
        Items.Add("banana", "ingredient");
        Items.Add("glegle", "ingredient");

        //Implemented ingredients
        Items.Add("cinderwheat", "ingredient");
        Items.Add("salt", "ingredient");
        Items.Add("lactite", "ingredient");
        Items.Add("smolderdough", "ingredient");
        Items.Add("silkydough", "ingredient");
        Items.Add("krocream", "ingredient");

        //Ingredients needing sprites
        Items.Add("mohofruit", "ingredient"); //TL note: the moho is the boundary between the crust and the mantle
        Items.Add("mohokrocream", "ingredient"); //could be used as a food item as well
        Items.Add("mohojam", "ingredient");
        Items.Add("mohofilling", "ingredient");

        //placeholder foods
        Items.Add("ice", "food");
        Items.Add("salad", "food");

        //Implemented foods
        Items.Add("cinderloaf", "food");
        Items.Add("quaso", "food");

        //Foods needing sprites
        Items.Add("mohotart", "food");
        Items.Add("krocreambun", "food");
        Items.Add("mohopie", "food");
    }

    private void LoadMixDict()
    {
        //placeholder recipes
        MixDict.Add("applebanana", new recipe("ice", 1));
        MixDict.Add("glegle", new recipe("salad", 1));

        //Implemented recipes
        MixDict.Add("cinderwheatsalt", new recipe("smolderdough", 1));
        MixDict.Add("lactitesmolderdough", new recipe("silkydough", 1));
        MixDict.Add("lactitesalt", new recipe("krocream", 1));

        //Recipes needing sprites
        MixDict.Add("mohofruitsalt", new recipe("mohojam", 1));
        MixDict.Add("mohofruitmohojam", new recipe("mohofilling", 1));
        MixDict.Add("krocreammohofruit", new recipe("mohokrocream", 1));

    }

    private void LoadBakeDict()
    {
       
        //Implemented recipes
        BakeDict.Add("smolderdough", new recipe("cinderloaf", 1));
        BakeDict.Add("silkydough", new recipe("quaso", 1));

        //Recipes needing sprites
        
        BakeDict.Add("krocreamsilkydough", new recipe("krocreambun", 1));
        BakeDict.Add("mohojamsmolderdough", new recipe("mohotart", 1));
        BakeDict.Add("mohofillingsilkydough", new recipe("mohopie", 1));

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

