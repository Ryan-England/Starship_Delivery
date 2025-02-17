public class Recipe
{
    public string name;
    public string RecipeID;
    public string ingredient1_id;
    public int ingredient1_amount;
    public string ingredient2_id;
    public int ingredient2_amount;
    public string ingredient3_id;
    public int ingredient3_amount;
    public string ingredient4_id;
    public int ingredient4_amount;
    public string minigame1_id;
    public string minigame2_id;
    public string minigame3_id;
    public string output_id;
    public int output_amount;

    public Recipe(string name, string RecipeID, string ingredient1_id, int ingredient1_amount, string minigame1_id, string output_id, int output_amount, string ingredient2_id = null, int ingredient2_amount = 0, string ingredient3_id = null, int ingredient3_amount = 0, string ingredient4_id = null, int ingredient4_amount = 0, string minigame2_id = null, string minigame3_id = null)
    {
        this.name = name;
        this.RecipeID = RecipeID;
        this.ingredient1_id = ingredient1_id;
        this.ingredient1_amount = ingredient1_amount;
        this.ingredient2_id = ingredient2_id;
        this.ingredient2_amount = ingredient2_amount;
        this.ingredient3_id = ingredient3_id;
        this.ingredient3_amount = ingredient3_amount;
        this.ingredient4_id = ingredient4_id;
        this.ingredient4_amount = ingredient4_amount;
        this.minigame1_id = minigame1_id;
        this.minigame2_id = minigame2_id;
        this.minigame3_id = minigame3_id;
        this.output_id = output_id;
        this.output_amount = output_amount;
    }
}
