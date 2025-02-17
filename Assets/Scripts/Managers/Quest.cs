public class Quest
{
    public string name;
    public string QuestID;
    public string quest_text;
    public string required_item_id;
    public int required_item_amount;
    public string recipe_reward_id;
    public string item_reward_id;
    public int item_reward_amount;

    //possible use for planet-wide rep
    public string planet_id;
    public int reputation_reward;

    public Quest(string name, string QuestID, string quest_text, string required_item_id, int required_item_amount, string recipe_reward_id = null, string item_reward_id = null, int item_reward_amount = 0, string planet_id = null, int reputation_reward = 0)
    {
        this.name = name;
        this.QuestID = QuestID;
        this.quest_text = quest_text;
        this.required_item_id = required_item_id;
        this.required_item_amount = required_item_amount;
        this.recipe_reward_id = recipe_reward_id;
        this.item_reward_id = item_reward_id;
        this.item_reward_amount = item_reward_amount;
        this.planet_id = planet_id;
        this.reputation_reward = reputation_reward;
    }
}
