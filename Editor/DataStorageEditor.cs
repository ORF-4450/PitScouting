using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(DataStorage))]
public class DataStorageEditor : Editor {

	string StatusMessage = "";

	public override void OnInspectorGUI ()
	{
		DataStorage Script = (DataStorage)target;
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Version Number:");
        for (int i = 0; i < 3; i++)
        {
            Script.Version[i] = GUILayout.TextField(Script.Version[i]);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box"); //Begins Data Input 
        GUILayout.Label("Upload Result Text:"); //Upload Result Text Box Object
        Script.uploadResultText = (ResultText)EditorGUILayout.ObjectField(Script.uploadResultText, typeof(ResultText), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box"); //Begins Data Input 
        GUILayout.Label("Download Result Text:"); //Upload Result Text Box Object
        Script.downloadResultText = (ResultText)EditorGUILayout.ObjectField(Script.downloadResultText, typeof(ResultText), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal ("box"); //Begins Data Input 
		GUILayout.Label ("Base Server URL:"); //URL to recieve data for events from
		Script.serverBaseURL = GUILayout.TextField (Script.serverBaseURL);
		GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal("box"); //Begins Data Input 
        GUILayout.Label("Google Form Script"); //Script DataStorage should refer to when uploading
        Script.GoogleForm = (GoogleForm)EditorGUILayout.ObjectField(Script.GoogleForm, typeof(GoogleForm), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box"); //Begins Data Input 
        GUILayout.Label("Datastorage Key"); //String that goes in front of file name
        Script.dataStorageKey = GUILayout.TextField(Script.dataStorageKey);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box"); //Begins Data Input 
        GUILayout.Label("App Name"); //Legacy
        Script.appName = GUILayout.TextField(Script.appName);
        GUILayout.EndHorizontal();

        /* TEMPLATE
        GUILayout.BeginHorizontal("box"); //Begins Data Input 
        GUILayout.Label("PUT_LABEL_HERE"); //Label of data input box
        Script.VARIABLE = GUILayout.TextField(Script.VARIABLE); //Sets variable to what is in the box
        GUILayout.EndHorizontal(); //Ends Data Input
        */

		if (Script.data.Count <= 0) {
			GUILayout.Label ("Currenty no data to display!");
			return;
		}
		foreach (KeyValuePair<string,string> kvp in Script.data) {
			GUILayout.BeginHorizontal ("box");
			GUILayout.Label (kvp.Key); //Sets label to variable
			GUILayout.Label (kvp.Value);
			GUILayout.EndHorizontal ();
		}
		if (GUILayout.Button ("Click to save to file and clear!")) {
			StatusMessage = "Data saved to: " + Script.saveToFile (true);
		}
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Click to Download.")) //If button is true, run function
        {
            Script.StartCoroutine(Script.downloadJson());
        }

        if (GUILayout.Button("Click to Sync.")) //If button is true, run function
        {
            Script.sync();
        }

        if (GUILayout.Button("Click to Upload.")) //If button is true, run function
        {
            Script.StartCoroutine(Script.uploadData());
        }
        GUILayout.EndHorizontal();
        GUILayout.Label (StatusMessage);
	}
}
