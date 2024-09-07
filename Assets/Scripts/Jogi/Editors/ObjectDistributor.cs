#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Rendering;

[ExecuteInEditMode]
public class ObjectDistributor : MonoBehaviour
{
    public GameObject objectToDistribute;
    public RuntimeAnimatorController[] controllers;
    public int numberOfObjects = 5;
    public float radius = 2f;
    public bool randomRotation = false;
    public bool lookAtTarget = false;

    public void DistributeObjects()
    {
        if (objectToDistribute == null)
        {
            Debug.LogError("Object to distribute is not set.");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * (360f / numberOfObjects);
            Vector3 position = transform.position + Quaternion.Euler(0f, angle, 0f) * (Vector3.forward * radius);

            GameObject newObject = Instantiate(objectToDistribute, position, Quaternion.identity);
            newObject.transform.SetParent(transform, false);

            if (lookAtTarget)
            {
                // Rotate the object to look towards the script's GameObject or away from it
                Vector3 lookDirection = (lookAtTarget ? transform.position : -transform.position) - newObject.transform.position;
                newObject.transform.rotation = Quaternion.LookRotation(lookDirection);
            }
            else
            {
                // Rotate the object to look towards the script's GameObject or away from it
                Vector3 lookDirection = (lookAtTarget ? transform.position : -transform.position) + newObject.transform.position;
                newObject.transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    public void DeleteObjects()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
    }

    public void DistributeInWarzone()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate random positions within the specified range.
            float randomX = Random.Range(-radius, radius);
            float randomZ = Random.Range(-radius, radius);

            // Create a new instance of the object and set its position.
            GameObject newObj = Instantiate(objectToDistribute, transform.position + new Vector3(randomX, 0f, randomZ), Quaternion.identity);
            Animator animator = newObj.GetComponentInChildren<Animator>();
            animator.runtimeAnimatorController = controllers[Random.Range(0, controllers.Length)];
            newObj.transform.SetParent(transform, true);

            // You may want to add more customization for the newly created objects here.
            // For example, you can add random rotations or scales.

            // Parent the new object to the current GameObject (optional).
            newObj.transform.parent = transform;
        }
    }

    private void OnValidate()
    {
        if (numberOfObjects < 1)
            numberOfObjects = 1;

        if (radius < 0)
            radius = 0;
    }
}

[CustomEditor(typeof(ObjectDistributor))]
public class ObjectDistributorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectDistributor distributor = (ObjectDistributor)target;

        DrawDefaultInspector();

        distributor.lookAtTarget = EditorGUILayout.Toggle("Look At Target", distributor.lookAtTarget);

        if (GUILayout.Button("Distribute Objects"))
        {
            distributor.DistributeObjects();
        }

        if (GUILayout.Button("Delete Objects"))
        {
            distributor.DeleteObjects();
        }

        if (GUILayout.Button("Distribute In Warzone"))
        {
            distributor.DistributeInWarzone();
        }
    }
}

#endif