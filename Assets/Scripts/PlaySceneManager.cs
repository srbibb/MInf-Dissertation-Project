using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    Scenario scenario;
    public GameObject scenarioObj;
    TMP_Text questionText;
    TMP_Text[] recipientText = new TMP_Text[3];
    GameObject[] recipientBox = new GameObject[3];
    int recipientIndex = 0;
    int questionIndex = 0;
    int selectionMode = 1; // which decision player is currently making - is iot or is not iot - 1 is yes 2 is no
    bool deviceChoiceMode; // when player switches select mode
    Dictionary<string, GameObject> objDict = new Dictionary<string, GameObject>();
    Dictionary<string, Sprite[]> spritesDict = new Dictionary<string, Sprite[]>();
    Dictionary<string, int> choiceDict = new Dictionary<string,int>(); 
    public DialogueManager dialogueMan;
    Dictionary<string, List<Result>> results = new Dictionary<string, List<Result>>();
    ToggleGroup[] toggleGroups = new ToggleGroup[3];
    GameObject[] groups = new GameObject[3];
    public Toggle[] toggs = new Toggle[6];
    string objectName;
    bool ansDisplayed = false;
    public GameObject helpNote;
    public GameObject finishBtn;
    public TMP_Text scenarioTitle;

    // Start is called before the first frame update
    void Start()
    {
        questionText = GameObject.Find("Question").GetComponent<TMP_Text>();
        recipientBox[0] = GameObject.Find("RecipientBg1");
        recipientBox[1] = GameObject.Find("RecipientBg2");
        recipientBox[2] = GameObject.Find("RecipientBg3");
        recipientText[0] = GameObject.Find("Recipient1").GetComponent<TMP_Text>();
        recipientText[1] = GameObject.Find("Recipient2").GetComponent<TMP_Text>();
        recipientText[2] = GameObject.Find("Recipient3").GetComponent<TMP_Text>();
        groups[0] = GameObject.Find("Group1");
        groups[1] = GameObject.Find("Group2");
        groups[2] = GameObject.Find("Group3");
        toggleGroups[0] = GameObject.Find("Group1").GetComponent<ToggleGroup>();
        toggleGroups[1] = GameObject.Find("Group2").GetComponent<ToggleGroup>();
        toggleGroups[2] = GameObject.Find("Group3").GetComponent<ToggleGroup>();

        scenarioObj.SetActive(false);
        deviceChoiceMode = true;
        objDict.Add("SofaL", GameObject.Find("SofaL"));
        objDict.Add("SofaR", GameObject.Find("SofaR"));
        objDict["SofaL"].GetComponent<Button>().enabled = false; 
        objDict["SofaR"].GetComponent<Button>().enabled = false;
        dialogueMan.startDialogue(1, "PCDialogue");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            Application.Quit();
        } 
        if(Input.GetKeyDown("space") && ansDisplayed == true) {
            changeMode(true);
        }
        if(Input.GetMouseButtonDown(0) && helpNote.activeInHierarchy) {
            helpNote.SetActive(false);
        }


    }

    public void handleClick(string objName){
        Dictionary<string, int> flavourText = new Dictionary<string,int>{
            {"SofaL", 0}, 
            {"SofaR", 0},
            {"Headphones", 1},
            {"Notebook",2},
            {"Clock", 3},
            {"Guitar", 4},
            {"CoffeeCup", 5},
            {"Light", 6},
            {"Book", 7}};
        if (deviceChoiceMode == true && !dialogueMan.active) {
            toggleSelection(objName); 
        } else if (deviceChoiceMode == false && !dialogueMan.active && !ansDisplayed && !scenarioObj.activeInHierarchy) {
            if (Resources.Load<Scenario>("Scenarios/" + objName) != null) {
                if(results.Keys.Count== 0) {
                    loadScenario(objName);
                } else if (!results.ContainsKey(objName)) {
                    loadScenario(objName);
                } else if (results.ContainsKey(objName)) {
                    dialogueMan.startDialogue(7, "PCDialogue");
                }
            } else if (flavourText.ContainsKey(objName)) {
                dialogueMan.startDialogue(flavourText[objName], "FlavourDialogue");
            }
        }
    }

    public void toggleSelectionMode(int setting) {
        selectionMode = setting;
    }

    void toggleSelection(string objName){
        string spriteName = objName + "Sprites";
        if (!objDict.ContainsKey(objName)) {
            objDict.Add(objName, GameObject.Find(objName));
            spritesDict.Add(spriteName, Resources.LoadAll<Sprite>("Sprites/" + spriteName));
            choiceDict.Add(objName, selectionMode);
        }
        Image comp = objDict[objName].GetComponent<Image>();
        comp.sprite = spritesDict[spriteName][selectionMode];
        choiceDict[objName] = selectionMode;
    }

    void loadScenario(string objName){
        objectName = objName;
        string title = "";
        switch (objectName) {
            case "Alexa": 
                title = "Smart Assistant - Voice recording information";
                break;
            case "Litter":
                title = "Smart Litter - Email information";
                break;
            case "TV":
                title = "Smart TV - Voice call information";
                break;
            case "Watch":
                title = "Smart Watch - Reminder information";
                break;
        }
        scenarioTitle.text = "Privacy Settings: " + title;
        scenario = Resources.Load<Scenario>("Scenarios/" + objName);
        results.Add(objName, new List<Result>());
        scenarioObj.SetActive(true);
        questionIndex = 0;
        setQuestion();
    }

    public void finishSelection() {
        if (choiceDict.Count == PointCalc.getAnswers().Count) {
            deviceChoiceMode = false;
            PointCalc.calcOverallPoints(choiceDict);
            dialogueMan.startDialogue(5, "PCDialogue");
        } else {
            dialogueMan.startDialogue(0, "PCDialogue"); 
        }
    }

    public void changeMode(bool showingAns) {
        if (!showingAns) {
            Dictionary<string, int> ans = PointCalc.getAnswers();
            foreach (string objName in objDict.Keys) {
                if (spritesDict.ContainsKey(objName + "Sprites")) {
                    objDict[objName].GetComponent<Image>().sprite = spritesDict[objName + "Sprites"][ans[objName]];
                }
                ansDisplayed = true;
            }
        } else {
            ansDisplayed = false;
            objDict["SofaL"].GetComponent<Button>().enabled = true;
            objDict["SofaR"].GetComponent<Button>().enabled = true;
            foreach (string objName in objDict.Keys) {
                if (spritesDict.ContainsKey(objName + "Sprites")) {
                    objDict[objName].GetComponent<Image>().sprite = spritesDict[objName + "Sprites"][0];
                }
            }
            GameObject.Find("SelectModeCanvas").SetActive(false);
            dialogueMan.startDialogue(4, "PCDialogue");
        }
    }

    public void Submit() {
        recipientIndex = 0;
        Question currQuestion = scenario.question[questionIndex];
        for (int i=0; i < currQuestion.recipients.Length; i++) {
            ToggleGroup tGroup = toggleGroups[i];
            Toggle toggle = tGroup.ActiveToggles().FirstOrDefault();
            results[objectName].Add(new Result(scenario.question[questionIndex].text, scenario.question[questionIndex].recipients[recipientIndex], 
                scenario.question[questionIndex].purpose, toggle.name));
            recipientIndex +=1;
        }

        if (questionIndex < scenario.question.Length-1){
            questionIndex += 1;
            setQuestion();
        } else {
            scenarioObj.SetActive(false);
            objectName = null;
            if (results.Count == 4) {
                finishBtn.SetActive(true);
            }
        }
        
    }

    void setQuestion() {
        Question currQuestion = scenario.question[questionIndex];
        questionText.SetText(currQuestion.text + currQuestion.purpose + "?");
        recipientText[0].SetText(currQuestion.recipients[0]);
        if (currQuestion.recipients.Length > 1){
            recipientText[1].SetText(currQuestion.recipients[1]);
            groups[1].SetActive(true);
            recipientBox[1].SetActive(true);
        } else {
            groups[1].SetActive(false);
            recipientBox[1].SetActive(false);
        }
        if (currQuestion.recipients.Length > 2) {
            recipientText[2].SetText(currQuestion.recipients[2]);
            groups[2].SetActive(true);
            recipientBox[2].SetActive(true);
        } else {
            groups[2].SetActive(false);
            recipientBox[2].SetActive(false);
        }
    }
    
    public void showNote() {
        helpNote.SetActive(true);
    }

    public void finishScene() {
        PointCalc.setResults(results);
        SceneManager.LoadScene("Results");
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

    public string printResult() {
        return string.Format("Question: {0} Recipient: {1} Purpose: {2} Choice: {3}", this.question, this.recipient, this.purpose, this.choice);
    }
}
