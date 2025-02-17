public class Quest
{
    public string name;
    public string QuestID;
    public string quest_text;
    public string required_item_id;
    public int required_item_amount;
    public string recipe_reward_id = null;
    public string item_reward_id = null;
    public int item_reward_amount = 0;

    //possible use for planet-wide rep
    public string planet_id = null;
    public int reputation_reward = 0;
}
