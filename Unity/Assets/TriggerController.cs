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
    //public GameObject selectedObject;
    public GameObject engine;
    public GameObject turret;
    private GameObject selectedObject = null;
    private ModelObjectControl selectedObjectControl;
    private Animator selectedObjectAnimator;

    private StreamReader Reader { get; set; }

    // Use this for initialization
	void Start () 
    {
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
            selectedObjectControl.Collapse();
        }
        else if(Input.GetKeyDown(expandKey))
        {
            selectedObjectControl.Expand();
        }
        else if(Input.GetKeyDown(normalizeKey))
        {
            selectedObjectControl.ResetObject();
        }

        if (Reader == null || Reader.Peek() == -1) return;
        var line = Reader.ReadLine();
        if (string.IsNullOrEmpty(line)) return;
        char[] charArray = {' '};

        string[] splitLine = line.Split(charArray, StringSplitOptions.RemoveEmptyEntries);
        var command = splitLine[0];

        switch (command.ToLower().Trim())
        {
            case "collapse": selectedObjectControl.Collapse();
                break;
            case "expand": selectedObjectControl.Expand();
                break;
            case "show": MapItemNameToPrefab(splitLine[1].ToLower().Trim());
                break;
            case "remove": GameObject.Destroy(selectedObject);
                break;
            case "turn": MapWordToDirection(splitLine[1].ToLower().Trim());
                break;
            case "reset": selectedObjectControl.ResetObject();
                break;
        }
    }

    private void MapWordToDirection(string direction)
    {
        switch(direction)
        {
            case "up": selectedObjectControl.Turn('x', -45f);
                break;
            case "down": selectedObjectControl.Turn('x', 45f);
                break;
            case "right": selectedObjectControl.Turn('y', 45f);
                break;
            case "left": selectedObjectControl.Turn('y', -45f);
                break;
            case "forward": selectedObjectControl.Turn('x', 45f);
                break;
            case "back": selectedObjectControl.Turn('x', -45f);
                break;
        }
    }

    private void MapItemNameToPrefab(string itemName)
    {
        switch (itemName)
        {
            case "engine": InstantiatePrefab(engine);
                break;
            case "turret": InstantiatePrefab(turret);
                break;
        }
    }

    private void InstantiatePrefab(GameObject prefab)
    {
        if(selectedObject != null) GameObject.Destroy(selectedObject);

        selectedObject = GameObject.Instantiate(prefab);
        selectedObjectControl = selectedObject.GetComponent<ModelObjectControl>();
    }
}
