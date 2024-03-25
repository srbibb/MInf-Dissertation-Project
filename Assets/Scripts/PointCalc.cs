using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointCalc
{
    static (int, int) points;
    static int[] normsChoices = new int[15];
    static int[] westinChoices = new int[3];
    static Dictionary<string, List<Result>> results = new Dictionary<string, List<Result>>();

    public static Dictionary<string, int> getAnswers() {
        Dictionary<string, int> answerDict = new Dictionary<string, int>{ //update as items are added
            { "Alexa", 1 },
            { "TV", 1 },
            { "Guitar", 2},
            { "Headphones", 2},
            { "Watch", 1},
            { "Litter", 1},
            { "Light", 1},
            { "Book", 2},
            { "CoffeeCup", 2},
            { "Clock", 2},
            { "Notebook", 2}}; //if dict not necessary just make array
        return answerDict;
    }

    public static void calcOverallPoints(Dictionary<string, int> choiceDict) {
        // save answers in scriptable object and read into dict on open game?
        int total = 0;
        Dictionary<string, int> answerDict = getAnswers();
        foreach (var entry in answerDict.Keys) {
            if (answerDict[entry] == choiceDict[entry]) {
                total += 1;
            }
        }
        points = (total, answerDict.Count);
    }

    public static (int, int) getOverallPoints() {
        return points;
    }

    public static void setNorms(int[] choices) {
        normsChoices = choices;
    }
    
    public static void setWestin(int[] choices) {
        westinChoices = choices;
    }

    public static int[] getNorms() {
        return normsChoices;
    }
    
    public static int[] getWestin() {
        return westinChoices;
    }

    public static void setResults(Dictionary<string, List<Result>> choices) {
        results = choices;
    }
    
    public static Dictionary<string, List<Result>> getResults() {
        return results;
    }

    public static void generateResults() {
        // makes fun fancy results stuff for end of game/reporting after they pick, the in depth stuff about their choices
    }
}
