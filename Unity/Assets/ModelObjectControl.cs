using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelObjectControl : MonoBehaviour 
{
    private GameObject modelObject;
    private Transform modelObjectTransform;
    private Animator modelObjectAnimator;
    private float objectScale = 1.0f;

	// Use this for initialization
	void Start () 
    {
        modelObject = this.gameObject;
        modelObjectTransform = modelObject.GetComponent<Transform>();
        modelObjectAnimator = modelObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    public void Expand()
    {
        objectScale *= 1.5f;
        foreach (Transform child in modelObjectTransform)
        {
            child.position *= 1.5f;
        }
    }

    public void Contract()
    {
        objectScale /= 1.5f;
        foreach (Transform child in modelObjectTransform)
        {
            child.position /= 1.5f;
        }
    }

    public void Normalize()
    {
        foreach(Transform child in modelObjectTransform)
        {
            child.position = child.position / objectScale;
        }
        objectScale = 1.0f;
    }
}
