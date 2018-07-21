using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour 
{
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
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //selectedObjectAnimator.SetTrigger("Normalize");
            selectedObjectControl.Contract();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //selectedObjectAnimator.SetTrigger("Expand");
            selectedObjectControl.Expand();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedObjectControl.Normalize();
        }
    }
}
