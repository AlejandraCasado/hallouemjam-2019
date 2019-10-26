using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateLighting))]

// Custom inspector for Generate Lighting script
public class GenerateLightingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateLighting scriptName = (GenerateLighting)target;
        if (GUILayout.Button(scriptName.GetGenerateButtonLabel()))
        {
            scriptName.GenerateLightingToggle();
        }
    }
}