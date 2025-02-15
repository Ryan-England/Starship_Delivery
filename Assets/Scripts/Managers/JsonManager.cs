using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;

public class JsonManager : MonoBehaviour
{
    //ENGLISH JSON FILES
    [SerializeField] private TextAsset engNPCJson;
    [SerializeField] private TextAsset engRecipeJson;
    [SerializeField] private TextAsset engQuestJson;
    [SerializeField] private TextAsset engItemJson;


    //LANGUAGE SETTINGS
    public enum Language
    {
        English,
        Simlish
    }
    public Language current_language;

    private Dictionary<string, NPC> npcList;
    private Dictionary<string, Recipe> recipeList;
    private Dictionary<string, Quest> questList;
    private Dictionary<string, Item> itemList;

    private JSONObject json_Object;

    void Start()
    {
        ChangeLanguage(Language.English);
    }

    public void ChangeLanguage(Language new_language)
    {
        current_language = new_language;
        PopulateDictionaries(current_language);
    }
    private void PopulateDictionaries(Language curr_Lang){
        npcList = new Dictionary<string, NPC>();
        recipeList = new Dictionary<string, Recipe>();
        questList = new Dictionary<string, Quest>();
        itemList = new Dictionary<string, Item>();
        switch (curr_Lang)
        {
            case Language.English:
                PopulateNPCs(engNPCJson.text);
                PopulateQuests(engQuestJson.text);
                PopulateRecipes(engRecipeJson.text);
                PopulateItems(engItemJson.text);
                break;
            case Language.Simlish:
                //populate the dictionaries with the objects from the json files
                //NPC
                //Recipe
                //Quest
                //Item
                break;
        }
    }

    
    private void PopulateNPCs(string json)
    {
        json_Object = new JSONObject(json);
        if (json_Object.type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list) {
            NPC newNPC = new NPC();
            newNPC.name = element["name"].stringValue;
            newNPC.UnitID = element["UnitID"].stringValue;
            if(element["QuestID"] != null){
                newNPC.QuestID = element["QuestID"].stringValue;
            }
            if(element["dialogues"] != null){
                newNPC.dialogues = new Dictionary<string, Dialogue>();
                foreach(JSONObject dialogue in element["dialogues"].list){
                    Dialogue newDialogue = new Dialogue();
                    newDialogue.name = newNPC.name;
                    //newDialogue.text = dialogue["text"].stringValue;
                    newNPC.dialogues.Add(dialogue["id"].stringValue, newDialogue);
                }
            }
            npcList.Add(newNPC.UnitID, newNPC);
        }
    }

    private void PopulateQuests(string json)
    {
        var json_Object = new JSONObject(json);
        if (json_Object.type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list) {
            Quest newQuest = new Quest();
            newQuest.name = element["name"].stringValue;
            newQuest.QuestID = element["QuestID"].stringValue;
            newQuest.quest_text = element["quest_text"].stringValue;
            newQuest.required_item_id = element["required_item_id"].stringValue;
            newQuest.required_item_amount = (int)element["required_item_amount"].intValue;
            if(element["recipe_reward_id"] != null){
                newQuest.recipe_reward_id = element["recipe_reward_id"].stringValue;
            }
            if(element["item_reward_id"] != null){
                newQuest.item_reward_id = element["item_reward_id"].stringValue;
            }
            if(element["item_reward_amount"] != null){
                newQuest.item_reward_amount = (int)element["item_reward_amount"].intValue;
            }
            if(element["planet_id"] != null){
                newQuest.planet_id = element["planet_id"].stringValue;
            }
            if(element["reputation_reward"] != null){
                newQuest.reputation_reward = (int)element["reputation_reward"].intValue;
            }
            questList.Add(newQuest.QuestID, newQuest);
        }
    }

    private void PopulateRecipes(string json)
    {
        var json_Object = new JSONObject(json);
        if (json_Object.type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list) {
            Recipe newRecipe = new Recipe();
            newRecipe.name = element["name"].stringValue;
            newRecipe.RecipeID = element["RecipeID"].stringValue;
            newRecipe.ingredient1_id = element["ingredient1_id"].stringValue;
            newRecipe.ingredient1_amount = (int)element["ingredient1_amount"].intValue;
            if (element["ingredient2_id"] != null)
            {
                newRecipe.ingredient2_id = element["ingredient2_id"].stringValue;
                newRecipe.ingredient2_amount = (int)element["ingredient2_amount"].intValue;
            }
            if (element["ingredient3_id"] != null)
            {
                newRecipe.ingredient3_id = element["ingredient3_id"].stringValue;
                newRecipe.ingredient3_amount = (int)element["ingredient3_amount"].intValue;
            }
            if (element["ingredient4_id"] != null)
            {
                newRecipe.ingredient4_id = element["ingredient4_id"].stringValue;
                newRecipe.ingredient4_amount = (int)element["ingredient4_amount"].intValue;
            }
            newRecipe.minigame1_id = element["minigame1_id"].stringValue;
            if (element["minigame2_id"] != null)
            {
                newRecipe.minigame2_id = element["minigame2_id"].stringValue;
            }
            if (element["minigame3_id"] != null)
            {
                newRecipe.minigame3_id = element["minigame3_id"].stringValue;
            }
            newRecipe.output_id = element["output_id"].stringValue;
            newRecipe.output_amount = (int)element["output_amount"].intValue;
            recipeList.Add(newRecipe.RecipeID, newRecipe);
        }
    }

    private void PopulateItems(string json)
    {
        var json_Object = new JSONObject(json);
        if (json_Object.type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list) {
            Item newItem = new Item();
            newItem.name = element["name"].stringValue;
            newItem.ItemID = element["ItemID"].stringValue;
            newItem.description = element["description"].stringValue;
            newItem.value = element["value"].intValue;
            if(element["tags"] != null){
                foreach(JSONObject tag in element["tags"].list){
                    newItem.tags.Add(tag.stringValue);
                }
            }
            itemList.Add(newItem.ItemID, newItem);
        }
    }

    public Quest getQuest(string id){
        if (questList.ContainsKey(id))
        {
            return questList[id];
        }
        else
        {
            Debug.LogError("Quest with id " + id + " not found");
            return null;
        }
    }
        public Item getItem(string id){
        if (itemList.ContainsKey(id))
        {
            return itemList[id];
        }
        else
        {
            Debug.LogError("Item with id " + id + " not found");
            return null;
        }
    }
    public Recipe getRecipe(string id){
        if (recipeList.ContainsKey(id))
        {
            return recipeList[id];
        }
        else
        {
            Debug.LogError("Recipe with id " + id + " not found");
            return null;
        }
    }
    public NPC getNPC(string id){
        if (npcList.ContainsKey(id))
        {
            return npcList[id];
        }
        else
        {
            Debug.LogError("NPC with id " + id + " not found");
            return null;
        }
    }
}
