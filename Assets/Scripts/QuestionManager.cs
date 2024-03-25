using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour
{
    public DialogueManager dialogMan;
    public TMP_Text questionText;
    int questionindex = 0;
    Dialogue currentDialog;
    string currDialog;
    int[] normsChoices = new int[15];
    int[] westinChoices = new int[3];
    bool doingQs = false;
    // Start is called before the first frame update
    void Start()
    {
        questionText.text = "Welcome to our Privacy Norms Quiz!";
        dialogMan.startDialogue(0, "OpeningDialogue");
        currentDialog = Resources.LoadAll<Dialogue>("Dialogue/NormsQuestions")[0];
        currDialog = "norms";
    }

    // Update is called once per frame
    //TODO: can't click questions qhen textbox up (same for playmanager)
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            Application.Quit();
        } 
        if (dialogMan.active == false && doingQs == false) {
            questionText.text = (currentDialog.messages[0].text);
            doingQs = true;
        }
        if(Input.GetMouseButtonDown(0) && currDialog == "finished") {
            SceneManager.LoadScene("LivingRoom");
        }
    }

    public void advanceText(int choice) {
        if (dialogMan.active) {
            return;
        }
        if (currDialog == "norms") {
            normsChoices[questionindex] = choice;
        } else if (currDialog == "westin") {
            westinChoices[questionindex] = choice;
        }
        questionindex = currentDialog.messages[questionindex].next;
        if (questionindex > -1){ //dialogue remaining to show
            questionText.text = (currentDialog.messages[questionindex].text);
        } else if (questionindex == -2) {
            currDialog = "westin";
            questionindex = 0;
            GameObject.Find("IDK").SetActive(false); //-87 and -269
            GameObject.Find("Agr").GetComponent<Transform>().Translate(0, -0.4f, 0);
            GameObject.Find("Disagr").GetComponent<Transform>().Translate(0, 0.4f, 0);
            currentDialog = Resources.LoadAll<Dialogue>("Dialogue/WestinQuestions")[0];
            questionText.text = (currentDialog.messages[questionindex].text);
        } else {
            dialogMan.startDialogue(3, "OpeningDialogue");
            currDialog = "finished";
            PointCalc.setNorms(normsChoices);
            PointCalc.setWestin(westinChoices);
        }
    }
}
