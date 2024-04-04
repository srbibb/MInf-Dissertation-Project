using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour
{
    public DialogueManager dialogueMan;
    public TMP_Text questionText;
    int questionindex = 0;
    Dialogue currentDialogue;
    string currDialogue;
    int[] normsChoices = new int[15];
    int[] westinChoices = new int[3];
    bool doingQs = false;
    // Start is called before the first frame update
    void Start()
    {
        questionText.text = "Welcome to our Privacy Norms Quiz!";
        dialogueMan.startDialogue(0, "OpeningDialogue");
        currentDialogue = Resources.LoadAll<Dialogue>("Dialogue/NormsQuestions")[0];
        currDialogue = "norms";
    }

    // Update is called once per frame
    //TODO: can't click questions qhen textbox up (same for playmanager)
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            Application.Quit();
        } 
        if (dialogueMan.active == false && doingQs == false) {
            questionText.text = (currentDialogue.messages[0].text);
            doingQs = true;
        }
        if(Input.GetMouseButtonDown(0) && currDialogue == "finished") {
            SceneManager.LoadScene("LivingRoom");
        }
    }

    public void advanceText(int choice) {
        if (dialogueMan.active) {
            return;
        }
        if (currDialogue == "norms") {
            normsChoices[questionindex] = choice;
        } else if (currDialogue == "westin") {
            westinChoices[questionindex] = choice;
        }
        questionindex = currentDialogue.messages[questionindex].next;
        if (questionindex > -1){ //dialogue remaining to show
            questionText.text = (currentDialogue.messages[questionindex].text);
        } else if (questionindex == -2) {
            currDialogue = "westin";
            questionindex = 0;
            GameObject.Find("IDK").SetActive(false); //-87 and -269
            GameObject.Find("Agr").GetComponent<Transform>().Translate(0, -0.4f, 0);
            GameObject.Find("Disagr").GetComponent<Transform>().Translate(0, 0.4f, 0);
            currentDialogue = Resources.LoadAll<Dialogue>("Dialogue/WestinQuestions")[0];
            questionText.text = (currentDialogue.messages[questionindex].text);
        } else {
            dialogueMan.startDialogue(3, "OpeningDialogue");
            currDialogue = "finished";
            PointCalc.setNorms(normsChoices);
            PointCalc.setWestin(westinChoices);
        }
    }
}
