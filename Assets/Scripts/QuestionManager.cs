using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    //public TMP_Text response;
    public Scenario scenario;
    public GameObject prefab;
    TMP_Text questionText;
    TMP_Text recipientText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadScenario(int index){
        // Vector3 position    = new Vector3(1, 1, 1);
        // Quaternion rotation = new Quaternion(1, 1, 1, 1);
        // GameObject obj      = Instantiate(prefab, position, rotation) as GameObject;
        
        if (scenario.messages[0].question != null) {
            questionText = GameObject.Find("Question").GetComponent<TMP_Text>();
            questionText.SetText(scenario.messages[0].text);
            recipientText = GameObject.Find("Recipient").GetComponent<TMP_Text>();
            recipientText.SetText(scenario.messages[0].question.recipients[0]);
        } else {
            Debug.Log("hm"); //hide question bit and print dialogue text
            //if you can click through a selection I think that would be an impressive demo
            //response doesn't need to go anywhere - just that you can open dialogue (on little mockup phone - improve later), make selection, further selection, phone closes,
            //can continue game
        }
    }
}
