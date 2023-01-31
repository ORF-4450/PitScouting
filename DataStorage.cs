using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.Networking;


public class DataStorage : MonoBehaviour
{
    //#Data Storage Variables
        public Dictionary<string, string> data = new Dictionary<string, string>();
        public Dictionary<string, DataInput> inputs = new Dictionary<string, DataInput>();\
    //
    public string serverBaseURL = "https://orf-4450scoutingapp.azurewebsites.net"; //Website to pull competition data off of.
    public string[] Version = { "2022", "0", "0" }; //Version number, doesn't mean much.
    public GoogleForm GoogleForm;

    [SerializeField] public string dataStorageKey;
    public string appName; //LEGACY

    private UnityWebRequest currentDownloadRequest;
    private UnityWebRequest currentUploadRequest;
    public ResultText uploadResultText;
    public ResultText downloadResultText; //LEGACY

    private bool lastPingTest = false;
//Having issues displaying these? Check out DataStorageEditor.cs


    /*
     *  DataStorage is essentially a library for the rest of the code, in addition to storing data.
     *  Scouting data is stored in .txt files on the device (along with .png's).
     *  While inputting the data, it's stored in an array.
     *  DataStorage will also convert this array to a .txt
     *  DataStorage is the reason this app is able to exist.
     */

    /*
     *  Not only does DataStorage do those things, but it also interacts with the server.
     *  DataStorage will take a json file from the server and read it out as the current Events and Teams for those events.
     *  DataStorage will also send data to the server as an array. On the server's side it is converted to json.
     */


    // Use this for initialization
    void Start()
    {

        //Setup static values
        data.Add("Version", "v" + Version[0] + "." + Version[1] + "." + Version[2]);
        data.Add("ScouterName", "");
    }

    void LateUpdate()
    {
        if (currentDownloadRequest != null)
        {
            uploadResultText.setText("Download JSON progress: " + currentDownloadRequest.downloadProgress.ToString("P2"));

        }
        if (currentUploadRequest != null)
        {
            //float progress = (currentUploadRequest.uploadProgress + currentUploadRequest.progress) / 2;
            uploadResultText.setText("Upload data progress: " + currentUploadRequest.uploadProgress.ToString("P2"));

        }
    }

    public bool addData(string key, string value, bool overwrite)
    {
        try
        {
            data.Add(key, value); //adds (key, value) to data, a <string, string> dictionary
            return true;
        }
        catch (ArgumentNullException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            if (!overwrite)
            {
                return false;
            }
            data[key] = value; //Set the corresponding string for the string 'key' to value
            return true;
        }
    }

    public bool addData(string key, string value, bool overwrite, DataInput DI) //This second addData allows 'addData' to use 4 inputs (previous used 3, so you can call either version). This is called an overload function.
    {
        try
        {
            inputs.Add(key, DI); //string 'key', and DataInput
            Debug.Log("Added input at key " + key);
        }
        catch (ArgumentNullException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            if (!overwrite)
            {
                return false;
            }
            inputs[key] = DI;
        }
        return addData(key, value, overwrite);
    }

    /** 
	*Saves the current data to the file system.
	*If clear is true, clears the data from the DataStorage after saving it.
	**/

    public string saveToFile()
    {
        return saveToFile(false);
    }

    public void saveAndClear()
    {
        saveToFile(true);
    }

    /** 
	*Saves the current data to the file system.
	*If clear is true, clears the data from the DataStorage after saving it.
	**/
    public string saveToFile(bool clear)
    {
        string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + dataStorageKey + data["TeamNumber"] + ".txt"; //Default path, may need adjusting if duplicate fils
        if (File.Exists(filePath))
        {
            int maxCount = 1000;
            for (int i = 1; i < maxCount + 1; i++)
            {
                string tmpFilePath = Application.persistentDataPath + Path.DirectorySeparatorChar + dataStorageKey + data["TeamNumber"] + "-" + (maxCount - i + 1) + ".txt"; //Try different possibilities until 1000, then give up
                if (!File.Exists(tmpFilePath))
                {
                    filePath = tmpFilePath;
                }
                if (i == maxCount && filePath == Application.persistentDataPath + Path.DirectorySeparatorChar + dataStorageKey + data["TeamNumber"] + ".txt")
                {
                    Debug.LogError("Too many files! Couldn't save data, not clearing");
                    return "error";
                }
            }
        }
        using (StreamWriter sw = File.CreateText(filePath))
        {
            string[] excluded = {"Pre_StartingPos","Teleop_Climb"};
            foreach (KeyValuePair<string, string> kvp in data)
            {
                if (inputs.ContainsKey(kvp.Key) && clear && !isStaticKey(kvp.Key) && (kvp.Key != excluded[0])) //Clear Function
                    inputs[kvp.Key].clearData();
                string bufferText = kvp.Value.Replace(';', ':');
                sw.WriteLine(kvp.Key + ";" + kvp.Value.Replace(',', 'ǂ'));
            }
        }
        return filePath;
    }

    private bool isStaticKey(string key)
    {
        return key == "ScouterName" || key == "ScouterTeamNumber" || key == "Version" || key == "EventKey";
    }

    public void sync() //Syncs the app to the server.
    {

      if(Application.internetReachability == NetworkReachability.NotReachable)
      {
          Debug.Log("Error. Check internet connection!");
      } else
      {
          Debug.Log("Success! Internet Avalible.");
        StartCoroutine(syncRoutine());
      }
      
    }

    public IEnumerator syncRoutine()
    {
        yield return pingTest();

        if (lastPingTest)
        {
            StartCoroutine(downloadJson()); //Downloads server data to display events and team names.
            StartCoroutine(uploadData()); //Reads .txt's the app has saved -> Formats -> Sends to GoogleForm -> GoogleForm Interprets -> GoogleForm Sends to the Google Form
        }
        else
        {
            uploadResultText.setText("Failed to connect to server,");
            uploadResultText.setText("please try again later.");
        }
    }

    private IEnumerator pingTest() //If able to connect to the server, return true. Else, return false.
    {
        UnityWebRequest pingWebRequest = UnityWebRequest.Get(serverBaseURL + "/api/v1/ping.php");
        pingWebRequest.timeout = 5;
        yield return pingWebRequest.SendWebRequest();
        string text = pingWebRequest.downloadHandler.text;
        Debug.Log(text);
        pingWebRequest.Dispose();
        lastPingTest = text == "pong"; //If text == "pong", set lastPingTest to true. If false, set it to false.
    }
    public IEnumerator downloadJson()
    {
        Debug.Log("Starting download of JSON");
        UnityWebRequest www = UnityWebRequest.Get(serverBaseURL + "/api/v1/syncDownload.php");
        {
            Debug.Log("Starting download of JSON");
            currentDownloadRequest = www;
            yield return www.SendWebRequest();
            Debug.Log("Server Files Retrived:" + www.downloadHandler.text);
            currentDownloadRequest = null;
            Debug.Log("Download Completed.");

        StartCoroutine(GetRequest(serverBaseURL,(UnityWebRequest req) =>
        {
            if (!(req.result == UnityWebRequest.Result.ProtocolError))
            {
                GetComponent<EventTeamData>().clearData();
                SyncData data = JsonUtility.FromJson<SyncData>(www.downloadHandler.text);
                GetComponent<EventTeamData>().loadData(data);

                if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "data.json"))
                File.Delete(Application.persistentDataPath + Path.DirectorySeparatorChar + "data.json");

                StreamWriter sw = File.CreateText(Application.persistentDataPath + Path.DirectorySeparatorChar + "data.json");
                sw.Write(www.downloadHandler.text);
                sw.Close();
                if (data.CurrentVersion[2] != Version[2])
                {
                    Debug.Log("Revision Version mismatch.");
                }
                if (data.CurrentVersion[1] != Version[1])
                {
                    Debug.LogWarning("Minor Version mismatch.");
                    uploadResultText.setText("Warning: Minorly out of date. Please update if possible."); //This means the app version number does not match that of the server.
                }
                if (data.CurrentVersion[0] != Version[0])
                {
                    Debug.LogWarning("Major/Year Version mismatch.");
                    uploadResultText.setText("Warning: Majorly out of date. Please update ASAP."); //This means the app version number does not match that of the server.
                }
            else
            {
                uploadResultText.setText("Download complete.");
            }
            //sw.Close();
            }
            else
            {
                uploadResultText.setText("Error encountered downloading from server.");
                Debug.LogError("Code: " + www.responseCode + " Error: " + www.error);
            }
            www.Dispose();
        }));
        }
    }
    public IEnumerator uploadData()
    {
        DirectoryInfo dinfo = new DirectoryInfo(Application.persistentDataPath);
        {

            foreach (FileInfo file in dinfo.GetFiles())
            {
                if (file.Extension.Equals(".txt") && file.Name.Contains(dataStorageKey) && !file.Name.Contains("data")){



                //Formatting in preparation for sending to GoogleForm.cs
                    Dictionary<string, string> formData = new Dictionary<string, string>();
                    formData["App"] = appName;
                    // Open the stream and read it back.
                    using (StreamReader sr = file.OpenText())
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Debug.Log(Application.persistentDataPath + "/" + file.Name);
                            string[] data = s.Split(';');

                            
                            uploadResultText.setText(file.Name + ":" + data[0] + " " + data[0]);
                            if (data[1] == "") data[1] = "empty";
                            formData[data[0]] = data[1];
                            Debug.Log(file.Name + ":" + data[0] + " " + data[1]);
                        }
                    }
                Debug.Log("Creating request");
                //UnityWebRequest uploadRequest = UnityWebRequest.Post(serverBaseURL + "/api/v1/submit.php", formData);
                StartCoroutine(GoogleForm.Post(formData, "entry.1470553737"));
                Debug.Log("Form upload begun for file " + file.Name);
//
                    /*currentUploadRequest = uploadRequest;
                    uploadRequest.chunkedTransfer = false;
                    yield return uploadRequest.SendWebRequest();
                    Debug.Log(uploadRequest.result);
                    if (uploadRequest.result != UnityWebRequest.Result.Success)
    {
        // Error
    }
    else
    {
    }
                    Debug.Log(uploadRequest.downloadHandler.text);
                    if (!uploadRequest.isHttpError && JsonUtility.FromJson<ValidatorData>(uploadRequest.downloadHandler.text).App == formData["App"])
                    {
                        file.Delete();
                    }
                    else
                    {
                        //if (!uploadRequest.isHttpError) {
                        //    Debug.LogError("!uploadRequest.isHttpError");
                            //}
                        if (!(uploadRequest.responseCode == 200))
                        {
                            uploadResultText.setText("An error has occured.");
                            Debug.LogError("Error uploading file " + file.Name + " Error Code: " + uploadRequest.responseCode);
                        }
                        continue;
                    }*/
//
                Debug.Log("Request complete.");


                yield return new WaitForSeconds(0.25f);
                } else {
                    Debug.Log(file.Name + " Failed Upload");
                }
            }
        }
        currentUploadRequest = null;
        uploadResultText.setText("Uploading Complete");
    }

  public IEnumerator GetRequest(string endpoint, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(endpoint))
        {
            // Send the request and wait for a response
            UnityWebRequestAsyncOperation test = request.SendWebRequest();
            yield return test;
            Debug.Log(test);
            callback(request);
        }
    }
}


[System.Serializable]
public class SyncData
{
    public string[] CurrentVersion;
    public EventData[] Events;
    public EventTeamList[] TeamsByEvent;
    public MatchSync[] EventMatches;
}

[System.Serializable] //I don't belive ValidatorData is used anymore. Maybe we can delete it.
public class ValidatorData //Used http://json2csharp.com/ to generate this.
{
    public string App { get; set; }
    public string Version { get; set; }
    public string ScouterName { get; set; }
    public string ScouterTeamNumber { get; set; }
    public string EventKey { get; set; }
    public string TeamNumber { get; set; }
    public string Pre_StartingPos { get; set; }
    public string Auto_CrossedBaseline { get; set; }
    public string Auto_Notes { get; set; }
    public string Auto_PlaceSwitch { get; set; }
    public string Auto_PlaceScale { get; set; }
    public string Teleop_ScalePlace { get; set; }
    public string Teleop_SwitchPlace { get; set; }
    public string Teleop_ExchangeVisit { get; set; }
    public string RobotNotes { get; set; }
    public string Teleop_Climb { get; set; }
    public string Strategy_PowerUp { get; set; }
    public string Strategy_General { get; set; }
    public string NoAlliance { get; set; }
}