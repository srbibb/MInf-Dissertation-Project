using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointCalc
{
    public static Dictionary<string, int> getAnswers() {
        Dictionary<string, int> answerDict = new Dictionary<string, int>{ //update as items are added
            { "Alexa", 1 },
            { "TV", 1 },
            { "Guitar", 2},
            { "Headphones", 2}}; //if dict not necessary just make array
        return answerDict;
    }
    public static (int, int) getOverallPoints(Dictionary<string, int> choiceDict) {
        // save answers in scriptable object and read into dict on open game?
        int total = 0;
        Dictionary<string, int> answerDict = getAnswers();
        foreach (var entry in answerDict.Keys) {
            if (answerDict[entry] == choiceDict[entry]) {
                total += 1;
            }
        }
        return (total, answerDict.Count);
    }

    public static void generateResults() {
        // makes fun fancy results stuff for end of game/reporting after they pick, the in depth stuff about their choices
    }
}
