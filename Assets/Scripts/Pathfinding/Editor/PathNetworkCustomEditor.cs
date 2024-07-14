using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathNetwork))]
public class PathNetworkCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Network"))
        {
            PathNetwork pathNetwork = (PathNetwork)target;
            pathNetwork.GenerateNodes();
        }
    }
}
