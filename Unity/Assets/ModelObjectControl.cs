using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

public class ModelObjectControl : MonoBehaviour
{
    public float expansionFactor = 1.5f;
    public float interpolationTime = 0.5f;
    public float rotationTime = 0.75f;

    private Transform objectTransform;
    private Dictionary<Transform, TransformVectors> originalPositions = new Dictionary<Transform, TransformVectors>();

    private Dictionary<Transform, VectorData> vectorData = null;
    private float scaleTimeRemaining;
    private float objectScale = 1.0f;

    private VectorData rotationData = null;
    private float rotationTimeRemaining;


    public bool IsExpanded { get { return objectScale > 1.0f; } }

    // Use this for initialization
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();

        AddOriginalPositions(objectTransform);
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationData != null && rotationTimeRemaining > 0)
        {
            rotationTimeRemaining -= Time.deltaTime;

            Vector3 newVector = Vector3.Lerp(rotationData.StartVector, rotationData.EndVector, (rotationTime - rotationTimeRemaining) / rotationTime);
            objectTransform.eulerAngles = newVector;

            if (rotationTimeRemaining <= 0f)
            {
                rotationData = null;
                rotationTimeRemaining = 0f;
            }
        }

        if (vectorData != null && scaleTimeRemaining > 0)
        {
            scaleTimeRemaining -= Time.deltaTime;

            foreach (var kvp in vectorData)
            {
                if (kvp.Value.TravelDistance == 0f) continue;

                kvp.Key.localPosition = Vector3.Lerp(kvp.Value.StartVector, kvp.Value.EndVector, (interpolationTime - scaleTimeRemaining) / interpolationTime);
            }

            if (scaleTimeRemaining <= 0f)
            {
                vectorData = null;
                scaleTimeRemaining = 0f;
            }
        }
    }

    private void AddOriginalPositions(Transform transform)
    {
        originalPositions.Add(transform, new TransformVectors(transform.localPosition, transform.eulerAngles));
        foreach (Transform child in transform)
        {
            AddOriginalPositions(child);
        }
    }

    #region Rotation

    public void Turn(char axis, float rotationValue)
    {
        rotationTimeRemaining += rotationTime;
        if (rotationData != null)
        {
            AppendTurn(axis, rotationValue);
            return;
        }

        Vector3 rotationVector = MapRotationValue(axis, rotationValue);
        rotationData = new VectorData(objectTransform.eulerAngles, objectTransform.eulerAngles + rotationVector);
    }

    private void AppendTurn(char axis, float rotationValue)
    {
        Vector3 rotationVector = MapRotationValue(axis, rotationValue);
        rotationData = new VectorData(rotationData.StartVector, rotationData.EndVector + rotationVector);
    }

    private Vector3 MapRotationValue(char axis, float rotationValue)
    {
        Vector3 rotationVector = new Vector3();
        switch (axis)
        {
            case 'x':
                rotationVector.x = rotationValue;
                break;
            case 'y':
                rotationVector.y = rotationValue;
                break;
            case 'z':
                rotationVector.z = rotationValue;
                break;
        }

        return rotationVector;
    }

    #endregion

    #region Expand

    public void Expand()
    {
        scaleTimeRemaining += interpolationTime;
        objectScale *= expansionFactor;

        if (vectorData != null)
        {
            AppendExpansion(objectTransform);
            return;
        }

        vectorData = new Dictionary<Transform, VectorData>();
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

        vectorData = tempList;
    }

    #endregion

    #region Collapse

    public void Collapse()
    {
        if (objectScale == 1.0f) return;

        scaleTimeRemaining += interpolationTime;
        objectScale /= expansionFactor;
        if (vectorData != null)
        {
            AppendCollapse(objectTransform);
            return;
        }

        vectorData = new Dictionary<Transform, VectorData>();

        ApplyCollapse(objectTransform);
    }

    private void ApplyCollapse(Transform parent)
    {
        foreach (Transform child in parent)
        {
            ApplyCollapse(child);
            vectorData.Add(child, new VectorData(child.localPosition, child.localPosition / expansionFactor));
        }
    }

    private void AppendCollapse(Transform parent)
    {
        var tempList = new Dictionary<Transform, VectorData>();

        foreach (var kvp in vectorData)
        {
            tempList.Add(kvp.Key, new VectorData(kvp.Key.localPosition, kvp.Value.EndVector / expansionFactor));
        }

        vectorData = tempList;
    }

    #endregion

    public void ResetObject()
    {
        foreach (var kvp in originalPositions)
        {
            kvp.Key.localPosition = kvp.Value.PositionVector;
            kvp.Key.eulerAngles = kvp.Value.RotationVector;
        }

        objectScale = 1.0f;

        vectorData = null;
        rotationData = null;

        rotationTimeRemaining = 0f;
        scaleTimeRemaining = 0f;
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

    private class TransformVectors
    {
        public TransformVectors(Vector3 position, Vector3 rotation)
        {
            this.positionVector = position;
            this.rotationVector = rotation;
        }

        private readonly Vector3 positionVector;
        public Vector3 PositionVector { get { return positionVector; } }

        private readonly Vector3 rotationVector;
        public Vector3 RotationVector { get { return rotationVector; } }
    }
}
