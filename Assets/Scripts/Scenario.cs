using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scenario", menuName = "Scenario")]

[System.Serializable]
public class Scenario : ScriptableObject {
    public int objectID;
    public string objectName;
    public Message[] messages;
}

[System.Serializable]
public class Message {
    public string text;
    public Response[] responses;
    public Question question;
}

[System.Serializable]
public class Response {
    public int next;
    public string reply;
    public string choice;

}
[System.Serializable]
public class Question {
    public int next;
    public string[] recipients;
    public string purpose;
    public string[] choice;
}