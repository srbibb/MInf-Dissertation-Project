using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
//The ‘next’ field notes which response to show next, I’d recommend using -1 as your default ‘exit’ value.

[System.Serializable]
public class Dialogue : ScriptableObject {
    public string speaker;
    public Message[] messages;
}

[System.Serializable]
public class Message {
    public string text;
    public int next;
}

