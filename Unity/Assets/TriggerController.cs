using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour 
{
    public KeyCode contractKey = KeyCode.Alpha1;
    public KeyCode expandKey = KeyCode.Alpha2;
    public KeyCode normalizeKey = KeyCode.Alpha3;
    public GameObject selectedObject;
    private ModelObjectControl selectedObjectControl;
    private Animator selectedObjectAnimator;

	// Use this for initialization
	void Start () 
    {
        selectedObjectControl = selectedObject.GetComponent<ModelObjectControl>();
        selectedObjectAnimator = selectedObject.GetComponent<Animator>();
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
    }
}
