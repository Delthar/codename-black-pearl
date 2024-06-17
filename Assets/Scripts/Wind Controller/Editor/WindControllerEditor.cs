using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindController))]
public class WindControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Randomize"))
        {
            WindController windController = (WindController)target;
            windController.Randomize();
        }
    }
}
