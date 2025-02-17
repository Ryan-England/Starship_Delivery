using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationHandler : MonoBehaviour
{
    [SerializeField] private string unitID;
    [SerializeField] private GameObject chatBoxPrefab;
    [SerializeField] private GameObject optionBoxPrefab;
    [SerializeField] private GameObject jsonManager;
    private Dictionary<string, Discourse> conversations;
    private NPC character;
    private string first_Line;
    private Quest quest;
    private Text opt1;
    private Text opt2;
    // Start is called before the first frame update
    void Start()
    {
        // Get the conversation data from the JSON Manager script
        if (jsonManager != null)
        {
            character = jsonManager.GetComponent<JsonManager>().getNPC(unitID);
            //conversations = character.dialogues;
        }
        else
        {
            Debug.LogError("JsonManager not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(chatBoxPrefab.activeSelf){
            if(!optionBoxPrefab.activeSelf){
                DisplayNextLine(first_Line);
            }
        }
        else{
            chatBoxPrefab.SetActive(true);
            first_Line = getDiscourseStart();
            DisplayNextLine(first_Line);
        }
    }

    private string getDiscourseStart()
    {
        // Get the first line of dialogue
        return "";
    }

    private void DisplayNextLine(string line_ID)
    {
        // Display the first line of dialogue
    }
}
