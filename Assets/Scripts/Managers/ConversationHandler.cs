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
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text dialogueName;
    [SerializeField] private GameObject jsonManager;
    private Dictionary<string, Discourse> conversations;
    private NPC character;
    private string first_Line;
    private string next_line_ID;
    private Discourse curr_discourse;
    private Quest quest;  //Unclear, may be able to just be the questID
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
        if(optionBoxPrefab.activeSelf){
            /*if(c.ans == 1){
                optionBoxPrefab.SetActive(false);
                DisplayNextSentence();
                string sentence = dialogueLines.Dequeue();
                string choice = dialogueOptions.Dequeue();            
            }
            else if(c.ans == 2){
                optionBoxPrefab.SetActive(false);
                string sentence = dialogueLines.Dequeue();
                string choice = dialogueOptions.Dequeue();  
                DisplayNextSentence(); 
            }
            else{
                Debug.Log("answer is " + c.ans);
            } */
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
        // Get the first line of dialogue
        // ISSUES: There will be multiple starting lines, need to find a way to determine which one to start with
        return "";
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
        }
    }
}
