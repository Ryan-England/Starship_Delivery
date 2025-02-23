using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Inventory inv;
    public Dictionary<string, bool> Quests = new Dictionary<string, bool>();
    private CompassUI compassUI;
    private void Start()
    {
        // Try to find the CompassUI in the scene
        compassUI = FindObjectOfType<CompassUI>();

        if (compassUI == null)
        {
            Debug.LogError("CompassUI not found in the scene. Please add the CompassUI component in the scene.");
        }
    }
    public void AddQuest(NPCQuest n){
        Quests.Add(n.quest_name, false);

        if (n.questMarker != null && compassUI != null)
        {
            if (n.itemMarker != null) {
                compassUI.ShowQuestMarker(n.itemMarker);  // Show the quest marker on the compass
            }
            compassUI.ShowQuestMarker(n.questMarker);  // Show the quest marker on the compass
        } else {
            Debug.LogError("QuestMarker is missing!");
        }
    }
    public void CompleteQuest(NPCQuest n){
        compassUI.HideQuestMarker(n.itemMarker);
        compassUI.HideQuestMarker(n.questMarker);
        Quests[n.quest_name] = true;
    }
}
