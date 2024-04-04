using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scenario", menuName = "Scenario")]

[System.Serializable]
public class Scenario : ScriptableObject {
    public string objectName;
    public Question[] question;
}

[System.Serializable]
public class Question { 
    public string text;
    public string[] recipients;
    public string purpose;
}