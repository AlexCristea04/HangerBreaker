using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

//Author Xilef992
[System.Serializable]
public class BehaviourStateData
{
    public string stateName;
    public float stateSpeed;
    [Tooltip("Animation trigger! Add the name of the animation in the first field and weather it is a bool or not in the second")]
    public String nameAnimation;
    public bool isTrigger = true;
    [FormerlySerializedAs("playOnEnter")] [Tooltip("Put the audio clip that will play upon entering the state!")]
    public AudioClip stateSound;
}

[CustomEditor(typeof(EnemyAi))]
public class ComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyAi myComponent = (EnemyAi)target;

        // Display default inspector property editor for the list
        DrawDefaultInspector();

        // Add a button to add a new element to the list
        if (GUILayout.Button("Add Behaviour"))
        {
            myComponent.behaviourState.Add(new BehaviourStateData());
        }

        // Display list elements with remove buttons
        for (int i = 0; i < myComponent.behaviourState.Count; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Element " + i);

            myComponent.behaviourState[i].stateName =
                EditorGUILayout.TextField(("Name of Behaviour", myComponent.behaviourState[i].stateName).ToString());
            myComponent.behaviourState[i].stateSpeed =
                EditorGUILayout.FloatField("State Speed", myComponent.behaviourState[i].stateSpeed);
            myComponent.behaviourState[i].nameAnimation =
                EditorGUILayout.TextField("Name of trigger", myComponent.behaviourState[i].nameAnimation).ToString();
            myComponent.behaviourState[i].isTrigger =
                EditorGUILayout.Toggle("Is it trigger?", myComponent.behaviourState[i].isTrigger);

            if (GUILayout.Button("Remove", GUILayout.Width(75)))
            {
                myComponent.behaviourState.RemoveAt(i);
            }

            EditorGUILayout.EndVertical();
        }
    }
}