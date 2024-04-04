using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    (int, int) points;
    public TMP_Text pointDisplay;
    public TMP_Text pointExp;
    public TMP_Text westinDisplay;
    public TMP_Text westinExp;
    int[] normsChoices = new int[15];
    int[] westinChoices = new int[3];
    Dictionary<string, List<Result>> results;
    public TMP_Text shareDisplay;
    string[] toDisplay = {"", "", ""};
    public GameObject[] tipsDisplay = new GameObject[3];
    public GameObject[] tipsBg = new GameObject[2];
    public GameObject westinBadge;
    string category = "";

    // Start is called before the first frame update
    void Start()
    {
        points = PointCalc.getOverallPoints();
        displayPoints();
        normsChoices = PointCalc.getNorms();
        westinChoices = PointCalc.getWestin();
        displayWestin();
        compareResults();
        writeResults();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void displayPoints() {
        pointDisplay.text = (points.Item1 + " out of " + points.Item2 + " correct.");
        string explanation = "Your points for guessing the IoT items. ";
        switch (points.Item1) {
            case < 3:
                explanation += "You should familiarise yourself with the kinds of smart devices available these days. As you saw, there are some surprising ones, such as smart litter boxes and smart toothbrushes!" +
                    "Being able to recognise them will allow you to protect your data in the future.";
                break;
            
            case < 7:
                explanation += "You got some of them right, but it would be worth brushing up on the kinds of smart devices available thes days. As you saw, there are some surprising ones, " +
                    "such as smart litter boxes and smart toothbrushes! Being able to recognise them will allow you to protect your data in the future.";
                break;
            
            case < 11:
                explanation += "Great job! You got most of them correct, but not quite all of them. Being able to recognise them will allow you to protect your data in the future, so congrats!";
                break;
            
            case 11:
                explanation += "You got all of them correct, very impressive! Being able to recognise them will allow you to protect your data in the future, so congrats!";
                break;
        }
        pointExp.text = explanation;
    }

    void displayWestin() {
        string explanation = "";
        int badge;
        //how to handle i don't know for westin scale? just hide the button?
        if ((westinChoices[0] == 5 || westinChoices[0] == 4) && (westinChoices[1] == 1 || westinChoices[1] == 2) 
            && (westinChoices[2] == 1 || westinChoices[2] == 2))//Fundamentalist
        {
            category = "Fundamentalist";
            explanation = "This is neither the most or least common group. You see your privacy as highly important, and don't agree that organisations are entitled to it. You probably think "
                + "people should refuse to disclose their information, and support legislation to secure rights surrounding privacy.";
            badge = 1;

        }
        else if((westinChoices[0] == 1 || westinChoices[0] == 2) && (westinChoices[1] == 5 || westinChoices[1] == 4) 
            && (westinChoices[2] == 5 || westinChoices[2] == 4))//Unconcerned
        {
            category = "Unconcerned";
            explanation = "This is the least common group. You might not understand what the big fuss about privacy is, and you're not too worried about providing your personal information to authorities "
                + "or other organisations.";
            badge = 0;
        }
        else//Pragmatist
        {
            category = "Pragmatist";
            explanation = "This is the most common group. You weigh the value of disclosing your personal information, taking into account the relevant risks to privacy and security. The trust you have"
                + " in a company or organisation is likely a deciding factor in whether to share your information. You value the ability to choose, "
                + " but support legislation when necessary.";
            badge = 2;
        }
        westinDisplay.text = "You're in the category Privacy " + category + ".";
        westinBadge.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/WestinBadges")[badge];
        westinExp.text = explanation;
    }

    void compareResults() {
        int[] mismatch = {0,0,0,0,0,0,0,0,0,0}; //or make array/dict
        List<int> noShared = new List<int> {0,0,0,0};
        string[] itemList = {"voice recording", "video call", "reminder", "email"};
        string mostShared;
        int mostSharedVal;
        string leastShared;
        int leastSharedVal;
        results = PointCalc.getResults();
        List<List<Result>> resultList = new List<List<Result>> {results["Alexa"], results["TV"], results["Watch"], results["Litter"]};
        int i = 0;
        // compare visitors in general, partner, neighbours (repeat for each data type)
        foreach (List<Result> r in resultList) {
            if ((normsChoices[12] > 3) && ((r[0].choice == "Yes") || (r[1].choice == "Yes") || (r[2].choice == "Yes") || (r[6].choice == "Yes") || (r[7].choice == "Yes") || (r[8].choice == "Yes") || (r[9].choice == "Yes"))) { //no purpose
                mismatch[0] += 1;
            } 
            if ((normsChoices[10] > 3) && ((r[2].choice == "Yes") || (r[5].choice == "Yes"))) { //visitors in general
                mismatch[1] += 1;
            }
            if ((normsChoices[1] > 3) && ((r[0].choice == "Yes") || (r[3].choice == "Yes"))) { //partner
                mismatch[2] += 1;
            } 
            if ((normsChoices[9] > 3) && ((r[1].choice == "Yes") || (r[4].choice == "Yes"))) { //neighbour
                mismatch[3] += 1;
            }
            // unrelated orgs (repeat for each data type)
            if ((normsChoices[13] > 3) && (r[9].choice == "Yes")) { 
                mismatch[4] += 1;
            } 
            // advertisers (repeat for each data type)
            if ((normsChoices[14] > 3) && ((r[8].choice == "Yes") || (r[11].choice == "Yes"))) { 
                mismatch[5] += 1;
            } 

            foreach (Result res in r) {
                if (res.choice == "Yes") {
                    noShared[i] += 1;
                }
            }
            i += 1;
        } 

        // email info (litter only)
        foreach (Result res in resultList[3]) {
            if ((normsChoices[7] > 3) && (res.choice == "Yes")) {
                mismatch[6] += 1;
            }
        }

        // voice recording info (alexa only)
        foreach (Result res in resultList[0]) {
            if ((normsChoices[6] > 3) && (res.choice == "Yes")) {
                mismatch[7] += 1;
            }
        }

        // video call info (tv only)
        foreach (Result res in resultList[1]) {
            if ((normsChoices[11] > 3) && (res.choice == "Yes")) {
                mismatch[8] += 1;
            }
        }

        // share reminder info no purpose (watch only)
        if ((normsChoices[2] > 3) && ((resultList[2][0].choice == "Yes") || (resultList[2][1].choice == "Yes") || (resultList[2][2].choice == "Yes") || (resultList[2][6].choice == "Yes") || (resultList[2][7].choice == "Yes"))) {
            mismatch[9] += 1;
        }

        mostSharedVal = noShared.Max();
        mostShared = itemList[noShared.ToList().IndexOf(mostSharedVal)];
        leastSharedVal = noShared.Min();
        leastShared = itemList[noShared.ToList().IndexOf(leastSharedVal)];
        shareDisplay.text = string.Format("You shared your {0} information the most times: {1} times. You share your {2} information the least times: {3} times. Here's some tips based on your answers and gameplay choices.", mostShared, mostSharedVal, leastShared, leastSharedVal);
        putToDisplay(mismatch);

    }

    void putToDisplay(int[] mismatch) {
        if (mismatch[0] > 1) {
            toDisplay[System.Array.IndexOf(toDisplay, "")] = "You said that you wouldn't want to share your information with no purpose, but during the game scenarios you did so multiple times. "
                + "When considering your privacy preferences, it's important not just to consider what you're sharing, but why you're sharing it, and whether you've been told why.";
        }
        int first = checkDataType(mismatch, 6);
        int second = checkRecipient(mismatch, 1);
        
        if (System.Array.IndexOf(toDisplay, "") != -1 && first != -1) {
            checkDataType(mismatch, (first+1));
        }
        if (System.Array.IndexOf(toDisplay, "") != -1 && second != -1) {
            checkRecipient(mismatch, (second+1));
        }

        for (int i = 0; i <3; i++) {
            tipsDisplay[i].GetComponent<TMP_Text>().text = toDisplay[i];
        }

        if (System.Array.IndexOf(toDisplay, "") != -1) {
            int notFilled = 2;
            if (System.Array.IndexOf(toDisplay, "") < 2) {
                tipsDisplay[2].SetActive(false);
                tipsBg[1].SetActive(false);
                notFilled = 1;
            }
            if (System.Array.IndexOf(toDisplay, "") < 2) {
                tipsDisplay[1].SetActive(false);
                tipsBg[0].SetActive(false);
                notFilled = 0;
            }
            tipsDisplay[notFilled].GetComponent<TMP_Text>().text = "You did a good job being consistent to your privacy preferences. Well done!";
        }

    }

    int checkDataType(int[] mismatch, int start) {
        Debug.Log("start " + start);
        string[] itemList = {"email", "voice recording", "video call", "reminder"};
        int nextFree = -1;
        for (int i = start; i <10; i++) {
            if (mismatch[i] > 1) {
                nextFree = System.Array.IndexOf(toDisplay, "");
                toDisplay[nextFree] = "You said that you wouldn't want to share your " + itemList[i-6] + " information, but during the game scenarios "
                    + "you chose to share it multiple times. It's important to consider whether you're sticking to your own privacy preferences."; // can check still spare slot to add info
                return i;
            }
        }
        return -1;
    }

    int checkRecipient(int[] mismatch, int start) {
        string[] recipientList = {"visitors in general", "your partner", "your neighbour", "unrelated organisations", "advertisers"};
        int nextFree = -1;
        for (int i = start; i<6; i++) {
            if (mismatch[i] > 1) {
                Debug.Log(i + " mismatch " + mismatch[i]);
                nextFree = System.Array.IndexOf(toDisplay, "");
                toDisplay[nextFree] = "You said that you wouldn't want to share your information with " + recipientList[i] + ", but during the game scenarios "
                    + "you chose to share it multiple times. It's important to consider not just what you're sharing but who you're sharing it with."; 
                return i;
            }
        }
        return -1;
    }

    void writeResults() {
        string resOutput = "";
        string fullPath = @"GameResults.txt";

        foreach (string key in results.Keys) {
            resOutput += (key + ":\n");
            foreach (Result res in results[key]) {
                resOutput += (res.printResult() + "\n");
            }
        }
        resOutput += ("\nPoints:" + points);
        string contents = "\nNorms Choices:" + string.Join(",", normsChoices) + "\nWestin Choices:"  + string.Join(",", westinChoices) + "\nWestin Category:" + category;
        resOutput += contents;
        System.IO.File.WriteAllText(fullPath, resOutput);
    }

    public void finish(){
        Application.Quit();
    }
}
