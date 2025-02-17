using System.Collections;
using System.Collections.Generic;

public class NPC
{
    public string name;
    public string UnitID;
    public string QuestID;
    public Dictionary<string, Dialogue> dialogues;

    public NPC(string name, string UnitID, Dictionary<string, Dialogue> dialogues, string QuestID = null)
    {
        this.name = name;
        this.UnitID = UnitID;
        this.QuestID = QuestID;
        this.dialogues = dialogues;
    }
}
