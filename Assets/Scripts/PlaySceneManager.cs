using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PlaySceneManager : MonoBehaviour
{
    //public TMP_Text response;
    public Scenario scenario;
    public GameObject scenarioObj;
    TMP_Text questionText;
    TMP_Text[] recipientText = new TMP_Text[3];
    int recipientindex = 0;
    int questionindex = 0;
    int selectionMode = 1; // which decision player is currently making - is iot or is not iot - 1 is yes 2 is no
    bool deviceChoiceMode; // when player switches select mode
    //need something that checks player has done all objects when they click to continue and stops if they haven't
    Dictionary<string, GameObject> objDict = new Dictionary<string, GameObject>();
    Dictionary<string, Sprite[]> spritesDict = new Dictionary<string, Sprite[]>();
    Dictionary<string, int> choiceDict = new Dictionary<string,int>(); //not sure if this is the best way? how to store correct answers? just do it in array and have list of objects and answers? or save their choices in array
    // in order of dictionary with correct answers might be better than both in dict or both in array bc then can check dict actually
    public DialogueManager dialogMan;
    int[] answers;
    //Dictionary<(string, string), string> alexaRes = new Dictionary<(string, string), string>(); //(recipient, purpose), choice
    //2d list with question and each recipient response
    List<Result> alexaRes = new List<Result>();
    //needs to get device obj to know where to save
    ToggleGroup[] toggleGroups = new ToggleGroup[3];

    // Start is called before the first frame update
    void Start()
    {
        questionText = GameObject.Find("Question").GetComponent<TMP_Text>();
        recipientText[0] = GameObject.Find("Recipient1").GetComponent<TMP_Text>();
        recipientText[1] = GameObject.Find("Recipient2").GetComponent<TMP_Text>();
        recipientText[2] = GameObject.Find("Recipient3").GetComponent<TMP_Text>();
        toggleGroups[0] = GameObject.Find("Group1").GetComponent<ToggleGroup>();
        toggleGroups[1] = GameObject.Find("Group2").GetComponent<ToggleGroup>();
        toggleGroups[2] = GameObject.Find("Group3").GetComponent<ToggleGroup>();

        scenarioObj.SetActive(false);
        deviceChoiceMode = true;
        objDict.Add("SofaL", GameObject.Find("SofaL"));
        objDict.Add("SofaR", GameObject.Find("SofaR"));
        objDict["SofaL"].GetComponent<Button>().enabled = false; // so they don't interfere with headphones, turn back on when doing scenarios to add flavour text
        objDict["SofaR"].GetComponent<Button>().enabled = false;
        dialogMan.startDialogue(1, "PCDialogue");
        //TODO: can click objs while textbox up - make it so can't do that
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
            //check if flavour text, check if scenario already complete
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
        scenarioObj.SetActive(true);
        questionindex = 0;
        recipientindex = 0;
        //check how many recipients and adjust if 2 or 1
        if (scenario.question[questionindex] != null) {
            questionText.SetText(scenario.question[questionindex].text + scenario.question[questionindex].purpose + "?");
            recipientText[0].SetText(scenario.question[questionindex].recipients[0]);
            recipientText[1].SetText(scenario.question[questionindex].recipients[1]);
            recipientText[2].SetText(scenario.question[questionindex].recipients[2]);
        } else {
            Debug.Log("Error: no question found"); //hide question bit and print dialogue text
        }
    }

    public void changeQuestion(int choice){
        if (recipientindex < scenario.question[questionindex].recipients.Length-1) {
            recipientindex += 1;
            recipientText[0].SetText(scenario.question[questionindex].recipients[recipientindex]);
        } 
    }

    public void finishSelection() {
        if (choiceDict.Count == PointCalc.getAnswers().Count) { //actually make it when continue button is clicked AND this is reached so user has chance to change answers if they want
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
            dialogMan.startDialogue(4, "PCDialogue");
        } else {
            dialogMan.startDialogue(0, "PCDialogue"); 
        }
    }

    public void Submit() {
        recipientindex = 0;
        foreach (ToggleGroup tGroup in toggleGroups) {
            Toggle toggle = tGroup.ActiveToggles().FirstOrDefault();
            //need to use recipient index here to add each one seapartely
            //also make it so can't redo items
            alexaRes.Add(new Result(scenario.question[questionindex].text, scenario.question[questionindex].recipients[recipientindex], 
                scenario.question[questionindex].purpose, toggle.name));
            recipientindex +=1;
        }
        foreach (var val in alexaRes) {
            Debug.Log(val.choice);
        }

        if (questionindex < scenario.question.Length-1){
            questionindex += 1;
            questionText.SetText(scenario.question[questionindex].text + scenario.question[questionindex].purpose + "?");
            recipientText[0].SetText(scenario.question[questionindex].recipients[0]);
            recipientText[1].SetText(scenario.question[questionindex].recipients[1]);
            recipientText[2].SetText(scenario.question[questionindex].recipients[2]);
        } else {
            scenarioObj.SetActive(false);
        }
    }
}

public class Result
{
    public string question;
    public string recipient;
    public string purpose;
    public string choice;

    public Result(string q, string r, string p, string c) {
        this.question = q.Trim();
        this.recipient = r.Trim();
        this.purpose = p.Trim();
        this.choice = c.Trim();
    }
}
