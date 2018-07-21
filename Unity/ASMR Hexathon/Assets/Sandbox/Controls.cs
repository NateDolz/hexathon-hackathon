using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    private bool isHidden = false;
    private bool isScaling = false;
    Vector3 target;
    Vector3 currentTransform;

    // Use this for initialization
    void Start () {
        currentTransform = transform.localScale;
        target = new Vector3(5, 5, 5);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHidden) {
                gameObject.GetComponent<Renderer>().enabled = false;
                isHidden = true;
            }
            else
            {
                gameObject.GetComponent<Renderer>().enabled = true;
                isHidden = false;
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (transform.localScale != new Vector3(5, 5, 5))
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (Input.GetKey(KeyCode.X))
        {
            if (transform.localScale != new Vector3(0,0,0))
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine("LerpScale", 1f);
        }

    }

    IEnumerator LerpScale(float time)
    {
        var originalScale = transform.localScale;
        var targetScale = originalScale + new Vector3(5.0f, 5.0f, 5.0f);
        var originalTime = time;

        while (time > 0.0f)
        {
            time -= Time.deltaTime;

            transform.localScale = Vector3.Lerp(targetScale, originalScale, time / originalTime);
            yield return null;
        }
    }

}
