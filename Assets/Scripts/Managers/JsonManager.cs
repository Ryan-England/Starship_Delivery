using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;

public class JsonManager : MonoBehaviour
{
    //ENGLISH JSON FILES
    [Tooltip("The JSON file containing the NPC data in English")]
    [SerializeField] private TextAsset engNPCJson;
    [Tooltip("The JSON file containing the Recipe data in English")]
    [SerializeField] private TextAsset engRecipeJson;
    [Tooltip("The JSON file containing the Quest data in English")]
    [SerializeField] private TextAsset engQuestJson;
    [Tooltip("The JSON file containing the Item data in English")]
    [SerializeField] private TextAsset engItemJson;


    //LANGUAGE SETTINGS
    public enum Language
    {
        English,
        Simlish
    }
    [Tooltip("The current language of the game")]
    public Language current_language;

    private Dictionary<string, NPC> npcList;
    private Dictionary<string, Recipe> recipeList;
    private Dictionary<string, Quest> questList;
    private Dictionary<string, Item> itemList;

    private JSONObject json_Object;

    void Awake()
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
                //PopulateItems(engItemJson.text);
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
        if (json_Object.list[0].type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list[0]) {
            var dialogues = new Dictionary<string, Discourse>();
            var newNPC = new NPC(element["name"].stringValue, element["UnitID"].stringValue, dialogues);
            if (element["QuestID"] != null)
            {
                newNPC.QuestID = element["QuestID"].stringValue;
            }
            if(element["dialogues"] != null){
                foreach(JSONObject dialogue in element["dialogues"].list){
                    Discourse newDialogue = new Discourse(dialogue["speaker"].stringValue, dialogue["line_ID"].stringValue, dialogue["text"].stringValue, 
                        dialogue["flag_ID"].intValue, dialogue["prev_line_ID"].stringValue, dialogue["next_line_ID"].stringValue, dialogue["quest_ID"].stringValue, 
                        dialogue["choice_A"].stringValue, dialogue["choice_A_ID"].stringValue, dialogue["choice_B"].stringValue, dialogue["choice_B_ID"].stringValue);
                    newNPC.dialogues.Add(dialogue["line_ID"].stringValue, newDialogue);
                }
            }
            npcList.Add(newNPC.UnitID, newNPC);
        }
    }

    private void PopulateQuests(string json)
    {
        var json_Object = new JSONObject(json);
        if (json_Object.list[0].type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list[0]) {
            Quest newQuest = new Quest(element["name"].stringValue, element["QuestID"].stringValue, element["quest_text"].stringValue, element["required_item_id"].stringValue, (int)element["required_item_amount"].intValue);
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
        if (json_Object.list[0].type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list[0]) {
            Recipe newRecipe = new Recipe(element["name"].stringValue, element["RecipeID"].stringValue, element["ingredient1_id"].stringValue, (int)element["ingredient1_amount"].intValue, element["minigame1_id"].stringValue, element["output_id"].stringValue, (int)element["output_amount"].intValue);
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
            if (element["minigame2_id"] != null)
            {
                newRecipe.minigame2_id = element["minigame2_id"].stringValue;
            }
            if (element["minigame3_id"] != null)
            {
                newRecipe.minigame3_id = element["minigame3_id"].stringValue;
            }
            recipeList.Add(newRecipe.RecipeID, newRecipe);
        }
    }

    private void PopulateItems(string json)
    {
        var json_Object = new JSONObject(json);
        if (json_Object.list[0].type != JSONObject.Type.Array)
        {
            Debug.LogError("Json file is not an array");
            return;
        }
        foreach (JSONObject element in json_Object.list[0]) {
            Item newItem = new Item();
            newItem.name = element["name"].stringValue;
            newItem.ItemID = element["ItemID"].stringValue;
            newItem.image_filepath = element["image_filepath"].stringValue;
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
            return npcList["generic"];
        }
    }
}
