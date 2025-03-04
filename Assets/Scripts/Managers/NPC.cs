using System.Collections;
using System.Collections.Generic;

public class NPC
{
    public string name;
    public string UnitID;
    public Dictionary<string, Discourse> dialogues;

    public NPC(string name, string UnitID, Dictionary<string, Discourse> dialogues)
    {
        this.name = name;
        this.UnitID = UnitID;
        this.dialogues = dialogues;
    }
}
