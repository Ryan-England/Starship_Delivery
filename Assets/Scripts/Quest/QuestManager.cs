using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Inventory inv;
    public Dictionary<string, bool> Quests = new Dictionary<string, bool>();
    public void AddQuest(NPCQuest n){
        Quests.Add(n.quest_name, false);
    }
    public void CompleteQuest(NPCQuest n){
        Quests[n.quest_name] = true;
    }
}
