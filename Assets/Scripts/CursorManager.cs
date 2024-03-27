using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public DialogueManager dialogMan;
    public Texture2D cursor;
    public GameObject scenarioObj;
    public GameObject helpNote;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver() {
        if ((!dialogMan.active) && (!scenarioObj.activeInHierarchy) && (!helpNote.activeInHierarchy)) {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
    }
       
    void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
