using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class InteractionHandler : MonoBehaviour
{
    // Enum of three states for this object, it'll either be a NPC, Object, or an Objective.
    public enum SearchType { NPC, Object, Objectives }
    // Eventually, we'll use searchType for unique setups for dialogue or dialogue boxes/choices.
    [SerializeField] private SearchType searchType;
    // Decide whether the interaction will be dialogue or not
    [SerializeField] private bool hasDialogue = false;
    // List of string lines shown in the inspector
    [SerializeField] private Queue<string> dialogueLines = new Queue<string>();
    [SerializeField] private Queue<string> dialogueOptions = new Queue<string>();

    // Prefab of chatbox that'll appear when dialogue is ready.
    [SerializeField] private GameObject chatBoxPrefab;
    [SerializeField] private GameObject optionBoxPrefab;

    [SerializeField] private Dialogue dialogue;
    
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text dialogueName;

    [SerializeField] private Text opt1;
    [SerializeField] private Text opt2;

    [SerializeField] private Choice c;

    public NPCQuest quest;
    public GameObject quest_anim;
    public Text quest_name;
    public QuestManager qm;
    public Inventory inv; 

    public bool quest_complete_first = false;
    public bool quest_complete = false;


    public string[] sentences;
    private bool skip = false;
    // Reference that tracks the current line of dialogue
    private int currentLineIndex = 0;

    public AudioSource source;
    public AudioClip clip;

    // Called by the detection manager to interact with the object on E press.
    public void Interact()
    {
        // If dialogue is allowed, the chatbox exists, and the total dialogue lines are more than zero, do a dialogue interaction
        // if (hasDialogue && dialogueLines.Count > 0)
        // {
            // Find the TMP_Text component as a child of the ChatBox
            // TMP_Text chatText = newChatBox.GetComponentInChildren<TMP_Text>();
            // if (chatText != null)
            // {
            //     // Debug.Log($"Displayed Dialogue: {dialogueLines[currentLineIndex]}");
            //     // chatText.text = dialogueLines[currentLineIndex];
            //     // currentLineIndex = (currentLineIndex + 1) % dialogueLines.Count;
            //     // //DisplayDialogue(dialogueLines[currentLineIndex]);
            // }
            // else
            // {
            //     Debug.LogError("No TMP_Text found as a child of the ChatBox!");
            // }
        // }
        // else
        // {
        //     Debug.LogWarning("Interaction available, but no dialogue is set.");
        // }
        if(chatBoxPrefab.activeSelf){
            if(!optionBoxPrefab.activeSelf){
                DisplayNextSentence();
            }
        }
        else{
            chatBoxPrefab.SetActive(true);
            sentences = dialogue.sentences;
            CheckQuest();
            StartDialogue(dialogue);
        }
    }
    void Update(){
        if(optionBoxPrefab.activeSelf){
            if(c.ans == 1){
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
            } 
        }
  
    }

    public void CheckQuest(){
        if(quest.have_quest){
            if(qm.Quests.ContainsKey(quest.quest_name)){
                if(qm.Quests[quest.quest_name]){
                    sentences = dialogue.quest_complete;
                    return;
                }
                switch(quest.quest_type){
                    case 0: 
                    case 1:
                        if(inv.FindItem(quest.item_name)){
                            inv.RemoveItem(quest.item_name, Inventory.ItemType.Ingredient, 1);
                            sentences = dialogue.quest_check;
                            qm.CompleteQuest(quest);
                            quest_complete_first = true;
                            return;
                        }
                        break;
                    default: 
                        break;
                }
            }
        }
    }
    // public void NextLine() {
    //     // If the total dialogue lines are more than zero
    //     if (dialogueLines.Count > 0) {
    //         currentLineIndex = (currentLineIndex + 1) % dialogueLines.Count;
    //         Debug.Log($"Moving to line {currentLineIndex}: {dialogueLines[currentLineIndex]}");
    //     }
    // }

    // public void SetLine(int index) {
    //     // If index is more than or equal to 0 and isn't more than the total lines.
    //     if (index >=0 && index < dialogueLines.Count) {
    //         currentLineIndex = index;
    //         Debug.Log($"Setting dialogue to the line {currentLineIndex}: {dialogueLines[currentLineIndex]}");
    //     }
    // }
    public void StartDialogue(Dialogue dialogue){
        Debug.Log("Starting conversation with " + dialogue.name);
        optionBoxPrefab.SetActive(false);
        dialogueName.text = dialogue.name;
        int i = 0;
        foreach (string sentence in sentences){
            Debug.Log(sentence);
            dialogueLines.Enqueue(sentence);
        }
        foreach(string choice in dialogue.choices){
            dialogueOptions.Enqueue(choice);
        }
        source.PlayOneShot(clip);
        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        int j = 0;
        foreach (string s in dialogueLines){
            Debug.Log(s);
        }
        Debug.Log("pressed");
        Debug.Log(dialogueLines.Count);
        if(dialogueLines.Count == 0){
            EndDialogue();
            return;
        }

        string sentence = dialogueLines.Dequeue();
        string choice = dialogueOptions.Dequeue();
        if(choice != ""){
            optionBoxPrefab.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            string[] opts = choice.Split(',');
            opt1.text = opts[0]; 
            opt2.text = opts[1];
        }
        else{
            optionBoxPrefab.SetActive(false);
        }
        source.PlayOneShot(clip);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach(char i in sentence.ToCharArray()){
            dialogueText.text += i; 
            yield return null;
        }
    }
    void EndDialogue(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        dialogueLines.Clear();
        dialogueOptions.Clear();
        dialogueName.text ="";
        dialogueText.text = "";
        opt1.text = "";
        opt2.text = "";
        // movement.enabled = true;
        // GetComponent<Camera>().m_XAxis.m_MaxSpeed = currentX;
        // GetComponent<Camera>().m_YAxis.m_MaxSpeed = currentY;
        // box.SetActive(false);
        chatBoxPrefab.SetActive(false);
        optionBoxPrefab.SetActive(false);
        c.ans = 0; 

        if(quest.have_quest){
            if(!qm.Quests.ContainsKey(quest.quest_name)){
                quest_name.text = quest.quest_name;
                quest_anim.SetActive(true);
                qm.AddQuest(quest);
            }
            else if(qm.Quests[quest.quest_name] && quest_complete_first){
                quest_name.text = "Quest Complete: " + quest.quest_name;
                quest_anim.SetActive(true);
                quest_complete_first = false;
            }
        }
        Debug.Log("End of Conversation");
    }
}
