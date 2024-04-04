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
    Dictionary<string, Dialogue> dialogueDict = new Dictionary<string, Dialogue>();
    public PlaySceneManager manager;
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
        if (!dialogueDict.ContainsKey(name)) {
         dialogueDict.Add(name, Resources.Load<Dialogue>("Dialogue/" + name));
        }
        dialog = dialogueDict[name];
        current = cur;
        toggleTextbox();
        advanceText();
    }

    public void toggleTextbox() {
        dialogueBox.SetActive(!dialogueBox.activeInHierarchy);
        active = dialogueBox.activeInHierarchy; //for scenemanager to check
    }

    void advanceText() {
        int next;
        if (current > -1){ //dialogue remaining to show
            next = dialog.messages[current].next;
            textbox.text = (dialog.speaker + ": " + dialog.messages[current].text);
            current = next; 
        } else if (current == -2) {
            toggleTextbox();
            manager.showNote();
        } else if (current == -3) {
            toggleTextbox();
            manager.changeMode(false);
        } else {
            toggleTextbox();
        }
        
    }

    public void setText(string input) {
        textbox.text = input;
    }
}
