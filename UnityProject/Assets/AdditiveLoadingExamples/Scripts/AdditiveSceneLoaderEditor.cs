using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AdditiveSceneLoader))]

// Custom inspector for Additve Scene Loader
public class AdditiveSceneLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AdditiveSceneLoader scriptName = (AdditiveSceneLoader)target;
        if (GUILayout.Button(scriptName.GetLoadButtonLabel()))
        {
            scriptName.LoadAllScenesInEditor();
        }
    }
}