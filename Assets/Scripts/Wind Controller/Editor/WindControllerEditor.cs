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
            windController.SetWindDirection(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            windController.SetWindForce(Random.Range(0f, 3f));
        }
    }
}
