using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationHandler : MonoBehaviour
{
    [Tooltip("The ID of this NPC in the JSON file")]
    [SerializeField] private string unitID;

    [Tooltip("The prefab of the chatbox that will display the dialogue")]
    [SerializeField] private GameObject chatBoxPrefab;
    [Tooltip("The prefab of the option box that will display the dialogue choices")]
    [SerializeField] private GameObject optionBoxPrefab;

    [Tooltip("The text component of the chatbox that will display the dialogue")]
    [SerializeField] private Text dialogueText;
    [Tooltip("The text component of the chatbox that will display the name of the NPC")]
    [SerializeField] private Text dialogueName;

    [Tooltip("The object with the JSONManager script attached")]
    [SerializeField] private GameObject jsonManager;

    [Tooltip("The text component of the option box that will display the first dialogue choice")]
    [SerializeField] private Text opt1;
    [Tooltip("The text component of the option box that will display the second dialogue choice")]
    [SerializeField] private Text opt2;

    [Tooltip("The object with the Choice script attached")]
    [SerializeField] private Choice c;
    
    private Dictionary<string, Discourse> conversations;
    private NPC character;
    private string first_Line;
    private string next_line_ID;
    private Discourse curr_discourse;
    private int curr_flag = 0;
    private Quest Nquest;  //Unclear, may be able to just be the questID

    // TEMPORARY WHILE QUEST JSON IS NOT IMPLEMENTED
    public NPCQuest quest;
    public GameObject quest_anim;
    public Text quest_name;

    // Start is called before the first frame update
    void Start()
    {
        // Get the conversation data from the JSON Manager script
        if (jsonManager != null)
        {
            loadNPC(unitID);
            conversations = character.dialogues;
        }
        else
        {
            Debug.LogError("JsonManager not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(optionBoxPrefab.activeSelf){
            if(c.ans == 1){
                optionBoxPrefab.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                c.ans = 0;
                DisplayNextLine(curr_discourse.choice_A_ID);
            }
            if(c.ans == 2){
                optionBoxPrefab.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                c.ans = 0;
                DisplayNextLine(curr_discourse.choice_B_ID);
            }
        }
    }

    public void Interact()
    {
        if(chatBoxPrefab.activeSelf){ //if already in conversation
            if(!optionBoxPrefab.activeSelf){
                if (curr_discourse.next_line_ID != null)
                {
                    next_line_ID = curr_discourse.next_line_ID;
                    DisplayNextLine(next_line_ID);
                }
                else if (curr_discourse.choice_A != null)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    opt1.text = curr_discourse.choice_A;
                    opt2.text = curr_discourse.choice_B;
                    optionBoxPrefab.SetActive(true);
                }
                else
                {
                    /* if (curr_discourse.quest_ID != null)
                    {
                        quest = jsonManager.GetComponent<JsonManager>().getQuest(curr_discourse.quest_ID);
                        //Supposed to set quest to active, once JSON quest system is functional
                    }*/
                    chatBoxPrefab.SetActive(false);
                    if(quest.have_quest){
                        quest_name.text = quest.quest_name;
                        quest_anim.SetActive(true);
                    }
                    curr_flag += 1;
                }
            }
        }
        else{ //starting conversation
            chatBoxPrefab.SetActive(true);
            first_Line = getDiscourseStart();
            DisplayNextLine(first_Line);
        }
    }

    private string getDiscourseStart()
    {
        foreach (KeyValuePair<string, Discourse> entry in conversations)
        {
            if (entry.Value.flag_ID == curr_flag && entry.Value.prev_line_ID == null)
            {
                return entry.Key;
            }
        }
        return null;
    }

    private void DisplayNextLine(string line_ID)
    {
        // Get the next line of dialogue
        curr_discourse = conversations[line_ID];
        StartCoroutine(TypeSentence(curr_discourse.text));
    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach(char i in sentence.ToCharArray()){
            dialogueText.text += i; 
            yield return null;
        } //maybe include something to reduce font size if text is too long
        // futher issue - skipping text on repeated button presses
        // focus on matching current implementation first, improvements later
    }

    public void loadNPC(string unitID){
        character = jsonManager.GetComponent<JsonManager>().getNPC(unitID);
    }
}

