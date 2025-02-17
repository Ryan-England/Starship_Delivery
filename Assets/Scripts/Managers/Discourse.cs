public class Discourse
{
    public string speaker;
    public string line_ID;
    public string next_line_ID;
    public string text;
    public string quest_ID;
    public string choice_A;
    public string choice_A_ID;
    public string choice_B;
    public string choice_B_ID;

    public Discourse(string speaker, string line_ID, string text, string next_line_ID = null, string quest_ID = null, string choice_A = null, string choice_A_ID = null, string choice_B = null, string choice_B_ID = null)
    {
        this.speaker = speaker;
        this.line_ID = line_ID;
        this.next_line_ID = next_line_ID;
        this.text = text;
        this.quest_ID = quest_ID;
        this.choice_A = choice_A;
        this.choice_A_ID = choice_A_ID;
        this.choice_B = choice_B;
        this.choice_B_ID = choice_B_ID;
    }

}
