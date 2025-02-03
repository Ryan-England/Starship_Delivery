// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Cinemachine;

// public class DialogueManager : MonoBehaviour
// {
//     private Queue<string> sentences;
//     public Text dialogueText;
//     public Text dialogueName;
//     public GameObject box;
//     public Movement2 movement;
//     public CinemachineFreeLook camera;    

//     public float currentX;
//     public float currentY;
//     // Start is called before the first frame update
//     void Start()
//     {
//         sentences = new Queue<string>();
//     }
//     public void Update(){
//         if(camera.m_XAxis.m_MaxSpeed != 0.0f && camera.m_YAxis.m_MaxSpeed != 0.0f){
//             currentX = camera.m_XAxis.m_MaxSpeed;
//             currentY = camera.m_YAxis.m_MaxSpeed;
//         }
//         // if(Input.GetKeyDown("e") && box.activeSelf){
//         //     DisplayNextSentence();
//         // }

//     }
//     public void StartDialogue(Dialogue dialogue){
//         Debug.Log("Starting conversation with " + dialogue.name);
//         dialogueName.text = dialogue.name;

//         sentences.Clear();

//         foreach (string sentence in dialogue.sentences){
//             Debug.Log(sentence);
//             sentences.Enqueue(sentence);
//         }
            
//         DisplayNextSentence();
//     }

//     public void DisplayNextSentence(){
//         foreach (string s in sentences){
//             Debug.Log(s);
//         }
//         Debug.Log("pressed");
//         Debug.Log(sentences.Count);
//         if(sentences.Count == 0){
//             EndDialogue();
//             return;
//         }

//         string sentence = sentences.Dequeue();
//         StopAllCoroutines();
//         StartCoroutine(TypeSentence(sentence));
//     }

//     IEnumerator TypeSentence(string sentence){
//         dialogueText.text = "";
//         foreach(char i in sentence.ToCharArray()){
//             dialogueText.text += i; 
//             yield return null;
//         }
//     }
//     void EndDialogue(){
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         movement.enabled = true;
//         camera.m_XAxis.m_MaxSpeed = currentX;
//         camera.m_YAxis.m_MaxSpeed = currentY;
//         box.SetActive(false);
//         Debug.Log("End of Conversation");
//     }

// }
