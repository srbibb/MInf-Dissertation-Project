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
    //int recipientindex = 0;
    //int questionindex = 0;
    int selectionMode = 1; // which decision player is currently making - is iot or is not iot - 1 is yes 2 is no
    bool deviceChoiceMode; // when player switches select mode
    //need something that checks player has done all objects when they click to continue and stops if they haven't
    Dictionary<string, GameObject> objDict = new Dictionary<string, GameObject>();
    Dictionary<string, Sprite[]> spritesDict = new Dictionary<string, Sprite[]>();
    Dictionary<string, int> choiceDict = new Dictionary<string,int>(); //not sure if this is the best way? how to store correct answers? just do it in array and have list of objects and answers? or save their choices in array
    // in order of dictionary with correct answers might be better than both in dict or both in array bc then can check dict actually
    public DialogueManager dialogMan;

    // Start is called before the first frame update
    void Start()
    {
        scenarioObj.SetActive(false);
        deviceChoiceMode = true;
        objDict.Add("SofaL", GameObject.Find("SofaL"));
        objDict.Add("SofaR", GameObject.Find("SofaR"));
        objDict["SofaL"].GetComponent<Button>().enabled = false; // so they don't interfere with headphones, turn back on when doing scenarios to add flavour text
        objDict["SofaR"].GetComponent<Button>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            Application.Quit();
        } 
    }

    public void handleClick(string objName){
        if (deviceChoiceMode == true) {
            toggleSelection(objName); 
        } else if (deviceChoiceMode == false) {
            loadScenario(); //work out way to handle multiple objects
        }
    }

    public void toggleSelectionMode(int setting) {  //should i be using switch cases instead of if everywhere?
        selectionMode = setting;
    }

    void toggleSelection(string objName){
        string spriteName = objName + "Sprites";
        if (!objDict.ContainsKey(objName)) {
            objDict.Add(objName, GameObject.Find(objName));
            spritesDict.Add(spriteName, Resources.LoadAll<Sprite>(spriteName));
            choiceDict.Add(objName, selectionMode);
        }
        Image comp = objDict[objName].GetComponent<Image>();
        comp.sprite = spritesDict[spriteName][selectionMode];
        choiceDict[objName] = selectionMode;
    }

    void loadScenario(){
        // scenarioObj.SetActive(true);
        // questionindex = 0;
        // recipientindex = 0;
        // questionText = GameObject.Find("Question").GetComponent<TMP_Text>();
        // if (scenario.messages[0].question != null) {
        //     questionText.SetText(scenario.messages[0].text + scenario.messages[0].question[questionindex].purpose + "?");
        //     recipientText = GameObject.Find("Recipient").GetComponent<TMP_Text>(); //probably cache this at start instead of calling it every time, same with any other finds
        //     recipientText.SetText(scenario.messages[0].question[questionindex].recipients[0]);
        // } else {
        //     Debug.Log("Error: no question found"); //hide question bit and print dialogue text
        // }
    }

    public void changeQuestion(){
        // if (recipientindex < scenario.messages[0].question[questionindex].recipients.Length-1) {
        //     recipientindex += 1;
        //     recipientText = GameObject.Find("Recipient").GetComponent<TMP_Text>();
        //     recipientText.SetText(scenario.messages[0].question[questionindex].recipients[recipientindex]);
        // } else if (questionindex < scenario.messages[0].question.Length-1){
        //     recipientindex = 0;
        //     questionindex += 1;
        //     questionText.SetText(scenario.messages[0].text + scenario.messages[0].question[questionindex].purpose + "?");
        //     recipientText.SetText(scenario.messages[0].question[questionindex].recipients[recipientindex]);
        // } else {
        //     scenarioObj.SetActive(false);
        // }
    }

    public void finishSelection() {
        if (choiceDict.Count == PointCalc.getAnswers().Count) { //actually make it when continue button is clicked AND this is reached so user has chance to change answers if they want
            Debug.Log("Hold on I'm switching into scenario mode");
            deviceChoiceMode = false;
            PointCalc.getOverallPoints(choiceDict); //is this where i want to call this? where do i want to have the points? i guess i display straight away so they know which devices are iot
            // somehow do: here are all the iot devices
            objDict["SofaL"].GetComponent<Button>().enabled = true;
            objDict["SofaR"].GetComponent<Button>().enabled = true;
            foreach (string objName in objDict.Keys) {
                if (spritesDict.ContainsKey(objName + "Sprites")) {
                    objDict[objName].GetComponent<Image>().sprite = spritesDict[objName + "Sprites"][0];
                }
            }
            //feedback on choices now? presumably, so they know which options are interactable for scenarios
            //also make non iot non interactable/add flavour text - need some brancing for that in handle click lol
            GameObject.Find("SelectModeCanvas").SetActive(false);
        } else {
            dialogMan.startDialogue(0); //display in textbox when i have one lmao
        }
    }
}
