using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleForm : MonoBehaviour
{
    private string formURL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSeDO42dRhlJ6_7uiaSYDMpu-nietEZ5xSm9tOmCyv88KaTgOw/formResponse";
    public DataStorage DS;
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
        string[] keys = {"App","Version","ScouterName","EventKey","ScouterTeamNumber","TeamNumber","RobotNotes","Auto_LeftTarmac","Auto_BallThrown","Auto_Notes","Auto_LowerShots","Auto_UpperShots","Teleop_UpperShots","Teleop_LowerShots","Teleop_TerminalVisits","Teleop_Notes","Strategy_Launchpad","Strategy_Shooting","Teleop_RequiresTerminal","Strategy_General","Teleop_Climb"};
        foreach (string key in keys)
        {
            form.AddField(gFormID,key + ";" + data[key]);
        }
        byte[] rawData = form.data;
        WWW www = new WWW(formURL, rawData);
        yield return www;
    }
}
