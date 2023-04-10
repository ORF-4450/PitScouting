using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(ConfigurableInput))]
public class ConfigOptionEditor : Editor
{
    ConfigurableInput Script;
    public override void OnInspectorGUI ()
	{
        Script = (ConfigurableInput)target;

    //#ConfigOptions Script
        GUILayout.BeginHorizontal("box"); //Begins Data Input 
            GUILayout.Label("Configuration Options Script");
            Script.configOptionsScript = (ConfigurableInputOptions)EditorGUILayout.ObjectField(Script.configOptionsScript, typeof(ConfigurableInputOptions), true);
        GUILayout.EndHorizontal();
    //
        if (Script.configOptionsScript != null)
        {
            if (Script.configOptions.Count == 0)
            {
                reloadData();
            }

            //#dataKey
                GUILayout.BeginHorizontal("box"); //Begins Data Input 
                    GUILayout.Label("Data Key");
                    Script.dataKey = GUILayout.TextField(Script.dataKey);
                GUILayout.EndHorizontal();
            //#label
                GUILayout.BeginHorizontal("box"); //Begins Data Input 
                    GUILayout.Label("UI Label");
                    Script.label = GUILayout.TextField(Script.label);
                GUILayout.EndHorizontal();
    //#Buttons
        //GUILayout.BeginHorizontal();
        //#Activate On Load
            Script.activation = GUILayout.Toggle(Script.activation, "Activate on Load");
        //#Reload Dropdown
            if (GUILayout.Button("Reload Data Input Dropdown"))
                reloadData();
        //GUILayout.EndHorizontal();
    //#Dropdown
        //#Create Dropdown
            EditorGUI.BeginChangeCheck();
            Script.selectedConfig = EditorGUILayout.Popup("Input Type", Script.selectedConfig, Script.configOptions.Keys.ToArray());
        //#On Dropdown Value Changed
            if (EditorGUI.EndChangeCheck())
            {
            //#Destroy Children
                foreach (Transform child in Script.gameObject.transform)
                    {
                        if (Application.isPlaying)
                            Destroy(child.gameObject);
                        else
                            DestroyImmediate(child.gameObject);
                    }
            //#Create New Children
                reloadData();
                Script.Setup();
            }
        }

    }
    public void reloadData()
    {
        Script.configOptions.Clear();
        Script.fillConfigOptions();
    }
}