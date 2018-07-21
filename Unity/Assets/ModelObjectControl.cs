using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

public class ModelObjectControl : MonoBehaviour 
{
    public float expansionFactor = 1.5f;
    public float interpolationTime = 0.5f;
    
    private Transform objectTransform;
    private Animator animator;
    private Dictionary<Transform, VectorData> vectorData = null;
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private float timeRemaining;
    private float objectScale = 1.0f;

    public bool IsExpanded { get { return objectScale > 1.0f; } }

    // Use this for initialization
    void Start () 
    {
        objectTransform = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();

        AddOriginalPositions(objectTransform);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (vectorData == null) return;

        timeRemaining -= Time.deltaTime;

        foreach(var kvp in vectorData)
        {
            if (kvp.Value.TravelDistance == 0f) continue;

            kvp.Key.localPosition = Vector3.Lerp(kvp.Value.StartVector, kvp.Value.EndVector, (interpolationTime - timeRemaining) / interpolationTime);
        }

        if (timeRemaining <= 0f) vectorData = null;
    }

    private void AddOriginalPositions(Transform transform)
    {
        originalPositions.Add(transform, transform.localPosition);
        foreach(Transform child in transform)
        {
            AddOriginalPositions(child);
        }
    }

    public void Expand()
    {
        objectScale *= expansionFactor;

        if (vectorData != null)
        {
            AppendExpansion(objectTransform);
            return;
        }
        else
        { 
            vectorData = new Dictionary<Transform, VectorData>();
        }

        timeRemaining = interpolationTime;

        ApplyExpansion(objectTransform);
    }

    private void ApplyExpansion(Transform parent)
    {
        foreach (Transform child in parent)
        {
            ApplyExpansion(child);
            vectorData.Add(child, new VectorData(child.localPosition, child.localPosition * expansionFactor));
        }
    }

    private void AppendExpansion(Transform parent)
    {
        var tempList = new Dictionary<Transform, VectorData>();
        
        foreach (var kvp in vectorData)
        {
            tempList.Add(kvp.Key, new VectorData(kvp.Key.localPosition, kvp.Value.EndVector * expansionFactor));
        }

        timeRemaining = interpolationTime;
        vectorData = tempList;
    }

    public void Contract()
    {
        if (objectScale == 1.0f) return;

        objectScale /= expansionFactor;
        if (vectorData != null)
        {
            AppendContraction(objectTransform);
            return;
        }
        else
        {
            vectorData = new Dictionary<Transform, VectorData>();
        }

        timeRemaining = interpolationTime;

        ApplyContraction(objectTransform);
    }

    private void ApplyContraction(Transform parent)
    {
        foreach (Transform child in parent)
        {
            ApplyContraction(child);
            vectorData.Add(child, new VectorData(child.localPosition, child.localPosition / expansionFactor));
        }
    }

    private void AppendContraction(Transform parent)
    {
        var tempList = new Dictionary<Transform, VectorData>();

        foreach (var kvp in vectorData)
        {
            tempList.Add(kvp.Key, new VectorData(kvp.Key.localPosition, kvp.Value.EndVector / expansionFactor));
        }

        timeRemaining = interpolationTime;
        vectorData = tempList;
    }

    public void Normalize()
    {
        foreach(var kvp in originalPositions)
        {
            kvp.Key.localPosition = kvp.Value;
        }

        objectScale = 1.0f;
        vectorData = null;
    }

    private class VectorData
    {
        public VectorData(Vector3 startVector, Vector3 endVector)
        {
            this.startVector = startVector;
            this.endVector = endVector;

            this.travelDistance = Vector3.Distance(startVector, endVector);
        }

        private readonly Vector3 startVector;
        public Vector3 StartVector { get { return startVector; } }

        private readonly Vector3 endVector;
        public Vector3 EndVector { get { return endVector; } }

        private readonly float travelDistance;
        public float TravelDistance { get { return travelDistance; } }
    }
}
