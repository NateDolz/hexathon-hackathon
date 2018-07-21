using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TriggerController : MonoBehaviour 
{
    public KeyCode contractKey = KeyCode.Alpha1;
    public KeyCode expandKey = KeyCode.Alpha2;
    public KeyCode normalizeKey = KeyCode.Alpha3;
    public GameObject selectedObject;
    private ModelObjectControl selectedObjectControl;
    private Animator selectedObjectAnimator;

    private StreamReader Reader { get; set; }

    // Use this for initialization
	void Start () 
    {
        selectedObjectControl = selectedObject.GetComponent<ModelObjectControl>();
        selectedObjectAnimator = selectedObject.GetComponent<Animator>();
        var tempPath = Path.GetTempPath();
        var filePath = tempPath + @"\SpeechCommands.txt";
        var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        Reader = new StreamReader(fs);        
    }
	
	// Update is called once per frame
	void Update () 
    {
		if(Input.GetKeyDown(contractKey))
        {
            //selectedObjectAnimator.SetTrigger("Normalize");
            selectedObjectControl.Contract();
        }
        else if(Input.GetKeyDown(expandKey))
        {
            //selectedObjectAnimator.SetTrigger("Expand");
            selectedObjectControl.Expand();
        }
        else if(Input.GetKeyDown(normalizeKey))
        {
            selectedObjectControl.Normalize();
        }

        if (Reader.Peek() == -1) return;
        var line = Reader.ReadLine();
        if (string.IsNullOrEmpty(line)) return;
        char[] charArray = {' '};
        var command = line.Split(charArray, StringSplitOptions.RemoveEmptyEntries)[0];
        switch (command.ToLower().Trim())
        {
            case "collapse": selectedObjectControl.Contract();
                break;
            case "expand": selectedObjectControl.Expand();
                break;
        }
    }
}
