using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleForm : MonoBehaviour
{
    [SerializeField] private string formURL; //Not the url that you would use to fill it out. Use inspect element to find it.
    public DataStorage DS;
    [SerializeField] string[] keys;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Post(, "entry.1470553737"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Post(Dictionary<string,string> data, string gFormID)
    {
        Debug.Log("Ran!");
        WWWForm form = new WWWForm();

        //Sends in this order
        //string[] keys = {"App","Version","ScouterName","EventKey","ScouterTeamNumber","TeamNumber","RobotNotes","Auto_LeftTarmac","Auto_BallThrown","Auto_Notes","Auto_LowerShots","Auto_UpperShots","Teleop_UpperShots","Teleop_LowerShots","Teleop_TerminalVisits","Strategy_Launchpad","Strategy_Shooting","Teleop_RequiresTerminal","Strategy_General","Teleop_Climb"};
        foreach (string key in keys)
        {
            form.AddField(gFormID,key + ";" + data[key]); //Puts data into this format: "KEY;VALUE, KEY;VALUE, KEY;VALUE"
        }
        byte[] rawData = form.data; //Puts it into raw data

        //Magic
        WWW www = new WWW(formURL, rawData); //Lets up locally to send (I think)
        yield return www;
    }
}