using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    //public TMP_Text response;
    public Scenario scenario;
    public GameObject scenarioObj;
    TMP_Text questionText;
    TMP_Text recipientText;
    int recipientindex = 0;
    int questionindex = 0;
    // Start is called before the first frame update
    void Start()
    {
        scenarioObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadScenario(){
        // Vector3 position    = new Vector3(1, 1, 1);
        // Quaternion rotation = new Quaternion(1, 1, 1, 1);
        // GameObject obj      = Instantiate(prefab, position, rotation) as GameObject;

        scenarioObj.SetActive(true);
        questionindex = 0;
        recipientindex = 0;
        questionText = GameObject.Find("Question").GetComponent<TMP_Text>();
        if (scenario.messages[0].question != null) {
            questionText.SetText(scenario.messages[0].text + scenario.messages[0].question[questionindex].purpose + "?");
            recipientText = GameObject.Find("Recipient").GetComponent<TMP_Text>();
            recipientText.SetText(scenario.messages[0].question[questionindex].recipients[0]);
        } else {
            Debug.Log("Error: no question found"); //hide question bit and print dialogue text
            //if you can click through a selection I think that would be an impressive demo
            //response doesn't need to go anywhere - just that you can open dialogue (on little mockup phone - improve later), make selection, further selection, phone closes,
            //can continue game
        }
    }

    public void changeQuestion(){
        if (recipientindex < scenario.messages[0].question[questionindex].recipients.Length-1) {
            recipientindex += 1;
            recipientText = GameObject.Find("Recipient").GetComponent<TMP_Text>();
            recipientText.SetText(scenario.messages[0].question[questionindex].recipients[recipientindex]);
        } else if (questionindex < scenario.messages[0].question.Length-1){
            recipientindex = 0;
            questionindex += 1;
            questionText.SetText(scenario.messages[0].text + scenario.messages[0].question[questionindex].purpose + "?");
            recipientText.SetText(scenario.messages[0].question[questionindex].recipients[recipientindex]);
        } else {
            scenarioObj.SetActive(false);
        }

    }
}
