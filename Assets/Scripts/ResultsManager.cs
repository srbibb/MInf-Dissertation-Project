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
    private const string HomePageUrl = "https://www.spongehammergames.com/";
    string[] toDisplay = {"", "", ""};
    public GameObject[] tipsDisplay = new GameObject[3];

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
                //TODO: insert link to some resource for them to learn more to be accessible and all 
            
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
        string category = "";
        string explanation = "";
        //how to handle i don't know for westin scale? just hide the button?
        if ((westinChoices[0] == 5 || westinChoices[0] == 4) && (westinChoices[1] == 1 || westinChoices[1] == 2) 
            && (westinChoices[2] == 1 || westinChoices[2] == 2))//Fundamentalist
        {
            category = "Fundamentalist";
            explanation = "This group sees privacy as an especially high value, rejects the claims of many organizations to need or be entitled to get personal information for their "
                +"business or governmental programs, thinks more individuals should simply refuse to give out information they are asked for, and favors enactment of strong federal and state laws "
                +"to secure privacy rights and control organizational discretion. ";

        }
        else if((westinChoices[0] == 1 || westinChoices[0] == 2) && (westinChoices[1] == 5 || westinChoices[1] == 4) 
            && (westinChoices[2] == 5 || westinChoices[2] == 4))//Unconcerned
        {
            category = "Unconcerned";
            explanation = "This is the least common group. This group doesn’t know what the “privacy fuss” is all about, supports the benefits of most organizational programs over warnings about privacy abuse, "
                + "has little problem with supplying their personal information to government authorities or businesses, and sees no need for creating another government bureaucracy (a “Federal "
                + "Big Brother) to protect someone’s privacy. ";
        }
        else//Pragmatist
        {
            category = "Pragmatist";
            // explanation = "This is the most common group. This group weighs the value to them and society of various business or government programs calling for personal information, examines the relevance"
            //     + " and social propriety of the information sought, wants to know the potential risks to privacy or security of their information, looks to see whether fair information practices are being"
            //     + " widely enough observed, and then decides whether they will agree or disagree with specific information activities – with their trust in the particular industry or company"
            //     + " involved being a critical decisional factor. The pragmatists favor voluntary standards and consumer choice over legislation and government enforcement. But they will back"
            //     + " legislation when they think not enough is being done - or meaningfully done - by voluntary means."; //TODO: REWRITE THESE
            explanation = "This is the most common group. You weigh the value of disclosing your personal information, taking into account the relevant risks to privacy and security. The trust you have"
                + " in a company or organisation is likely a deciding factor in whether to share your information. You probably favour the ability for the consumer to choice whether to share their"
                + "information themselves, but support government legislation and enforcement when you feel not enough is done by voluntary means.";
        }
        westinDisplay.text = category;
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
        // compare close friends, partner, neighbours (repeat for each data type)
        foreach (List<Result> r in resultList) {
            if ((normsChoices[0] > 3) && ((r[2].choice == "Yes") || (r[5].choice == "Yes"))) { //close friends
                mismatch[0] += 1;
            }
            if ((normsChoices[1] > 3) && ((r[0].choice == "Yes") || (r[3].choice == "Yes"))) { //partner
                mismatch[1] += 1;
            } 
            if ((normsChoices[9] > 3) && ((r[1].choice == "Yes") || (r[4].choice == "Yes"))) { //neighbour
                mismatch[2] += 1;
            }
            if ((normsChoices[12] > 3) && ((r[0].choice == "Yes") || (r[1].choice == "Yes") || (r[2].choice == "Yes") || (r[6].choice == "Yes") || (r[7].choice == "Yes"))) { //no purpose
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
        shareDisplay.text = string.Format("You shared your {0} information the most times: {1} times.\nYou share your {2} information the least times: {3} times", mostShared, mostSharedVal, leastShared, leastSharedVal);
        putToDisplay(mismatch);

    }

    void putToDisplay(int[] mismatch) {
        if (mismatch[3] > 1) {
            toDisplay[System.Array.IndexOf(toDisplay, "")]  = "You said that you wouldn't want to share your information with no purpose, but during the game scenarios you did so multiple times. "
                + "When considering your privacy preferences, it's important not just to consider what you're sharing, but why you're sharing it, and whether you've been told why.";
        }
        int first = checkDataType(mismatch, 6);
        int second = checkRecipient(mismatch, 0);
        
        if (System.Array.IndexOf(toDisplay, "") != -1 && first != -1) {
            checkDataType(mismatch, first);
        }
        if (System.Array.IndexOf(toDisplay, "") != -1 && second != -1) {
            checkRecipient(mismatch, second);
        }

        for (int i = 0; i <3; i++) {
            tipsDisplay[i].GetComponent<TMP_Text>().text = toDisplay[i];
        }

        if (System.Array.IndexOf(toDisplay, "") != -1) {
            int notFilled = 2;
            if (System.Array.IndexOf(toDisplay, "") < 2) {
                tipsDisplay[2].SetActive(false);
                notFilled = 1;
            }
            if (System.Array.IndexOf(toDisplay, "") < 2) {
                tipsDisplay[1].SetActive(false);
                notFilled = 0;
            }
            tipsDisplay[notFilled].GetComponent<TMP_Text>().text = "You did a good job being consistent to your privacy preferences. Well done!";
        }

    }

    int checkDataType(int[] mismatch, int start) {
        string[] itemList = {"email", "voice recording", "video call", "reminder"};
        int nextFree = -1;
        for (int i = start; i <10; i++) {
            if (mismatch[i] > 1) {
                nextFree = System.Array.IndexOf(toDisplay, "");
                toDisplay[System.Array.IndexOf(toDisplay, "")] = "You said that you wouldn't want to share your " + itemList[i-6] + " information, but during the game scenarios "
                    + "you chose to share it multiple times. It's important to consider whether you're sticking to your own privacy preferences."; // can check still spare slot to add info
                return nextFree;
            }
        }
        return nextFree;
    }

    int checkRecipient(int[] mismatch, int start) {
        string[] recipientList = {"your close friends", "your partner", "your neighbour", "unrelated organisations", "advertisers"};
        int nextFree = -1;
        for (int i = start; i<5; i++) {
            if (mismatch[i] > 1) {
                nextFree = System.Array.IndexOf(toDisplay, "");
                toDisplay[System.Array.IndexOf(toDisplay, "")] = "You said that you wouldn't want to share your information with " + recipientList[i] + ", but during the game scenarios "
                    + "you chose to share it multiple times. It's important to consider not just what you're sharing but who you're sharing it with."; 
                return nextFree;
            }
        }
        return nextFree;
    }


    public void openURL(string url) {
        Application.OpenURL(url);
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
        string contents = "\nNorms Choices:" + string.Join(",", normsChoices) + "\nWestin Choices:"  + string.Join(",", westinChoices);
        resOutput += contents;
        System.IO.File.WriteAllText (fullPath, resOutput);
    }

    public void finish(){
        Application.Quit();
    }
}
