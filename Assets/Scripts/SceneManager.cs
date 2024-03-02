using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    //public TMP_Text response;
    public Scenario scenario;
    public GameObject scenarioObj;
    TMP_Text questionText;
    TMP_Text recipientText;
    int recipientindex = 0;
    int questionindex = 0;
    bool selectionMode;
    bool deviceChoice; //when player switches select mode
    public Sprite img;
    public Sprite[] spriteSheetSprites;

    // Start is called before the first frame update
    void Start()
    {
        scenarioObj.SetActive(false);
        selectionMode = true;
        deviceChoice = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            Application.Quit();
        }      
    }

    public void handleClick(string objName){
        if (selectionMode == true) {
            toggleSelection(objName); //also needs way for multiple objects - maybe pass id as var, also pass selection mode (yes or no)
        } else if (selectionMode == false) {
            loadScenario(); //work out way to handle multiple objects
        }
    }

    void toggleSelection(string objName){
        // SpriteRenderer sr = GameObject.Find("Alexa").GetComponent<SpriteRenderer>();
        // if (deviceChoice == true) {
        //     sr.sprite = spriteSheetSprites[0];
        // } else {
        //     Debug.Log("hi");
        //     sr.sprite = spriteSheetSprites[1];
        // }
        Image comp = GameObject.Find(objName).GetComponent<Image>();
        spriteSheetSprites = Resources.LoadAll<Sprite>(objName + "Sprites"); //prob change so only loads it once - maybe store in dictionary and check if in there
        comp.sprite = spriteSheetSprites[2];
        //comp.SetNativeSize();
    }

    void loadScenario(){
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
