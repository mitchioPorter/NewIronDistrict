using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {
    public string option;
    public DialogueBox box;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ParseOptions()
    {
        string command = option;
        box.playerChoosing = false;
        if (command == "bad")
        {
            PersistentObject.Instance.badDecision = true;
            box.mode = 1;
            box.lineNum++;
        }
        else
        {
            PersistentObject.Instance.badDecision = false;
            box.mode = 2;  
        }

        //Need to call this to tell code to proceed
        Debug.Log("THIS IS THE VALUE " + PersistentObject.Instance.badDecision);
        box.DialogueDisplay();
    }

    public void SetText(string newText) {
        Debug.Log("INSIDE OF SET TEXT");
        this.GetComponentInChildren<Text>().text = newText;
        Debug.Log("SET TEXT " + newText);
    }

    public void SetOption(string newOption) {
        this.option = newOption;
    }

}
