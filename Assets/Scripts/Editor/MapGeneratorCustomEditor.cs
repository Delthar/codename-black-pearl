using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Randomize"))
        {
            var mapGenerator = (MapGenerator)target;
            mapGenerator.CalcNoise();
        }
        if(GUILayout.Button("Generate Map"))
        {
            var mapGenerator = (MapGenerator)target;
            mapGenerator.GenerateMap();
        }
    }
}
