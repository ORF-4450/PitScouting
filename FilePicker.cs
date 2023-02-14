using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;

public class FilePicker : MonoBehaviour {
	public DataStorage DS;
	public GameObject MainMenu;
	public GameObject ScoutingMenu;
	public GameObject FileMenu;
	public Dropdown dropdown;
	public Text dataViewer;
	public FileInfo file;
	public Text resultText;
	public RawImage DisplayedImage;

	private string tmpFileName = null;

	//This script runs the file selection menu.

	public void loadMenu () { //Set hardcoded objects' SetActive bool to true, making them appear.
	//#Show Menus. This allows the script to load data into *every* object when data is loaded.
		FileMenu.SetActive (true);
		MainMenu.SetActive (true);
		ScoutingMenu.SetActive (true);
	}

	public void Start() {
	//#Set DS to <DataStorage> component
		DS = GetComponent<DataStorage>();
	//
	}


	public void LateUpdate() {
	//#Stop if dropdown has unexpected value
		if (dropdown == null)
			return;
		if (dropdown.value == 0 || dropdown.options[dropdown.value].text == ".DS_Store")
			return;
	//
		string tmp_path = Application.persistentDataPath + Path.DirectorySeparatorChar + dropdown.captionText.text;
		FileInfo tmp_file = new FileInfo (Application.persistentDataPath + Path.DirectorySeparatorChar + dropdown.captionText.text);
	//#If file is empty, set file to tmp_file
        if (file == null)
        {
            if (tmp_file.Exists)
            {
                file = tmp_file;
                dataViewer.text = getStringFromFile(file);
            }
        }
	//#If file does not exist and is not null, set dropdown to 0 and stop.
        if (file != null && !file.Exists)
        {
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            return;
        }
	//#If file equals tmp_file, stop. Else, set file to tmp_file
		if (tmp_file.Equals(file))
			return;
		file = tmp_file;
	//#If file is .TXT, read as text.
		if (file.Extension == ".txt") //If the file is a .txt, read as text
		{
			dataViewer.text = getStringFromFile(file);
		}
	//#If file is .PNG, read as image
		if (file.Extension == ".png") //if the file is a .png, read as an image
		{			
			DisplayedImage.enabled = true; //Enable the RawImage 
			if (tmpFileName != tmp_path)
			{
				Texture2D tex = new Texture2D(2, 2);
				Debug.Log(tmp_path);
				tex.LoadImage(File.ReadAllBytes(tmp_path)); //Load the image into Texture2D tex
				DisplayedImage.texture = tex; //Set RawImage to Texture2D tex
				Debug.Log("LoadedImage");
				tmpFileName = tmp_path;
			}
		} else {
		DisplayedImage.enabled = false;
		}
	//
	}

	public void Update() {
	//#Stop if dropdown has unexpected value
		if (dropdown == null)
			return;
	//
		List<Dropdown.OptionData> files = new List<Dropdown.OptionData>();
		DirectoryInfo directory = new DirectoryInfo (Application.persistentDataPath);
		files.Add (new Dropdown.OptionData("Choose a file"));
	//#Add File names to "files"
		foreach (FileInfo file in directory.GetFiles()) {
			if (file.Name.StartsWith (".") || file.Extension == ".json")
				continue;
			files.Add (new Dropdown.OptionData (file.Name));
		}
	//#Set dropdown values to "files"
		if (dropdown.options != files) {
			dropdown.options = files;
			dropdown.RefreshShownValue ();
		}
	}

	public string getStringFromFile(FileInfo file) { //Only works with .TXT files
	//#Stop if an error is encountered
        if (!file.Exists) return "File not found.";
	//#Read .TXT file
		StreamReader sr = file.OpenText();
		string data = sr.ReadToEnd ();
		sr.Close ();
		return data;
	}

	public void loadFromSelectedFile() {
	//#Stop if an error is encountered
		if (file == null)
			return;
	//
		string raw = getStringFromFile (file);
	//#By deafult, data is invalid. If a version tag is included, it becomes valid data.
		bool valid = false;
		foreach (string line in raw.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)) {
			if (line.Split (';') [0] == "Version")
				valid = true;
		}
		//If error is encountered, send warning message and stop.
			if (!valid) {
				resultText.text = "Error: File is not valid, missing Version tag.";
        	    Debug.LogWarning("Error: File is not valid, missing Version tag. Raw data: " + raw);
				return;
		}
	//
		foreach (string line in raw.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)) {
			if (line == null)
				continue;
		//Format data to read
			string[] brokenString = line.Split (';'); //Splits data by the ";" character.
		//#Test for version mismatch
			if (brokenString [0].Equals ("Version") && brokenString [1] != DS.data ["Version"]) {
				resultText.text = "Error: Version mismatch between " + DS.data ["Version"] + " and " + brokenString [1];
				return;
			}
		//
			Debug.Log ("Attempting to access at key " + brokenString [0]);
		//If a key without a user input is encountered, stop.
			if (!DS.inputs.ContainsKey(brokenString[0]))
				continue;
		//Set user input value to correlated value in brokenString
			DS.inputs[brokenString[0]].changeData(brokenString[1]);
			resultText.text = "The file has been loaded!";
		//
		}
	}

    public void clearFiles()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
	//#Delete local .txt or .png files in directory
        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.Extension == ".txt" || file.Extension == ".png")
            {
                resultText.text = "Deleting file " + file.Name;
                file.Delete();
                resultText.text = "Deleted file " + file.Name;
            }
        }
	//#Delete downloaded files
        resultText.text = "Finished deleting unuploaded files. Clearing uploaded files.";
        DirectoryInfo uploaded = new DirectoryInfo(Application.persistentDataPath + Path.DirectorySeparatorChar + "uploaded");
        if (uploaded.Exists)
        {
            foreach (DirectoryInfo folder in uploaded.GetDirectories())
            {
                if (folder.GetFiles().Length > 0)
                {
                    foreach (FileInfo file in folder.GetFiles())
                    {
                        resultText.text = "Deleting file " + file.Name + " in uploaded" + Path.DirectorySeparatorChar + folder.Name;
                        file.Delete();
                        resultText.text = "Deleted file " + file.Name + " in uploaded" + Path.DirectorySeparatorChar + folder.Name;
                    }
                }
                resultText.text = "Deleting folder for team " + folder.Name;
                folder.Delete();
                resultText.text = "Deleted folder for team " + folder.Name;
            }
            resultText.text = "Deleting upload folder.";
            uploaded.Delete();
            resultText.text = "Deleted upload folder.";
        }
        resultText.text = "Clearing done.";
		tmpFileName = null;
    }
}
