using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CookbookUI : MonoBehaviour
{
    [SerializeField] private Image outputImage;
    [SerializeField] private TMP_Text outputAmountText;
    [SerializeField] private Transform ingredientContainer;

    [SerializeField] private JsonManager jsonManager;
    private List<GameObject> ingredientSlots = new List<GameObject>();
    private TMP_Text cookingStationText;

    void Start()
    {
        jsonManager = FindObjectOfType<JsonManager>();
        ShowRecipe("generic");
    }

    public void ShowRecipe(string recipeID)
    {
        Recipe recipe = jsonManager.getRecipe(recipeID);
        if (recipe == null) return;

        // Load Output Image
        // Item outputItem = jsonManager.getItem(recipe.output_id);
        // outputImage.sprite = Resources.Load<Sprite>(outputItem.image_filepath);
        // outputAmountText.text = "x" + recipe.output_amount;

        // // Clear Previous UI Elements
        // cookingStationText.text = "";
        // ingredientSlots.Clear();

        // Load Ingredients
        // AddIngredient(recipe.ingredient1_id, recipe.ingredient1_amount, 0);
        // if (!string.IsNullOrEmpty(recipe.ingredient2_id)) AddIngredient(recipe.ingredient2_id, recipe.ingredient2_amount, 1);
        // if (!string.IsNullOrEmpty(recipe.ingredient3_id)) AddIngredient(recipe.ingredient3_id, recipe.ingredient3_amount, 2);
        // if (!string.IsNullOrEmpty(recipe.ingredient4_id)) AddIngredient(recipe.ingredient4_id, recipe.ingredient4_amount, 3);

        // Load Minigames
        AddMinigame(recipe.minigame1_id);
        if (!string.IsNullOrEmpty(recipe.minigame2_id)) AddMinigame(recipe.minigame2_id);
        if (!string.IsNullOrEmpty(recipe.minigame3_id)) AddMinigame(recipe.minigame3_id);
    }

    private void AddIngredient(string ingredientID, int amount, int index)
    {
        Item ingredient = jsonManager.getItem(ingredientID);

        // Create a new UI GameObject for the ingredient
        GameObject newIngredient = new GameObject(ingredientID);
        newIngredient.transform.SetParent(ingredientContainer, false);

        // Add Image component for the ingredient icon
        Image iconImage = newIngredient.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>(ingredient.image_filepath);
        
        // Item ingredient = jsonManager.getItem(ingredientID);
        
        // // Ensure the correct UI slot exists
        // while (ingredientSlots.Count <= index)
        // {
        //     ingredientSlots.Add(Instantiate(ingredientPrefab, ingredientContainer));
        // }

        // GameObject ingredientGO = ingredientSlots[index];
        // ingredientGO.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(ingredient.image_filepath);
        // ingredientGO.transform.Find("Amount").GetComponent<Text>().text = "x" + amount;
    }

    private void AddMinigame(string minigameID)
    {
        // Find the specific GameObject named "CookingStationText"
        GameObject cookingStationTextGO = GameObject.Find("CookingStationText");
    
        if (cookingStationTextGO != null)
        {
            cookingStationText = cookingStationTextGO.GetComponent<TMP_Text>();

            if (cookingStationText != null)
            {
                // Append new minigame, adding a comma if there’s already text
                if (!string.IsNullOrEmpty(cookingStationText.text))
                {
                    cookingStationText.text += ", " + minigameID;
                }
                else
                {
                    cookingStationText.text = minigameID;
                }
            }
            else
            {
                Debug.LogWarning("TMP_Text not found on CookingStationText GameObject.");
            }
        } 
        else
            {
                Debug.LogWarning("TMP_Text GameObject not found in the scene.");
            }
        }
}