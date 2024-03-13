using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    Dialogue dialog; //store dialogues in dict and scenarios
    public TMP_Text textbox;
    public bool active = false;
    int current = 0; //how to set this for first msg - pass starting point?
    Dictionary<string, Dialogue> dialogDict = new Dictionary<string, Dialogue>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && active) {
            advanceText();
        } 
    }

    public void startDialogue(int cur, string name) {
        if (!dialogDict.ContainsKey(name)) {
            dialogDict.Add(name, Resources.LoadAll<Dialogue>(name)[0]);
        }
        dialog = dialogDict[name];
        current = cur;
        toggleTextbox();
        advanceText();
    }

    void toggleTextbox() {
        dialogueBox.SetActive(!dialogueBox.activeInHierarchy);
        active = dialogueBox.activeInHierarchy; //for scenemanager to check
    }

    void advanceText() {
        int next;
        if (current > -1){ //dialogue remaining to show
            next = dialog.messages[current].next;
            textbox.text = (dialog.speaker + ": " + dialog.messages[current].text);
            current = next; 
        } else {
            toggleTextbox();
        }
        
    }
}
