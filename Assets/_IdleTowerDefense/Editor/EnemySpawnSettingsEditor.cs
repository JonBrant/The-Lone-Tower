using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawnSettings))]
public class EnemySpawnSettingsEditor : Editor  
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EnemySpawnSettings settings = (EnemySpawnSettings)target;
        if (GUILayout.Button("Normalize"))
        {
            settings.NormalizeEntries();
        }
    }
}
